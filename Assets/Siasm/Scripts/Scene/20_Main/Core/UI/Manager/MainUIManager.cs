using UnityEngine;
using System.Threading;
using System.Linq;

namespace Siasm
{
    public sealed class MainUIManager : MonoBehaviour
    {
        [SerializeField]
        private MenuArmController menuArmController;

        [SerializeField]
        private MainHUDContent mainHUDContent;

        [SerializeField]
        private MainUIContent mainUIContent;

        [SerializeField]
        private MainDirectionContent mainDirectionContent;

        private MainStateMachineController mainStateMachineController;
        private MainUseCase mainUseCase;

        public MenuArmController MenuArmController => menuArmController;
        public MainHUDContent MainHUDContent => mainHUDContent;
        public MainUIContent MainUIContent => mainUIContent;

        public void Initialize(MainUseCase mainUseCase, CancellationToken token, MainStateMachineController mainStateMachineController, BaseCameraController baseCameraController)
        {
            this.mainUseCase = mainUseCase;
            this.mainStateMachineController = mainStateMachineController;

            menuArmController.Initialize(token, mainUseCase, baseCameraController);
            menuArmController.OnShowAction = () =>
            {
                // ステートを更新
                mainStateMachineController?.ChangeMainState(MainStateMachineController.MainState.PauseAndMenu);
            };

            menuArmController.OnHidAction = () =>
            {
                // ステートを更新
                mainStateMachineController?.ChangeMainState(MainStateMachineController.MainState.FreeExploration);
            };

            mainHUDContent.Initialize(mainUseCase);
            mainUIContent.Initialize(mainUseCase);
            mainDirectionContent.Initialize();
        }

        public void Setup(SaveDataCache saveDataCache)
        {
            var enableMenus = new bool[]
            {
                true,
                true,
                true,
                true,
                true,
                true,
                false,  // NOTE: 開発途中のため
                false,  // NOTE: 開発途中のため
                false,  // NOTE: 開発途中のため
                false   // NOTE: 開発途中のため
            };

            menuArmController.Setup(enableMenus, selectedIndex: 0);

            var saveDataOwnItem = saveDataCache.SaveDataOwnItems.FirstOrDefault(x => x.ItemId == ItemConstant.EgidoId);
            if (saveDataOwnItem == null)
            {
                mainHUDContent.Setup(0);
            }
            else
            {
                mainHUDContent.Setup(saveDataOwnItem.ItemNumber);
            }

            mainUIContent.Setup();
            mainDirectionContent.Setup();
        }

        public void UpdateViewOfEgido()
        {
            var saveDataOwnItem = SaveManager.Instance.LoadedSaveDataCache.SaveDataOwnItems.FirstOrDefault(x => x.ItemId == ItemConstant.EgidoId);
            mainHUDContent.ShowHoldEgidoView(saveDataOwnItem.ItemNumber);
        }

        public void ChangeActiveOfMainQuestView(bool isActive, MainQuestView.Parameter parameter = null)
        {
            if (isActive)
            {
                mainHUDContent.ShowMainQuestView(parameter);
            }
            else
            {
                mainHUDContent.HideMainQuestView();
            }
        }

        public void ChangeActiveOfOperationTutorialView(bool isActive, int targetIndex)
        {
            if (isActive)
            {
                mainHUDContent.ShowOperationTutorialView(targetIndex);
            }
            else
            {
                mainHUDContent.HideOperationTutorialView(targetIndex);
            }
        }

        /// <summary>
        /// ESPボタンでメニュー表示を切り替え
        /// </summary>
        public void SwitchMenuArm()
        {
            // フリー探索モードとメニューを開いている時だけ変更可能
            if (mainStateMachineController.IsCurrentMainState(MainStateMachineController.MainState.FreeExploration) ||
                mainStateMachineController.IsCurrentMainState(MainStateMachineController.MainState.PauseAndMenu))
            {
                menuArmController.PlaySwitchMenuAnimation();
            }
        }

        public void ChangeHUDContent(bool isActive)
        {
            mainHUDContent.gameObject.SetActive(isActive);
        }
    }
}
