using UnityEngine;

namespace Siasm
{
    public class TargetArrowController : MonoBehaviour
    {
        private const float fighterArrowOffsetY = 1.5f;

        [SerializeField]
        private TargetArrowLineRenderer targetArrowLineRenderer;

        private PlayerBattleFighterPrefab playerBattleFighter;
        private EnemyBattleFighterPrefab enemyBattleFighter;

        public int CurrentBoxIndex { get; private set; }

        public void Initialize()
        {
            CurrentBoxIndex = -1;
            targetArrowLineRenderer.Initialize();
        }

        public void Setup(PlayerBattleFighterPrefab playerBattleFighter, EnemyBattleFighterPrefab enemyBattleFighter)
        {
            this.playerBattleFighter = playerBattleFighter;
            this.enemyBattleFighter = enemyBattleFighter;
        }

        public void ShowTargetArrow(int boxIndex)
        {
            // 更新
            CurrentBoxIndex = boxIndex;

            // 選択時のSE
            var battleCommonSETypeAudioClipsScriptableObject = AssetCacheManager.Instance.GetAsset<ScriptableObject>(BattleCommonSETypeAudioClips.AssetName);
            var battleCommonSETypeAudioClips = battleCommonSETypeAudioClipsScriptableObject as BattleCommonSETypeAudioClips;
            AudioManager.Instance.PlaySEOfAudioClip(BaseAudioPlayer.PlayType.Single, battleCommonSETypeAudioClips.GetAudioClip(AudioSEType.MatchingView));

            // バトルBoxの取得
            var playerBattleBoxPrefab = playerBattleFighter.BattleFighterBoxView.GetInstanceBattleBoxPrefab(boxIndex);
            var enemyBattleBoxPrefab = enemyBattleFighter.BattleFighterBoxView.GetInstanceBattleBoxPrefab(boxIndex);

            // ターゲットアローを表示
            if (playerBattleBoxPrefab == null && enemyBattleBoxPrefab == null)
            {
                Debug.LogWarning($"プレイヤーとエネミーの両方とも対象となるバトルボックスが存在しないためターゲットアロー表示を終了しました => boxIndex: {boxIndex}");
                return;
            }
            else if (playerBattleBoxPrefab == null)
            {
                // ターゲット先をキャラにする
                // NOTE: ダイレクトアタック用のカラーにしてもいいかも
                var playerBattleFighterPosition = playerBattleFighter.transform.position;
                playerBattleFighterPosition.y += fighterArrowOffsetY;
                targetArrowLineRenderer.ShowTargetArrow(playerBattleFighterPosition, enemyBattleBoxPrefab.transform.position);
            }
            else if (enemyBattleBoxPrefab == null)
            {
                // ターゲット先をキャラにする
                // NOTE: ダイレクトアタック用のカラーにしてもいいかも
                var enemyBattleFighterPosition = enemyBattleFighter.transform.position;
                enemyBattleFighterPosition.y += fighterArrowOffsetY;
                targetArrowLineRenderer.ShowTargetArrow(playerBattleBoxPrefab.transform.position, enemyBattleFighterPosition);
            }
            else
            {
                targetArrowLineRenderer.ShowTargetArrow(playerBattleBoxPrefab.transform.position, enemyBattleBoxPrefab.transform.position);
            }
        }

        public void HideTargetArrow()
        {
            targetArrowLineRenderer.HideTargetArrow();
        }

        /// <summary>
        /// 表示のリセットは必要なら処理を追加する
        /// </summary>
        public void ResetTargetArrow()
        {
            CurrentBoxIndex = -1;
        }
    }
}
