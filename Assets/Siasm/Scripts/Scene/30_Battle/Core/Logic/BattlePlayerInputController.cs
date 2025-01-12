using UnityEngine.InputSystem;

namespace Siasm
{
    /// <summary>
    /// キーボードやマウスのスクロールなどの入力関連の操作を管理するクラス
    /// マウスのクリックとドラッグはBattleMouseControllerで管理
    /// </summary>
    public sealed class BattlePlayerInputController : BaseInputController
    {
        private const string actionNameMenu = "Menu";
        private const string actionNameZoom = "Zoom";

        private BattleUIManager battleUIManager;
        private BattleCameraController battleCameraController;
        private BattleScenePlayerInputAction battleScenePlayerInputAction;

        public void Initialize(BattleUIManager battleUIManager, BattleCameraController battleCameraController)
        {
            this.battleUIManager = battleUIManager;
            this.battleCameraController = battleCameraController;

            battleScenePlayerInputAction = new BattleScenePlayerInputAction();
            battleScenePlayerInputAction.Enable();
            battleScenePlayerInputAction.FindAction(actionNameMenu).performed += OnMenu;
            battleScenePlayerInputAction.FindAction(actionNameZoom).performed += OnZoom;
        }

        public void Setup() { }

        private void OnMenu(InputAction.CallbackContext context)
        {
            battleUIManager.ChangeActiveBattleArmAndMenuArmAsync();
        }

        private void OnZoom(InputAction.CallbackContext context)
        {
            // メニューを開いている最中と開いている時は変更できない
            if (battleUIManager.BattleMenuArmController.CurrentPlayableParameter.IsPlaying ||
                battleUIManager.BattleMenuArmController.CurrentPlayableParameter.IsOpening)
            {
                return;
            }

            var floatValue = context.ReadValue<float>();
            battleCameraController.ChangeZoomOfMouseWheel(floatValue);
        }

        private void OnDestroy()
        {
            if (battleScenePlayerInputAction == null)
            {
                return;
            }

            battleScenePlayerInputAction.FindAction(actionNameMenu).performed -= OnMenu;
            battleScenePlayerInputAction.FindAction(actionNameZoom).performed -= OnZoom;
            battleScenePlayerInputAction.Disable();
        }
    }
}
