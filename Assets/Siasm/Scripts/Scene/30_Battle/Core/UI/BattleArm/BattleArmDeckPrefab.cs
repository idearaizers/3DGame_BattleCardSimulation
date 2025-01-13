using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace Siasm
{
    /// <summary>
    /// プレイヤーのデッキや手札の操作を行うクラス
    /// </summary>
    public class BattleArmDeckPrefab : MonoBehaviour
    {
        private readonly Quaternion deckCardQuaternion = Quaternion.Euler(0.0f, -180.0f, 90.0f);

        private const string deckStringFormat = "デッキ枚数\n{0}";
        private const float localPositionOffSetZ = 0.0015f;
        private const float handMoveSpeed = 0.2f;

        [SerializeField]
        private Transform battleCardRootTransform;

        [SerializeField]
        private PlayerBattleCardPrefab playerBattleCardPrefab;

        [SerializeField]
        private TextMeshProUGUI deckText;

        private PlayerBattleCardOperationController playerBattleCardOperationController;
        private BattleUIManager battleUIManager;
        private Camera mainCamera;

        /// <summary>
        /// 山札カード
        /// </summary>
        private List<PlayerBattleCardPrefab> instanceDeckCardPrefabs;

        /// <summary>
        /// 手札カード
        /// 演出で山札から手札に加えた際に追加する
        /// プレイを行うことで手札入れ替えなどでplayerBattleCardOperationControllerの手札クラスのモデルと表示順番が違うことがあるので注意
        /// 表示順はこちらで管理している
        /// </summary>
        private List<PlayerBattleCardPrefab> instanceHandCardPrefabs;

        public void Initialize(PlayerBattleCardOperationController playerBattleCardOperationController, BattleUIManager battleUIManager, Camera mainCamera)
        {
            this.playerBattleCardOperationController = playerBattleCardOperationController;
            this.battleUIManager = battleUIManager;
            this.mainCamera = mainCamera;
        }

        public void Setup()
        {
            instanceDeckCardPrefabs = new List<PlayerBattleCardPrefab>();
            instanceHandCardPrefabs = new List<PlayerBattleCardPrefab>();
        }

        public void ResetDeck()
        {
            for (int i = 0; i < instanceDeckCardPrefabs.Count; i++)
            {
                var instanceDeckCardPrefab = instanceDeckCardPrefabs[i];
                Destroy(instanceDeckCardPrefab.gameObject);
                instanceDeckCardPrefab = null;
            }

            for (int i = 0; i < instanceHandCardPrefabs.Count; i++)
            {
                var instanceHandCardPrefab = instanceHandCardPrefabs[i];
                Destroy(instanceHandCardPrefab.gameObject);
                instanceHandCardPrefab = null;
            }

            instanceDeckCardPrefabs.Clear();
            instanceHandCardPrefabs.Clear();
        }

        public void SetupDeck()
        { 
            // デッキモデルを基にカードを生成する
            var deckBattleCardModels = playerBattleCardOperationController.PlayerBattleCardOperationModel.DeckBattleCardModels;
            if (deckBattleCardModels.Count == 0)
            {
                Debug.LogWarning("デッキ枚数が0枚のため処理を終了しました");
                return;
            }

            // 生成と初期化
            for (int i = 0; i < deckBattleCardModels.Count; i++)
            {
                // 生成
                var instanceDeckCardPrefab = Instantiate(playerBattleCardPrefab, battleCardRootTransform);
                instanceDeckCardPrefabs.Add(instanceDeckCardPrefab);

                // 初期化
                InitializeCard(instanceDeckCardPrefab);

                // 設定
                deckBattleCardModels[i].SetCardPlaceType(CardPlaceType.Deck);
                instanceDeckCardPrefab.Setup(deckBattleCardModels[i]);

                // 座標を変更
                // indexの0番が一番手前に来るように配置
                instanceDeckCardPrefab.transform.localPosition = new Vector3(0.0f, 0.055f, -localPositionOffSetZ * i);
                instanceDeckCardPrefab.transform.localRotation = deckCardQuaternion;
            }

            // デッキ枚数の表示を更新
            deckText.text = string.Format(deckStringFormat, instanceDeckCardPrefabs.Count);
        }

        public void PlayDrawCardAnimation()
        {
            PlayDrawCardAnimationAsync().Forget();
        }

        private async UniTask PlayDrawCardAnimationAsync()
        {
            await UniTask.CompletedTask;

            // 手札のモデルクラスを引いて更新する
            playerBattleCardOperationController.DrawHandCard();

            // 画面に反映するために値を習得する
            var handBattleCardModels = playerBattleCardOperationController.PlayerBattleCardOperationModel.HandBattleCardModels;

            // カードを引く前に現在の手札の位置を変更する
            for (int i = 0; i < instanceHandCardPrefabs.Count; i++)
            {
                var handPositionY = GetHandPositionY(handBattleCardModels.Count, i);
                _ = instanceHandCardPrefabs[i].transform.DOLocalMove(new Vector3(0.0f, handPositionY, 0.0f), handMoveSpeed);
            }

            // 山札からカードを引く
            // 現在の手札数を引いた残りの数を山札から引く
            for (int i = instanceHandCardPrefabs.Count; i < handBattleCardModels.Count; i++)
            {
                // デッキにあるのか確認するためにBattleCardModelを取得する
                // 指定のBattleCardModelがデッキにあるのか確認
                var battleCardModels = instanceDeckCardPrefabs.Select(playerBattleCardPrefab => playerBattleCardPrefab.CurrentBattleCardModel).ToArray();
                var deckBattleCardModelIndex = Array.IndexOf(battleCardModels, handBattleCardModels[i]);

                // 取得先がなければ既に手札にあるものとして次の処理を行う
                if (deckBattleCardModelIndex == -1)
                {
                    // デッキに指定のカードがなかったので生成して手札に追加する
                    // TODO: 生成するものがもしもあれば追加実装する

                    AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);

                    // 手札に生成して加える
                    AddHandCard(handBattleCardModels[i]);

                    // 演出用に少し待機
                    await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

                    // Debug.LogWarning($"デッキに指定のカードモデルが存在しなかったためドロー処理をスキップしました => handCardModelIndex: {i}");
                    continue;
                }

                AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);

                // デッキから手札に移動した状態に変更する
                var drawDeckCardPrefab = instanceDeckCardPrefabs[deckBattleCardModelIndex];

                // 山札のリストから取り除く
                instanceDeckCardPrefabs.RemoveAt(deckBattleCardModelIndex);

                // 状態を変更
                drawDeckCardPrefab.SetIsHand(CardPlaceType.Hand);

                // 移動先を取得する
                var handPositionY = GetHandPositionY(handBattleCardModels.Count, i);

                // カードの位置を変更する
                _ = drawDeckCardPrefab.transform.DOLocalMove(new Vector3(0.0f, handPositionY, 0.0f), handMoveSpeed);

                // 管理先を変更
                instanceHandCardPrefabs.Add(drawDeckCardPrefab);

                // デッキ枚数の表示を更新
                deckText.text = string.Format(deckStringFormat, instanceDeckCardPrefabs.Count);

                // 演出用に少し待機
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            }
        }

        public void AddHandCard(BattleCardModel battleCardModel)
        {
            // 生成
            var instanceHandCardPrefab = Instantiate(playerBattleCardPrefab, battleCardRootTransform);
            instanceHandCardPrefabs.Add(instanceHandCardPrefab);

            // 初期化
            InitializeCard(instanceHandCardPrefab);

            // 設定
            battleCardModel.SetCardPlaceType(CardPlaceType.Hand);
            instanceHandCardPrefab.Setup(battleCardModel);

            // 状態を変更
            instanceHandCardPrefab.SetIsHand(CardPlaceType.Hand);

            // 配置場所の移動
            // 一番手前に来るように配置
            instanceHandCardPrefab.transform.localPosition = new Vector3(0.0f, 0.055f, -localPositionOffSetZ * 0);
            instanceHandCardPrefab.transform.localRotation = deckCardQuaternion;

            // 全ての手札を最新の位置に移動する
            for (int i = 0; i < instanceHandCardPrefabs.Count; i++)
            {
                var handPositionY = GetHandPositionY(instanceHandCardPrefabs.Count, i);
                _ = instanceHandCardPrefabs[i].transform.DOLocalMove(new Vector3(0.0f, handPositionY, 0.0f), handMoveSpeed);
            }
        }

        private void InitializeCard(PlayerBattleCardPrefab playerBattleCardPrefab)
        {
            // 初期化
            playerBattleCardPrefab.Initialize(mainCamera);

            // 
            playerBattleCardPrefab.OnMousePointerEntoryAction = (battleCardModel) =>
            {
                battleUIManager.BattleHUDController.BattleCardDetailHUDPrefab.PlayShowAnimation(battleCardModel);
            };

            // 
            playerBattleCardPrefab.OnReturnPositionAction = (playerBattleCardPrefab) =>
            {
                // 何番目の手札にあるのか取得
                var handIndex = Array.IndexOf(instanceHandCardPrefabs.ToArray(), playerBattleCardPrefab);

                // 移動前の手札の位置に戻す
                var handPositionY = GetHandPositionY(instanceHandCardPrefabs.Count, handIndex);
                instanceHandCardPrefabs[handIndex].transform.DOLocalMove(new Vector3(0.0f, handPositionY, 0.0f), handMoveSpeed);
            };

            // 
            playerBattleCardPrefab.OnMouseDragEndAction = (playerBattleCardPrefab) =>
            {
                // 非表示にする
                battleUIManager.BattleHUDController.BattleCardDetailHUDPrefab.PlayHideAnimation();

                // 何番目の手札にあるのか取得
                var handIndex = Array.IndexOf(instanceHandCardPrefabs.ToArray(), playerBattleCardPrefab);
                var instanceHandCardPrefab = instanceHandCardPrefabs[handIndex];
                instanceHandCardPrefabs.RemoveAt(handIndex);
                Destroy(instanceHandCardPrefab.gameObject);
                instanceHandCardPrefab = null;

                // 手札の位置を更新する
                for (int i = 0; i < instanceHandCardPrefabs.Count; i++)
                {
                    var handPositionY = GetHandPositionY(instanceHandCardPrefabs.Count, i);
                    instanceHandCardPrefabs[i].transform.DOLocalMove(new Vector3(0.0f, handPositionY, 0.0f), handMoveSpeed);
                }
            };

            // 
            playerBattleCardPrefab.OnMouseDragEndOfChangeCardAction = (playerBattleCardPrefab, battleCardModel) =>
            {
                // 非表示にする
                battleUIManager.BattleHUDController.BattleCardDetailHUDPrefab.PlayHideAnimation();

                // 何番目の手札にあるのか取得
                var handIndex = Array.IndexOf(instanceHandCardPrefabs.ToArray(), playerBattleCardPrefab);
                var instanceHandCardPrefab = instanceHandCardPrefabs[handIndex];
                instanceHandCardPrefabs.RemoveAt(handIndex);
                Destroy(instanceHandCardPrefab.gameObject);
                instanceHandCardPrefab = null;

                // 手札に加える
                AddHandCard(battleCardModel);
            };
        }

        /// <summary>
        /// 右が一番引いた順番が古いカードになる
        /// NOTE: 将来的には計算式でまとめた方がよさそう
        /// </summary>
        /// <param name="handNumber"></param>
        /// <param name="handIndex"></param>
        /// <returns></returns>
        private float GetHandPositionY(int handNumber, int handIndex)
        {
            if (handNumber > 10)
            {
                Debug.LogWarning($"手札が11枚以上の場合は処理の見直しが必要のため手札枚数を1とした値を返しています => handNumber: {handNumber}");
                handNumber = 1;
            }

            // 手札の枚数が偶数か奇数かによって配置場所を変える
            var handPositionY = 0.0f;
            if (handNumber % 2 == 0)
            {
                if (handNumber == 10)
                {
                    handPositionY = 0.605f + (0.1f * (handNumber - 4)) - (0.105f * handIndex);
                }
                else if (handNumber == 8)
                {
                    handPositionY = 0.605f + (0.1f * (handNumber - 2)) - (0.135f * handIndex);
                }
                else if (handNumber == 6)
                {
                    handPositionY = 0.605f + (0.1f * handNumber) - (0.19f * handIndex);
                }
                else
                {
                    handPositionY = 0.605f + (0.1f * handNumber) - (0.2f * handIndex);
                }
            }
            else
            {
                if (handNumber == 9)
                {
                    handPositionY = 0.605f + (0.1f * (handNumber - 3)) - (0.115f * handIndex);
                }
                else if (handNumber == 7)
                {
                    handPositionY = 0.605f + (0.1f * (handNumber - 1)) - (0.16f * handIndex);
                }
                else
                {
                    handPositionY = 0.605f + (0.1f * handNumber) - (0.2f * handIndex);
                }
            }

            return handPositionY;
        }

        // async UniTask
        public void PlayDeckChange()
        {
            // 手札をすべてデッキの場所に移動させる

            AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);

            // 
            foreach (var instanceHandCardPrefab in instanceHandCardPrefabs)
            {
                // TODO: 必要なら高さ位置を考慮する
                instanceHandCardPrefab.transform.DOLocalMove(new Vector3(0.0f, 0.05f, -0.0075f), handMoveSpeed);
                
                // 
                instanceHandCardPrefab.SetIsHand(CardPlaceType.None);
                instanceDeckCardPrefabs.Add(instanceHandCardPrefab);
            }

            // 全てを空にする
            instanceHandCardPrefabs.Clear();

            // デッキ枚数の表示を更新
            deckText.text = string.Format(deckStringFormat, instanceDeckCardPrefabs.Count);

            // 一度、全てのカードを破棄した方がいいかも
        }

        public void ChangeDeck(int deckIndex)
        {
            // playerBattleCardOperationController.ChangeDeck(deckIndex);
        }

        private void OnDestroy()
        {
            if (instanceDeckCardPrefabs != null)
            {
                for (int i = 0; i < instanceDeckCardPrefabs.Count; i++)
                {
                    var instanceDeckCardPrefab = instanceDeckCardPrefabs[i];
                    Destroy(instanceDeckCardPrefab.gameObject);
                    instanceDeckCardPrefab = null;
                }
            }

            if (instanceHandCardPrefabs != null)
            {
                for (int i = 0; i < instanceHandCardPrefabs.Count; i++)
                {
                    var instanceHandCardPrefab = instanceHandCardPrefabs[i];
                    Destroy(instanceHandCardPrefab.gameObject);
                    instanceHandCardPrefab = null;
                }
            }

            instanceDeckCardPrefabs.Clear();
            instanceHandCardPrefabs.Clear();
        }
    }
}
