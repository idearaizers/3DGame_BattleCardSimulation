using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Siasm
{
    public sealed class MainPlayerInputController : BaseInputController
    {
        private const string actionNameMenu = "Menu";
        private const string actionNameZoom = "Zoom";

        private MainUIManager mainUIManager;
        private MainCameraController mainCameraController;
        private MainUseCase mainUseCase;

        // 仮
        private MainScenePlayerInputAction mainScenePlayerInputAction;

        public void Initialize(MainUIManager mainUIManager, MainCameraController mainCameraController, MainUseCase mainUseCase)
        {
            this.mainUIManager = mainUIManager;
            this.mainCameraController = mainCameraController;
            this.mainUseCase = mainUseCase;

            mainScenePlayerInputAction = new MainScenePlayerInputAction();
            mainScenePlayerInputAction.Enable();
            mainScenePlayerInputAction.FindAction(actionNameMenu).performed += OnMenu;

            // NOTE: 一旦ズームはoffにする
            // mainScenePlayerInputAction.FindAction(actionNameZoom).performed += OnZoom;
        }

        public void Setup() { }

        private void OnMenu(InputAction.CallbackContext context)
        {
            // AtoBデバイスを所持していれば使用可能
            var saveDataOwnItem = mainUseCase.LoadedSaveDataCache.SaveDataOwnItems.FirstOrDefault(saveDataOwnItem => saveDataOwnItem.ItemId == ItemConstant.AtoBDeviceId);
            if (saveDataOwnItem != null)
            {
                mainUIManager.SwitchMenuArm();
            }
        }

        private void OnZoom(InputAction.CallbackContext context)
        {
            float zoomValue = context.ReadValue<float>();
            mainCameraController.OnZoomAndOut(zoomValue);
        }

        /// <summary>
        /// 一応、破棄処理を実装
        /// </summary>
        private void OnDestroy()
        {
            mainScenePlayerInputAction.FindAction(actionNameMenu).performed -= OnMenu;
            mainScenePlayerInputAction.Disable();

            // 破棄
            mainScenePlayerInputAction.Dispose();

            // NOTE: 一旦ズームはoffにする
            // mainScenePlayerInputAction.FindAction(actionNameZoom).performed -= OnZoom;
        }
    }
}
