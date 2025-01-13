using UnityEngine;

namespace Siasm
{
    public class BattleArmPrefab : MonoBehaviour
    {
        [SerializeField]
        private BattleArmDeckPrefab battleArmDeckPrefab;

        [SerializeField]
        private Animator animator;

        public BattleArmDeckPrefab BattleArmDeckPrefab => battleArmDeckPrefab;

        public void Initialize(PlayerBattleCardOperationController playerBattleCardOperationController, BattleUIManager battleUIManager, Camera mainCamera)
        {
            battleArmDeckPrefab.Initialize(playerBattleCardOperationController, battleUIManager, mainCamera);
        }

        public void Setup()
        {
            battleArmDeckPrefab.Setup();
        }

        public bool IsLeftArmHideAnimation()
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // 指定したアニメーション名が再生中で、かつnormalizedTimeが1以上なら終了している
            return stateInfo.IsName("LeftArm_Hide") && stateInfo.normalizedTime >= 1.0f;
        }

        public void PlayDrawCardAnimation()
        {
            battleArmDeckPrefab.PlayDrawCardAnimation();
        }

        public void PlayDeckChange()
        {
            battleArmDeckPrefab.PlayDeckChange();
        }
    }
}
