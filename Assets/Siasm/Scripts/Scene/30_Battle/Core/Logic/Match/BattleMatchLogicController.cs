using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace Siasm
{
    /// <summary>
    /// マッチ関連の処理をまとめたクラス
    /// プレイヤーとエネミーは手札の概念がある
    /// デッキ切れの際はシャッフルカードを使用することで山札をリセットできる
    /// デッキから引くカードがないとシャッフルカードを引く。シャッフルカードは威力が低いので注意
    /// 思考停止の際はルーレットの目が必ず最低になる
    /// 数字の引き分けが10回続くとそのバトルは引き分けになる：バースト。バースト用のスキルもある
    /// バトルマッチにてバトルボックスに設定したカードは必ず消費される
    /// </summary>
    public class BattleMatchLogicController : MonoBehaviour
    {
        private static readonly Vector3 zoomOutPositionOfPlayer = new Vector3(-5.0f, 0.0f, -3.0f);
        private static readonly Vector3 zoomOutPositionOfEnemy = new Vector3( 5.0f, 0.0f, -3.0f);
        private static readonly Vector3 zoomOutPositionOfDraw = new Vector3(0.0f, 0.0f, -3.0f);

        private const float zoomOutSpeedOfDamage = 0.2f;
        private const float cameraMoveOffsetPositionX = 2.5f;
        private const float nextMatchTime = 0.3f;

        [SerializeField]
        private BattleMatchReelLogic battleMatchReelLogic;

        [SerializeField]
        private BattleMatchEffect battleMatchEffect;

        private CancellationToken token;
        private BattleSpaceManager battleSpaceManager;
        private BattleUIManager battleUIManager;
        private BattleAbilityLogicController battleAbilityLogicController;
        private PlayerBattleFighterPrefab playerBattleFighterPrefab;
        private EnemyBattleFighterPrefab enemyBattleFighterPrefab;

        public void Initialize(CancellationToken token, BattleSpaceManager battleSpaceManager, BattleUIManager battleUIManager,
            BattleAbilityLogicController battleAbilityLogicController, BattleConfigDebug battleConfigDebug, BattleObjectPoolContainer battleObjectPoolContainer,
            Camera mainCamera)
        {
            this.token = token;
            this.battleSpaceManager = battleSpaceManager;
            this.battleUIManager = battleUIManager;
            this.battleAbilityLogicController = battleAbilityLogicController;

            battleMatchReelLogic.Initialize(battleConfigDebug);
            battleMatchEffect.Initialize(battleObjectPoolContainer, mainCamera);
        }

        public void Setup(PlayerBattleFighterPrefab playerBattleFighterPrefab, EnemyBattleFighterPrefab enemyBattleFighterPrefab)
        {
            this.playerBattleFighterPrefab = playerBattleFighterPrefab;
            this.enemyBattleFighterPrefab = enemyBattleFighterPrefab;

            battleMatchReelLogic.Setup();
            battleMatchEffect.Setup();
        }

        /// <summary>
        /// バトルを行う
        /// リールの値は演出の直前で決定させていてモデルクラスを基に演出を再生させていないので注意
        /// </summary>
        /// <returns></returns>
        public async UniTask StartCombatAsync()
        {
            // バトルを行う最大回数を取得する
            var playerBattleBoxCount = playerBattleFighterPrefab.BattleFighterBoxView.InstanceBattleBoxPrefabs.Count;
            var enemyBattleBoxCount = enemyBattleFighterPrefab.BattleFighterBoxView.InstanceBattleBoxPrefabs.Count;
            var maxBattleBoxCount = Mathf.Max(playerBattleBoxCount, enemyBattleBoxCount);

            // バトルボックスの数だけリールマッチを実行
            for (int i = 0; i < maxBattleBoxCount; i++)
            {
                // バトルボックスから使用するバトルカードモデルを取得
                var playerBattleCardModel = playerBattleFighterPrefab.BattleFighterBoxView.GetBattleCardModelOfBattleBox(i);
                var enemyBattleCardModel = enemyBattleFighterPrefab.BattleFighterBoxView.GetBattleCardModelOfBattleBox(i);

                // 二人とも攻撃するためのバトルカードモデルがない場合は次の処理を実行する
                if (playerBattleCardModel == null &&
                    enemyBattleCardModel == null)
                {
                    continue;
                }

                // 演出前に待機モーションに変更
                playerBattleFighterPrefab.BattleFighterAnimation.SetImage(BattleFighterAnimationType.Idle);
                enemyBattleFighterPrefab.BattleFighterAnimation.SetImage(BattleFighterAnimationType.Idle);

                // 演出で使用するパラメータを用意
                var startMatchReelParameter = new StartMatchReelParameter
                {
                    PlayerReelParameter = new ReelParameter
                    {
                        BaseBattleFighterPrefab = playerBattleFighterPrefab,
                        IsThinkingFreeze = playerBattleFighterPrefab.IsThinkingFreeze,
                        BattleCardModel = playerBattleCardModel,
                        RemainingBattleCardNumber = playerBattleBoxCount - i - 1 // 残り数のため-1しています
                    },
                    EnemyReelParameter = new ReelParameter
                    {
                        BaseBattleFighterPrefab = enemyBattleFighterPrefab,
                        IsThinkingFreeze = enemyBattleFighterPrefab.IsThinkingFreeze,
                        BattleCardModel = enemyBattleCardModel,
                        RemainingBattleCardNumber = enemyBattleBoxCount - i - 1 // 残り数のため-1しています
                    }
                };

                // マッチリールを実行する
                await StartMatchReelAync(token, startMatchReelParameter);

                // 一度、マッチリールを非表示にする
                playerBattleFighterPrefab.BattleMatchReelView.Hide();
                enemyBattleFighterPrefab.BattleMatchReelView.Hide();

                // どちらかがやられていれば処理を終了する
                if (enemyBattleFighterPrefab.IsDead ||
                    playerBattleFighterPrefab.IsDead)
                {
                    return;
                }
            }

            // 見栄え用に少しだけ待機
            await UniTask.Delay(TimeSpan.FromSeconds(0.4f), cancellationToken: token);
        }

        /// <summary>
        /// マッチリールを実行する
        /// </summary>
        /// <param name="token"></param>
        /// <param name="startMatchReelParameter"></param>
        /// <returns></returns>
        public async UniTask StartMatchReelAync(CancellationToken token, StartMatchReelParameter startMatchReelParameter)
        {
            // 指定回数マッチが続いたらバースト（強制終了）にする
            var drawCount = 0;

            // 結果が決まるまで繰り返し実行する
            while (true)
            {
                if (drawCount == BattleConstant.LimitMatchDrawCount)
                {
                    Debug.LogWarning($"{BattleConstant.LimitMatchDrawCount}回引き分けのためこのマッチは強制終了しました");
                    break;
                }

                // <===== 移動関連 =====>
                // カメラをマッチ場所に移動させる
                var matchPositon = GetMatchPositonOfCamera(startMatchReelParameter);
                battleSpaceManager.PlayMoveCameraAnimation(matchPositon, 0.5f, Ease.InOutSine);

                // 攻撃位置にファーターを移動する
                await battleSpaceManager.MoveFighterAnimationAsync(token, startMatchReelParameter);

                if (drawCount >= 3)
                {
                    // 見栄えのために待機なし
                }
                else if (drawCount >= 2)
                {
                    // 見栄えのために少し待機
                    await UniTask.Delay(TimeSpan.FromSeconds(0.05f), cancellationToken: token);
                }
                else
                {
                    // 見栄えのために少し待機
                    await UniTask.Delay(TimeSpan.FromSeconds(0.05f), cancellationToken: token);
                }

                // <===== リール演出関連 =====>
                // リールの回転演出を開始する
                battleUIManager.PlayReelDirection(startMatchReelParameter);

                if (drawCount >= 2)
                {
                    // 見栄えのために少し待機
                    await UniTask.Delay(TimeSpan.FromSeconds(0.05f), cancellationToken: token);
                }
                else
                {
                    // 見栄えのために少し待機
                    await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);
                }

                // <===== リール結果を表示関連 =====>
                // リール結果を取得して表示する
                var playerReelNumber = battleMatchReelLogic.GetReelNumber(startMatchReelParameter.PlayerReelParameter);
                var enemyReelNumber = battleMatchReelLogic.GetReelNumber(startMatchReelParameter.EnemyReelParameter);

                // リール値を変更
                // 一旦、そのまま値をプラスさせる
                // 将来的には見た目に分かるように反映したい
                var playerAddReelNumber = battleAbilityLogicController.GetAddReelNumber(startMatchReelParameter.PlayerReelParameter.BattleCardModel, playerReelNumber);
                var enemyAddReelNumber = battleAbilityLogicController.GetAddReelNumber(startMatchReelParameter.EnemyReelParameter.BattleCardModel, enemyReelNumber);
                playerReelNumber += playerAddReelNumber;
                enemyReelNumber += enemyAddReelNumber;

                // リール値を表示する
                await battleUIManager.StopReelDirectionAsync(token, playerReelNumber, enemyReelNumber);

                // <===== リール結果を基に演出を再生関連 =====>
                // リールタイプを取得
                var playerCardReelType = startMatchReelParameter.PlayerReelParameter.GetCardReelType();
                var enemyCardReelType = startMatchReelParameter.EnemyReelParameter.GetCardReelType();

                // ===========================================================
                // [マッチリールパターン]
                // ・攻撃 vs 攻撃
                // ・攻撃 vs None
                // ・None vs 攻撃
                // 
                // [マッチ勝利効果]
                // ・負けた方は攻撃ができないので一方的に攻撃される
                // ・リスクとリターンが高い
                // 
                // ・状態異常の付与については攻撃後にしているが調整してもいいかも
                // ・ダメージ後に状態異常の付与の表示がほしいかも
                // ===========================================================
                if ((playerCardReelType == CardReelType.Attack && enemyCardReelType == CardReelType.Attack) ||
                    (playerCardReelType == CardReelType.Attack && enemyCardReelType == CardReelType.None) ||
                    (playerCardReelType == CardReelType.None && enemyCardReelType == CardReelType.Attack))
                {
                    if (playerReelNumber > enemyReelNumber)
                    {
                        // 結果は確定しているので演出前に状態異常の付与を実行
                        battleAbilityLogicController.ExecuteBattleCardEffectOfPlayerInvoker(CardAbilityActivateType.Succeeded, startMatchReelParameter.PlayerReelParameter.BattleCardModel);
                        battleAbilityLogicController.ExecuteBattleCardEffectOfEnemyInvoker(CardAbilityActivateType.Failed, startMatchReelParameter.EnemyReelParameter.BattleCardModel);
                        await PlayDamageDirectionAsync(playerReelNumber, playerBattleFighterPrefab, enemyBattleFighterPrefab, isAttack: true, startMatchReelParameter);
                        break;
                    }
                    else if (playerReelNumber < enemyReelNumber)
                    {
                        await PlayDamageDirectionAsync(enemyReelNumber, enemyBattleFighterPrefab, playerBattleFighterPrefab, isAttack: true, startMatchReelParameter);
                        battleAbilityLogicController.ExecuteBattleCardEffectOfPlayerInvoker(CardAbilityActivateType.Failed, startMatchReelParameter.PlayerReelParameter.BattleCardModel);
                        battleAbilityLogicController.ExecuteBattleCardEffectOfEnemyInvoker(CardAbilityActivateType.Succeeded, startMatchReelParameter.EnemyReelParameter.BattleCardModel);
                        break;
                    }
                    else
                    {
                        // 引き分けなのでカウントを増やして再度ループを実行する
                        drawCount++;

                        battleAbilityLogicController.ExecuteBattleCardEffectOfPlayerInvoker(CardAbilityActivateType.Drawn, startMatchReelParameter.EnemyReelParameter.BattleCardModel);
                        battleAbilityLogicController.ExecuteBattleCardEffectOfEnemyInvoker(CardAbilityActivateType.Drawn, startMatchReelParameter.PlayerReelParameter.BattleCardModel);
                        await PlayDrawDirectionAsync(isAttack: true, drawCount);
                    }
                }
                // ===========================================================
                // [マッチリールパターン]
                // ・攻撃 vs 防御
                // ・防御 vs 攻撃
                // 
                // [マッチ勝利効果]
                // ・ガード側はダメージを軽減する
                // ・ガード側は相手から自分に付与される状態異常を無効化できる
                // ・ローリスクで比較的に安全に対処できる
                // ・エネミーは特殊な敵でなければほとんど使用してこないので主にプレイヤーが使用する
                // ===========================================================
                else if ((playerCardReelType == CardReelType.Attack && enemyCardReelType == CardReelType.Guard) ||
                        (playerCardReelType == CardReelType.Guard && enemyCardReelType == CardReelType.Attack))
                {
                    if (playerCardReelType == CardReelType.Attack)
                    {
                        battleAbilityLogicController.ExecuteBattleCardEffectOfPlayerInvokerToGuardSucceeded(CardAbilityActivateType.Succeeded, startMatchReelParameter.PlayerReelParameter.BattleCardModel);
                        battleAbilityLogicController.ExecuteBattleCardEffectOfEnemyInvokerToGuardSucceeded(CardAbilityActivateType.Failed, startMatchReelParameter.EnemyReelParameter.BattleCardModel);
                        await PlayAttackAndGuardDirectionAsync(playerReelNumber, playerBattleFighterPrefab, enemyReelNumber, enemyBattleFighterPrefab, startMatchReelParameter);
                        break;
                    }
                    else
                    {
                        battleAbilityLogicController.ExecuteBattleCardEffectOfPlayerInvokerToGuardSucceeded(CardAbilityActivateType.Failed, startMatchReelParameter.PlayerReelParameter.BattleCardModel);
                        battleAbilityLogicController.ExecuteBattleCardEffectOfEnemyInvokerToGuardSucceeded(CardAbilityActivateType.Succeeded, startMatchReelParameter.EnemyReelParameter.BattleCardModel);
                        await PlayAttackAndGuardDirectionAsync(enemyReelNumber, enemyBattleFighterPrefab, playerReelNumber, playerBattleFighterPrefab, startMatchReelParameter);
                        break;
                    }
                }
                // ===========================================================
                // [マッチリールパターン]
                // ・防御 vs 防御
                // ・防御 vs None
                // ・None vs 防御
                // 
                // [マッチ勝利効果]
                // ・ガードに対するカウンターの役割で、負けた方はガードが出来ない
                // ・攻撃と同じで負けた方はダメージを受ける。ガード崩しなイメージ
                // ・こちらもリスクとリターンが高いのでリスクを抑えたい場合は不要な攻撃カードで対処した方が良い
                // ===========================================================
                else if ((playerCardReelType == CardReelType.Guard && enemyCardReelType == CardReelType.Guard) ||
                        (playerCardReelType == CardReelType.Guard && enemyCardReelType == CardReelType.None) ||
                        (playerCardReelType == CardReelType.None && enemyCardReelType == CardReelType.Guard))
                {
                    if (playerReelNumber > enemyReelNumber)
                    {
                        battleAbilityLogicController.ExecuteBattleCardEffectOfPlayerInvoker(CardAbilityActivateType.Succeeded, startMatchReelParameter.PlayerReelParameter.BattleCardModel);
                        battleAbilityLogicController.ExecuteBattleCardEffectOfEnemyInvoker(CardAbilityActivateType.Failed, startMatchReelParameter.EnemyReelParameter.BattleCardModel);
                        await PlayDamageDirectionAsync(playerReelNumber, playerBattleFighterPrefab, enemyBattleFighterPrefab, isAttack: false, startMatchReelParameter);
                        break;
                    }
                    else if (playerReelNumber < enemyReelNumber)
                    {
                        battleAbilityLogicController.ExecuteBattleCardEffectOfPlayerInvoker(CardAbilityActivateType.Failed, startMatchReelParameter.PlayerReelParameter.BattleCardModel);
                        battleAbilityLogicController.ExecuteBattleCardEffectOfEnemyInvoker(CardAbilityActivateType.Succeeded, startMatchReelParameter.EnemyReelParameter.BattleCardModel);
                        await PlayDamageDirectionAsync(enemyReelNumber, enemyBattleFighterPrefab, playerBattleFighterPrefab, isAttack: false, startMatchReelParameter);
                        break;
                    }
                    else
                    {
                        // 引き分けなのでカウントを増やして再度ループを実行する
                        drawCount++;

                        battleAbilityLogicController.ExecuteBattleCardEffectOfPlayerInvoker(CardAbilityActivateType.Drawn, startMatchReelParameter.EnemyReelParameter.BattleCardModel);
                        battleAbilityLogicController.ExecuteBattleCardEffectOfEnemyInvoker(CardAbilityActivateType.Drawn, startMatchReelParameter.PlayerReelParameter.BattleCardModel);
                        await PlayDrawDirectionAsync(isAttack: false, drawCount);
                    }
                }
                // ===========================================================
                // それ以外
                // 処理を追加する必要があるのでエラーを表示
                // ===========================================================
                else
                {
                    Debug.LogError("CardReelTypeの条件式が足りないため正しく処理が行われていないです");
                    break;
                }
            }
        }

        /// <summary>
        /// 攻撃勝利と防御勝利の共通で使用
        /// </summary>
        /// <param name="damageNumber"></param>
        /// <param name="sourceBattleFighterPrefab"></param>
        /// <param name="targetBattleFighterPrefab"></param>
        /// <param name="isAttack"></param>
        /// <returns></returns>
        private async UniTask PlayDamageDirectionAsync(int damageNumber, BaseBattleFighterPrefab sourceBattleFighterPrefab, BaseBattleFighterPrefab targetBattleFighterPrefab, bool isAttack, StartMatchReelParameter startMatchReelParameter)
        {
            // 攻撃時のSE
            var battleCommonSETypeAudioClipsScriptableObject = AssetCacheManager.Instance.GetAsset<ScriptableObject>(BattleCommonSETypeAudioClips.AssetName);
            var battleCommonSETypeAudioClips = battleCommonSETypeAudioClipsScriptableObject as BattleCommonSETypeAudioClips;
            AudioManager.Instance.PlaySEOfAudioClip(BaseAudioPlayer.PlayType.Single, battleCommonSETypeAudioClips.GetAudioClip(AudioSEType.Strike1));

            var battleCardModel = sourceBattleFighterPrefab is PlayerBattleFighterPrefab
                ? startMatchReelParameter.PlayerReelParameter.BattleCardModel
                : startMatchReelParameter.EnemyReelParameter.BattleCardModel;

            // TODO: 属性相性などの表示を画面上に追加する

            var (resultDamageNumber, attributeResistType) = battleAbilityLogicController.GetAddDamageNumberOfEmotionAttributeType(damageNumber, startMatchReelParameter, sourceBattleFighterPrefab, targetBattleFighterPrefab);

            // ダメージ値を反映
            targetBattleFighterPrefab.ApplyDamage(resultDamageNumber);

            // TODO: ダメージを受けたキャラを上下にシェイクさせる

            // 少しズームアウトする
            if (targetBattleFighterPrefab.IsPlayer)
            {
                var targetPosition = battleSpaceManager.BattleCameraController.MainCamera.transform.localPosition + zoomOutPositionOfPlayer;
                battleSpaceManager.BattleCameraController.PlayMoveAnimationAsync(Vector3.zero, targetPosition, zoomOutSpeedOfDamage, Ease.OutQuad).Forget();
            }
            else
            {
                var targetPosition = battleSpaceManager.BattleCameraController.MainCamera.transform.localPosition + zoomOutPositionOfEnemy;
                battleSpaceManager.BattleCameraController.PlayMoveAnimationAsync(Vector3.zero, targetPosition, zoomOutSpeedOfDamage, Ease.OutQuad).Forget();
            }

            // ファイターの表示を変更
            var sourceBattleFighterAnimationType = isAttack
                ? BattleFighterAnimationType.Attack
                : BattleFighterAnimationType.Guard;

            sourceBattleFighterPrefab.BattleFighterAnimation.SetImage(sourceBattleFighterAnimationType);
            targetBattleFighterPrefab.BattleFighterAnimation.SetImage(BattleFighterAnimationType.TakeDamage);

            // ダメージを表示
            battleMatchEffect.ShowDamageEffec(damageNumber, resultDamageNumber, targetBattleFighterPrefab, attributeResistType, battleCardModel);

            // フリーズ状態を確認して必要なら表示
            battleMatchEffect.ShowThinkingFreezeEffect(targetBattleFighterPrefab);

            // ダメージを受けたファイターを後退させる
            await targetBattleFighterPrefab.BattleFighterMovement.MoveKnockBackAsync(Ease.OutQuad);

            // 見栄えの緩急用に少し待機
            await UniTask.Delay(TimeSpan.FromSeconds(nextMatchTime), cancellationToken: token);
        }

        /// <summary>
        /// 攻撃勝利と防御勝利の共通で使用
        /// </summary>
        /// <param name="isAttack"></param>
        /// <returns></returns>
        private async UniTask PlayDrawDirectionAsync(bool isAttack, int drawCount)
        {
            // 引き分けの時のSE
            var battleCommonSETypeAudioClipsScriptableObject = AssetCacheManager.Instance.GetAsset<ScriptableObject>(BattleCommonSETypeAudioClips.AssetName);
            var battleCommonSETypeAudioClips = battleCommonSETypeAudioClipsScriptableObject as BattleCommonSETypeAudioClips;
            AudioManager.Instance.PlaySEOfAudioClip(BaseAudioPlayer.PlayType.Single, battleCommonSETypeAudioClips.GetAudioClip(AudioSEType.BattleDraw));

            // ファイターの表示を変更
            var battleFighterAnimationType = isAttack
                ? BattleFighterAnimationType.Attack
                : BattleFighterAnimationType.Guard;

            playerBattleFighterPrefab.BattleFighterAnimation.SetImage(battleFighterAnimationType);
            enemyBattleFighterPrefab.BattleFighterAnimation.SetImage(battleFighterAnimationType);

            // 少しズームアウトする
            var targetPosition = battleSpaceManager.BattleCameraController.MainCamera.transform.localPosition + zoomOutPositionOfDraw;
            battleSpaceManager.BattleCameraController.PlayMoveAnimationAsync(Vector3.zero, targetPosition, zoomOutSpeedOfDamage, Ease.OutQuad).Forget();

            battleMatchEffect.ShowDrawEffect(playerBattleFighterPrefab, enemyBattleFighterPrefab, drawCount);

            // ファイターを移動させる
            await UniTask.WhenAll
            (
                playerBattleFighterPrefab.BattleFighterMovement.MoveBackwardAsync(Ease.OutQuad),
                enemyBattleFighterPrefab.BattleFighterMovement.MoveBackwardAsync(Ease.OutQuad)
            );

            // 見栄えの緩急用に少し待機
            // 2回目以降はスピードを上げる
            if (drawCount >= 2)
            {
                // 見栄え用に待機なし
            }
            else
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f), cancellationToken: token);
            }
        }

        /// <summary>
        /// 攻撃vs防御の演出を再生
        /// </summary>
        /// <param name="attackedNumber"></param>
        /// <param name="attackedBattleFighter"></param>
        /// <param name="guardNumber"></param>
        /// <param name="guardedBattleFighter"></param>
        /// <returns></returns>
        private async UniTask PlayAttackAndGuardDirectionAsync(int attackedNumber, BaseBattleFighterPrefab attackedBattleFighter,
            int guardNumber, BaseBattleFighterPrefab guardedBattleFighter, StartMatchReelParameter startMatchReelParameter)
        {
            // 攻撃成功時のSE
            var battleCommonSETypeAudioClipsScriptableObject = AssetCacheManager.Instance.GetAsset<ScriptableObject>(BattleCommonSETypeAudioClips.AssetName);
            var battleCommonSETypeAudioClips = battleCommonSETypeAudioClipsScriptableObject as BattleCommonSETypeAudioClips;
            AudioManager.Instance.PlaySEOfAudioClip(BaseAudioPlayer.PlayType.Single, battleCommonSETypeAudioClips.GetAudioClip(AudioSEType.Strike1));

            // TODO: 属性相性などの表示を画面上に追加する

            battleAbilityLogicController.AttackAndGuard(startMatchReelParameter, attackedBattleFighter, guardedBattleFighter);

            // ダメージ値を反映
            // 0でも表示する
            var damageNumber = Mathf.Clamp(attackedNumber - guardNumber, 0, 999);
            guardedBattleFighter.ApplyDamage(damageNumber);

            if (guardedBattleFighter.IsPlayer)
            {
                var targetPosition = battleSpaceManager.BattleCameraController.MainCamera.transform.localPosition + zoomOutPositionOfPlayer;
                battleSpaceManager.BattleCameraController.PlayMoveAnimationAsync(Vector3.zero, targetPosition, zoomOutSpeedOfDamage, Ease.OutQuad).Forget();
            }
            else
            {
                var targetPosition = battleSpaceManager.BattleCameraController.MainCamera.transform.localPosition + zoomOutPositionOfEnemy;
                battleSpaceManager.BattleCameraController.PlayMoveAnimationAsync(Vector3.zero, targetPosition, zoomOutSpeedOfDamage, Ease.OutQuad).Forget();
            }

            // ファイターの表示を変更
            attackedBattleFighter.BattleFighterAnimation.SetImage(BattleFighterAnimationType.Attack);
            guardedBattleFighter.BattleFighterAnimation.SetImage(BattleFighterAnimationType.Guard);

            // エフェクトを表示
            battleMatchEffect.ShowGuardEffec(damageNumber, guardedBattleFighter);

            // フリーズ状態を確認して必要なら表示
            battleMatchEffect.ShowThinkingFreezeEffect(guardedBattleFighter);

            // ファイターを移動させる
            await guardedBattleFighter.BattleFighterMovement.MoveKnockBackAsync();

            // 見栄えの緩急用に少し待機
            await UniTask.Delay(TimeSpan.FromSeconds(nextMatchTime), cancellationToken: token);
        }

        private Vector3 GetMatchPositonOfCamera(StartMatchReelParameter startMatchReelParameter)
        {
            var targetPositionX = 0.0f;

            // バトルガードがない場合はその場から少し離れた場所を中心にする
            if (startMatchReelParameter.PlayerReelParameter.BattleCardModel == null)
            {
                targetPositionX = playerBattleFighterPrefab.transform.localPosition.x + cameraMoveOffsetPositionX;
            }
            else if (startMatchReelParameter.EnemyReelParameter.BattleCardModel == null)
            {
                targetPositionX = enemyBattleFighterPrefab.transform.localPosition.x - cameraMoveOffsetPositionX;
            }
            else
            {
                // プレイヤーとエネミーの間の地点を取得する
                targetPositionX = (playerBattleFighterPrefab.transform.localPosition.x + enemyBattleFighterPrefab.transform.localPosition.x) / 2;
            }

            // 移動位置
            return new Vector3(targetPositionX, -0.7f, 8.0f);
        }
    }
}
