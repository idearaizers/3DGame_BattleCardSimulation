using System;
using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// NOTE: バトルでは表示項目を変えたいのでリファクタ予定
    /// </summary>
    public class CommonMenuArmPrefab : MonoBehaviour
    {
        private const string closeButtonName = "閉じる";

        private readonly string[] commonMenuNames = new string[]
        {
            "アイテム",     // 初期から解放
            "デッキ",       // 初期から解放
            "設定",         // 初期から解放
            "セーブ",       // 初期から解放
            "ロード",       // 初期から解放
            "収容記録",     // 特定のアイテムで解放
            "ステータス",   // 特定のアイテムで解放
            "外見",         // 特定のアイテムで解放
            "リワード",     // 特定のアイテムで解放
            "名刺"          // 特定のアイテムで解放
        };

        private readonly string[] battleMenuNames = new string[]
        {
            "アイテム", 
            "デッキ",
            "分析",
            "設定",
            "ヘルプ",
            "中段",
            "07",       // 空きで未使用
            "08",       // 空きで未使用
            "09",       // 空きで未使用
            "10"        // 空きで未使用
        };

        [Header("RightSide関連")]
        [SerializeField]
        private SideArmSwitcherPrefab sideArmSwitcherPrefab;

        [SerializeField]
        private ArmSwitcherPrefab armSwitcherPrefabOfCloseButton;

        [Header("MenuMonitor関連")]
        [SerializeField]
        private MenuMonitorPrefab menuMonitorPrefab;

        [Header("LeftSide関連")]
        [SerializeField]
        private GameObject leftSideArmSwitcherRootGameObject;

        [SerializeField]
        private ArmSwitcherPrefab[] leftArmSwitcherPrefabs;

        public Action OnCloseMenuAction { get; set; }
        public Action<int> OnDeckChangeAction { get; set; }
        public Action OnEscapeAction { get; set; }

        /// <summary>
        /// NOTE: リファクタが完了したら削除予定
        /// </summary>
        private bool isBattle;

        public void Initialize(BaseUseCase baseUseCase, BaseCameraController baseCameraController, bool isBattle = false, BattleSpaceManager battleSpaceManager = null)
        {
            this.isBattle = isBattle;

            // RightSide関連
            sideArmSwitcherPrefab.Initialize(baseCameraController.UICamera);
            armSwitcherPrefabOfCloseButton.Initialize(baseCameraController.UICamera, isLeftSide: false);
            armSwitcherPrefabOfCloseButton.OnClickAction = (armSwitcherPrefab) => OnCloseMenuAction?.Invoke();

            // MenuMonitor関連
            menuMonitorPrefab.Initialize(sideArmSwitcherPrefab, leftArmSwitcherPrefabs, baseUseCase, baseCameraController, isBattle, battleSpaceManager);
            menuMonitorPrefab.OnCloseAction = () => OnCloseMenuAction?.Invoke();
            menuMonitorPrefab.OnDeckChangeAction = (deckIndex) => OnDeckChangeAction?.Invoke(deckIndex);
            menuMonitorPrefab.OnEscapeAction = () => OnEscapeAction?.Invoke();

            // LeftSide関連
            for (int i = 0; i < leftArmSwitcherPrefabs.Length; i++)
            {
                leftArmSwitcherPrefabs[i].Initialize(baseCameraController.UICamera, isLeftSide: true);
                leftArmSwitcherPrefabs[i].OnClickAction = OnClickMenuButton;
            }

            // カーソルをボタンに被せた際に反応させたいか変更
            ChangeActiveCanvas(false);
        }

        public void Setup(bool[] activeMenus, int selectedIndex)
        {
            // RightSide関連
            sideArmSwitcherPrefab.Setup();
            armSwitcherPrefabOfCloseButton.Setup(closeButtonName, isActive: true);

            // MenuMonitor関連
            menuMonitorPrefab.Setup(activeMenus, selectedIndex);

            // LeftSide関連
            var currentMenuNames = isBattle
                ? battleMenuNames
                : commonMenuNames;

            for (int i = 0; i < leftArmSwitcherPrefabs.Length; i++)
            {
                leftArmSwitcherPrefabs[i].Setup(currentMenuNames[i], activeMenus[i]);
            }
        }

        public void ChangeActiveCanvas(bool isActive)
        {
            menuMonitorPrefab.ChangeActiveCanvas(isActive);
        }

        public void ChangeMenuContent(int selectedIndex)
        {
            menuMonitorPrefab.ChangeMenuContent(selectedIndex);
        }

        public void ChangeActiveOfLeftSideArm(bool isActive)
        {
            leftSideArmSwitcherRootGameObject.SetActive(isActive);
        }

        public void UpdateMenuContents(BaseMenuPrefab.BaseMenuPrefabParameter baseMenuPrefabParameter)
        {
            menuMonitorPrefab.UpdateMenuContents(baseMenuPrefabParameter);
        }

        public void ChangeActiveMenu(bool isActive)
        {
            menuMonitorPrefab.ChangeActiveMenu(isActive);
        }

        /// <summary>
        /// サイドアームが表示状態ならクローズする
        /// </summary>
        public void PlayCloseOfSideArmAnimation()
        {
            if (sideArmSwitcherPrefab.IsOpen)
            {
                sideArmSwitcherPrefab.PlayCloseAnimation();
            }
        }

        private void OnClickMenuButton(ArmSwitcherPrefab armSwitcherPrefab)
        {
            var selectedIndex = Array.IndexOf(leftArmSwitcherPrefabs, armSwitcherPrefab);
            menuMonitorPrefab.ChangeMenuContent(selectedIndex);
        }

        public void ShowDialogMenu(DialogMenuType dialogMenuType, BaseMenuDialogPrefab.BaseParameter dialogParameter)
        {
            menuMonitorPrefab.ShowDialogMenu(dialogMenuType, dialogParameter);
        }
    }
}
