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

        /// <summary>
        /// NOTE: 正しく取れていないみたい？？
        /// </summary>
        /// <returns></returns>
        public bool IsLeftArmHideAnimation()
        {
            // 現在のAnimatorState情報を取得（レイヤー0）
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

        public void PlayHide()
        {
            animator.Play("LeftArm_Hide");
        }

        public void PlayShow()
        {
            animator.Play("LeftArm_Show");
        }
    }
}
