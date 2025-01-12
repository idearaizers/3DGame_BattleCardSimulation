using System;
using UnityEngine;

namespace Siasm
{
    public class BattleHUDController : MonoBehaviour
    {
        [Header("上部関連")]
        [SerializeField]
        private CombatStartHUDArmPrefab combatStartHUDArmPrefab;

        [SerializeField]
        private PlayerMatchBattleCardHUDPrefab playerMatchBattleCardHUDPrefab;

        [SerializeField]
        private EnemyMatchBattleCardHUDPrefab enemyMatchBattleCardHUDPrefab;

        [SerializeField]
        private BattleCardDetailHUDPrefab battleCardDetailHUDPrefab;

        [Header("中部関連")]
        [SerializeField]
        private PlayerFighterStatusHUDArmPrefab playerFighterStatusHUDArmPrefab;

        [SerializeField]
        private EnemyFighterStatusHUDArmPrefab enemyFighterStatusHUDArmPrefab;

        [Header("下部関連")]
        [SerializeField]
        private MenuHUDArmPrefab menuHUDArmPrefab;

        public BattleCardDetailHUDPrefab BattleCardDetailHUDPrefab => battleCardDetailHUDPrefab;

        public Action OnCombatStartction { get; set; }
        public Action OnMenuButtonAction { get; set; }
        public Action<bool> OnFighterStatusAction { get; set; }

        public void Initialize(Camera uiCamera, BattleObjectPoolContainer battleObjectPoolContainer)
        {
            // 上部関連
            combatStartHUDArmPrefab.Initialize(uiCamera, BaseHUDArmPrefab.ButtonOrientationType.Center);
            combatStartHUDArmPrefab.OnClickAction = () => OnCombatStartction?.Invoke();
            playerMatchBattleCardHUDPrefab.Initialize(uiCamera);
            enemyMatchBattleCardHUDPrefab.Initialize(uiCamera);
            battleCardDetailHUDPrefab.Initialize(uiCamera);

            // 中部関連
            playerFighterStatusHUDArmPrefab.Initialize(true, uiCamera, BaseHUDArmPrefab.ButtonOrientationType.Right, battleObjectPoolContainer);
            playerFighterStatusHUDArmPrefab.OnFighterStatusAction = (isPlayer) => OnFighterStatusAction.Invoke(isPlayer);
            enemyFighterStatusHUDArmPrefab.Initialize(false, uiCamera, BaseHUDArmPrefab.ButtonOrientationType.Left, battleObjectPoolContainer);
            enemyFighterStatusHUDArmPrefab.OnFighterStatusAction = (isPlayer) => OnFighterStatusAction.Invoke(isPlayer); 

            // 下部関連
            menuHUDArmPrefab.Initialize(uiCamera, BaseHUDArmPrefab.ButtonOrientationType.Left);
            menuHUDArmPrefab.OnClickAction = () => OnMenuButtonAction?.Invoke();
        }

        public void Setup(BaseBattleFighterModel playerBattleFighterModel, BaseBattleFighterModel enemyBattleFighterModel)
        {
            // 上部関連
            combatStartHUDArmPrefab.Setup();
            playerMatchBattleCardHUDPrefab.Setup();
            enemyMatchBattleCardHUDPrefab.Setup();
            battleCardDetailHUDPrefab.Setup();

            // 中部関連
            playerFighterStatusHUDArmPrefab.Setup(playerBattleFighterModel);
            enemyFighterStatusHUDArmPrefab.Setup(enemyBattleFighterModel);

            // 下部関連
            menuHUDArmPrefab.Setup();
        }

        /// <summary>
        /// 戦闘開始ボタンやファイターステータスなどのHUDを表示する
        /// </summary>
        /// <param name="playerBattleFighterModel">nullの場合は表示内容を更新しない</param>
        /// <param name="enemyBattleFighterModel">nullの場合は表示内容を更新しない</param>
        public void ShowAllHUD(BaseBattleFighterModel playerBattleFighterModel = null, BaseBattleFighterModel enemyBattleFighterModel = null)
        {
            // 上部関連
            combatStartHUDArmPrefab.PlayShowAnimation();
            // matchBattleCardPrefabは選択した時に表示するのでこのタイミングでは表示しない
            // battleCardDetailHUDPrefabは選択した時に表示するのでこのタイミングでは表示しない

            // 中部関連
            playerFighterStatusHUDArmPrefab.PlayShowAnimation(playerBattleFighterModel);
            enemyFighterStatusHUDArmPrefab.PlayShowAnimation(enemyBattleFighterModel);

            // 下部関連
            menuHUDArmPrefab.PlayShowAnimation();
        }

        /// <summary>
        /// 戦闘開始ボタンやファイターステータスなどのHUDを非表示にする
        /// </summary>
        public void HideAllHUD()
        {
            // 上部関連
            combatStartHUDArmPrefab.PlayHideAnimation();
            playerMatchBattleCardHUDPrefab.PlayHideAnimation();
            enemyMatchBattleCardHUDPrefab.PlayHideAnimation();
            battleCardDetailHUDPrefab.PlayHideAnimation();

            // 中部関連
            playerFighterStatusHUDArmPrefab.PlayHideAnimation();
            enemyFighterStatusHUDArmPrefab.PlayHideAnimation();

            // 下部関連
            menuHUDArmPrefab.PlayHideAnimation();
        }

        /// <summary>
        /// 指定されたカードを基にマッチ予定のバトルカードを表示する
        /// </summary>
        /// <param name="playerBattleCardModel">バトルボックスに設定されているカードを渡す</param>
        /// <param name="enemyBattleCardModel">バトルボックスに設定されているカードを渡す</param>
        public void ShowDisplayMatchBattleCardHUDPrefab(BattleCardModel playerBattleCardModel = null, BattleCardModel enemyBattleCardModel = null)
        {
            if (playerBattleCardModel != null)
            {
                playerMatchBattleCardHUDPrefab.PlayShowAnimation(playerBattleCardModel);
            }
            else
            {
                playerMatchBattleCardHUDPrefab.PlayHideAnimation();
            }

            if (enemyBattleCardModel != null)
            {
                enemyMatchBattleCardHUDPrefab.PlayShowAnimation(enemyBattleCardModel);
            }
            else
            {
                enemyMatchBattleCardHUDPrefab.PlayHideAnimation();
            }
        }
    }
}
