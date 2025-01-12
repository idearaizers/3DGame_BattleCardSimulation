using System.Linq;
using UnityEngine.InputSystem;

namespace Siasm
{
    public sealed class MainPlayerInputController : BaseInputController
    {
        private const string actionNameMenu = "Menu";

        private MainUIManager mainUIManager;
        private MainUseCase mainUseCase;

        private MainScenePlayerInputAction mainScenePlayerInputAction;

        public void Initialize(MainUIManager mainUIManager, MainUseCase mainUseCase)
        {
            this.mainUIManager = mainUIManager;
            this.mainUseCase = mainUseCase;

            mainScenePlayerInputAction = new MainScenePlayerInputAction();
            mainScenePlayerInputAction.Enable();
            mainScenePlayerInputAction.FindAction(actionNameMenu).performed += OnMenu;
        }

        public void Setup() { }

        /// <summary>
        /// AtoBデバイスを所持していればメニューは使用可能になる
        /// </summary>
        /// <param name="context"></param>
        private void OnMenu(InputAction.CallbackContext context)
        {
            var saveDataOwnItem = mainUseCase.LoadedSaveDataCache.SaveDataOwnItems.FirstOrDefault(saveDataOwnItem => saveDataOwnItem.ItemId == ItemConstant.AtoBDeviceId);
            if (saveDataOwnItem != null)
            {
                mainUIManager.SwitchMenuArm();
            }
        }

        private void OnDestroy()
        {
            if (mainScenePlayerInputAction == null)
            {
                return;
            }

            mainScenePlayerInputAction.FindAction(actionNameMenu).performed -= OnMenu;
            mainScenePlayerInputAction.Disable();
        }
    }
}
