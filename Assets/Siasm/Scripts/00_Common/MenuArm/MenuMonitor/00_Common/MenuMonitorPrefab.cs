using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// TODO: 現状、バトルでのみ表示項目を変えたいので処理を見直し予定
    /// </summary>
    public class MenuMonitorPrefab : MonoBehaviour
    {
        [SerializeField]
        private Canvas canvas;

        [Space]
        [SerializeField]
        private GameObject menuPrefabRootGameObject;

        [SerializeField]
        private GameObject menuDialogRootGameObject;

        [Space]
        [SerializeField]
        private BaseMenuPrefab[] baseMenuPrefabs;

        [Space]
        [SerializeField]
        private DialogMenuPrefabs dialogMenuPrefabs;

        /// <summary>
        /// バトルで使用するメニュー項目をまとめたもの
        /// 並び順は自由に変えれるようにしたいのでDictionaryで格納している
        /// </summary>
        [SerializeField]
        private MenuPrefabTypePrefabDictionary menuPrefabTypePrefabDictionary;

        private SideArmSwitcherPrefab sideArmSwitcherPrefab;
        private ArmSwitcherPrefab[] leftArmSwitcherPrefabs;
        private BaseUseCase baseUseCase;
        private BaseCameraController baseCameraController;

        /// <summary>
        /// -1の場合は未選択の状態
        /// </summary>
        private int currentSelectedIndex;

        /// <summary>
        /// 使用しているメニュー項目の一覧
        /// シーン場所によって表示項目を変えたいので処理を見直し予定
        /// </summary>
        private List<BaseMenuPrefab> currentBaseMenuPrefabs;

        private GameObject currentInstanceMenuDialogPrefab;

        public Action OnCloseAction { get; set; }
        public Action<int> OnDeckChangeAction { get; set; }
        public Action OnEscapeAction { get; set; }

        public void Initialize(SideArmSwitcherPrefab sideArmSwitcherPrefab, ArmSwitcherPrefab[] leftArmSwitcherPrefabs, BaseUseCase baseUseCase, BaseCameraController baseCameraController,
            bool isBattle = false, BattleSpaceManager battleSpaceManager = null)
        {
            this.sideArmSwitcherPrefab = sideArmSwitcherPrefab;
            this.leftArmSwitcherPrefabs = leftArmSwitcherPrefabs;
            this.baseUseCase = baseUseCase;
            this.baseCameraController = baseCameraController;

            // 表示項目を格納する
            currentBaseMenuPrefabs = baseMenuPrefabs.ToList();

            // バトルの場合は表示項目を変えたいので管理したいメニュー項目を上書きで格納する
            // NOTE: 設定値については別で管理に変えたいので見直し予定
            if (isBattle)
            {
                var menuPrefabTypes = new MenuPrefabType[]
                {
                    MenuPrefabType.Item,
                    MenuPrefabType.BattleDeck,
                    MenuPrefabType.BattleAnalyze,
                    MenuPrefabType.Settig,
                    MenuPrefabType.BattleHelp,
                    MenuPrefabType.BattleEscape
                };

                foreach (var menuPrefabType in menuPrefabTypes)
                {
                    var menuPrefabGameObject = menuPrefabTypePrefabDictionary[menuPrefabType];
                    currentBaseMenuPrefabs.Add(menuPrefabGameObject.GetComponent<BaseMenuPrefab>());
                }
            }

            // 格納した表示項目を初期化する
            for (int i = 0; i < currentBaseMenuPrefabs.Count; i++)
            {
                currentBaseMenuPrefabs[i].Initialize(
                    sideArmSwitcherPrefab,
                    baseUseCase,
                    baseCameraController,
                    battleSpaceManager?.PlayerBattleFighterSpawnController,     // バトル以外では参照先がないのでnullチェックしている
                    battleSpaceManager?.EnemyBattleFighterSpawnController       // バトル以外では参照先がないのでnullチェックしている
                );

                // 特定のコンポーネントについてAction処理を実行する
                var battleDeckMenuPrefab = currentBaseMenuPrefabs[i] as BattleDeckMenuPrefab;
                if (battleDeckMenuPrefab)
                {
                    battleDeckMenuPrefab.OnDeckChangeAction = (deckIndex) => OnDeckChangeAction?.Invoke(deckIndex);
                }

                var battleEscapeMenuPrefab = currentBaseMenuPrefabs[i] as BattleEscapeMenuPrefab;
                if (battleEscapeMenuPrefab)
                {
                    battleEscapeMenuPrefab.OnEscapeAction = () => OnEscapeAction?.Invoke();
                }

                // 管理しやすいように一旦、全て非表示にする
                currentBaseMenuPrefabs[i].Disable();
            }
        }

        public void Setup(bool[] enableMenus, int selectedIndex)
        {
            for (int i = 0; i < currentBaseMenuPrefabs.Count; i++)
            {
                currentBaseMenuPrefabs[i].Setup(enableMenus[i]);

                if (i == selectedIndex)
                {
                    currentBaseMenuPrefabs[i].Enable();
                    leftArmSwitcherPrefabs[i].SetColorOfButtonImage(true);
                }
            }

            currentSelectedIndex = selectedIndex;
        }

        public void ChangeMenuContent(int selectedIndex)
        {
            // 同じ項目を再表示しようとした場合は処理を行わない
            if (currentSelectedIndex == selectedIndex)
            {
                return;
            }

            // メニュー項目を切り替え
            // 元から表示していた項目
            if (currentSelectedIndex != -1)
            {
                currentBaseMenuPrefabs[currentSelectedIndex].HideChangeContent();
                leftArmSwitcherPrefabs[currentSelectedIndex].SetColorOfButtonImage(false);
            }

            // 新しく表示する項目
            currentBaseMenuPrefabs[selectedIndex].ShowChangeContent();

            // ボタンの色を変更
            leftArmSwitcherPrefabs[selectedIndex].SetColorOfButtonImage(true);

            // サイドアームが開いていれば閉じる
            if (sideArmSwitcherPrefab.IsOpen)
            {
                sideArmSwitcherPrefab.PlayCloseAnimation();
            }

            // 選択中のindexを更新
            currentSelectedIndex = selectedIndex;
        }

        public void UpdateMenuContents(BaseMenuPrefab.BaseMenuPrefabParameter baseMenuPrefabParameter)
        {
            foreach (var baseMenuPrefab in currentBaseMenuPrefabs)
            {
                baseMenuPrefab.UpdateContent(baseMenuPrefabParameter);
            }
        }

        public void ChangeActiveMenu(bool isActive)
        {
            menuPrefabRootGameObject.SetActive(isActive);
            menuDialogRootGameObject.SetActive(!isActive);
        }

        /// <summary>
        /// カーソルをボタンに被せた際の反応是非を切り替える
        /// キャンバスのカメラの参照を外すことでマウスカーソルを乗せた際に反応しないようにしている
        /// </summary>
        /// <param name="isEnable"></param>
        public void ChangeActiveCanvas(bool isEnable)
        {
            if (isEnable)
            {
                canvas.worldCamera = baseCameraController.UICamera;
            }
            else
            {
                canvas.worldCamera = baseCameraController.UICamera;
            }
        }

        /// <summary>
        /// DialogMenuに指定したprefabを生成して表示する
        /// </summary>
        /// <param name="dialogMenuType"></param>
        /// <param name="dialogParameter"></param>
        public void ShowDialogMenu(DialogMenuType dialogMenuType, BaseMenuDialogPrefab.BaseParameter dialogParameter)
        {
            // 生成済みのものが残っていた場合は先に破棄を行う
            if (currentInstanceMenuDialogPrefab != null)
            {
                Destroy(currentInstanceMenuDialogPrefab);
                currentInstanceMenuDialogPrefab = null;
            }

            var dialogMenuPrefabGameObject = dialogMenuPrefabs.GetGameObject(dialogMenuType);
            var instanceMenuDialogPrefab = Instantiate(dialogMenuPrefabGameObject, menuDialogRootGameObject.transform);
            currentInstanceMenuDialogPrefab = instanceMenuDialogPrefab;

            var baseMenuDialogPrefab = instanceMenuDialogPrefab.GetComponent<BaseMenuDialogPrefab>();
            baseMenuDialogPrefab.Initialize(sideArmSwitcherPrefab, baseUseCase, baseCameraController);
            baseMenuDialogPrefab.OnCloseAction = () => OnCloseAction?.Invoke();
            baseMenuDialogPrefab.Setup();
            baseMenuDialogPrefab.Show(dialogParameter);
        }

        private void OnDestroy()
        {
            if (currentInstanceMenuDialogPrefab != null)
            {
                Destroy(currentInstanceMenuDialogPrefab);
                currentInstanceMenuDialogPrefab = null;
            }
        }
    }
}
