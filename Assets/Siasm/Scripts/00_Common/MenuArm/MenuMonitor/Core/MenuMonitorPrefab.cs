using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Siasm
{
    public class MenuMonitorPrefab : MonoBehaviour
    {
        [SerializeField]
        private Canvas canvas;

        /// <summary>
        /// これはバトルとそれ以外で変えるかな
        /// </summary>
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


        [SerializeField]
        private MenuPrefabTypePrefabDictionary menuPrefabTypePrefabDictionary;



        // [Space]
        // [SerializeField]
        // private MenuPrefabTypePrefabs menuPrefabTypePrefabs;


        private SideArmSwitcherPrefab sideArmSwitcherPrefab;
        private ArmSwitcherPrefab[] leftArmSwitcherPrefabs;
        private BaseUseCase baseUseCase;
        private BaseCameraController baseCameraController;

        /// <summary>
        /// -1の場合は未選択の状態
        /// </summary>
        private int currentSelectedIndex;

        /// <summary>
        /// 破棄は生成時に前のものが残っていた際に行っています
        /// </summary>
        private GameObject currentInstanceDialogGameObject;

        public Action OnCloseAction { get; set; }

        public Action<int> OnDeckChangeAction { get; set; }
        public Action OnEscapeAction { get; set; }

        private bool isBattle;
        private List<BaseMenuPrefab> currentBaseMenuPrefabs;

        public void Initialize(SideArmSwitcherPrefab sideArmSwitcherPrefab, ArmSwitcherPrefab[] leftArmSwitcherPrefabs, BaseUseCase baseUseCase, BaseCameraController baseCameraController, bool isBattle = false, BattleSpaceManager battleSpaceManager = null)
        {
            this.sideArmSwitcherPrefab = sideArmSwitcherPrefab;
            this.leftArmSwitcherPrefabs = leftArmSwitcherPrefabs;
            this.baseUseCase = baseUseCase;
            this.baseCameraController = baseCameraController;

            // ここで初期化するページを設定できるようにするかな
            // そうすれば必要なことろだけ拡張できるかあ
            // 一旦、仮で実装をするかな
            this.isBattle = isBattle;

            // 仮
            currentBaseMenuPrefabs = new List<BaseMenuPrefab>();

            //　
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
            else
            {
                currentBaseMenuPrefabs = baseMenuPrefabs.ToList();
            }

            // 
            for (int i = 0; i < currentBaseMenuPrefabs.Count; i++)
            {
                currentBaseMenuPrefabs[i].Initialize(
                    sideArmSwitcherPrefab,
                    baseUseCase,
                    baseCameraController,
                    battleSpaceManager.PlayerBattleFighterSpawnController,
                    battleSpaceManager.EnemyBattleFighterSpawnController
                );

                //　仮
                var battleDeckMenuPrefab = currentBaseMenuPrefabs[i] as BattleDeckMenuPrefab;
                if (battleDeckMenuPrefab)
                {
                    battleDeckMenuPrefab.OnDeckChangeAction = (deckIndex) =>
                    {
                        OnDeckChangeAction?.Invoke(deckIndex);
                    };
                }

                //　仮
                var battleEscapeMenuPrefab = currentBaseMenuPrefabs[i] as BattleEscapeMenuPrefab;
                if (battleEscapeMenuPrefab)
                {
                    battleEscapeMenuPrefab.OnEscapeAction = () =>
                    {
                        OnEscapeAction?.Invoke();
                    };
                }

                // 一旦すべて非表示にする
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
                    // 指定したメニューを表示状態にする
                    currentBaseMenuPrefabs[i].Enable();

                    // 関連するメニューボタンも選択状態にする
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
        /// カーソルをボタンに被せた際に反応させたいか変更
        /// </summary>
        /// <param name="isActive"></param>
        public void ChangeActiveCanvas(bool isActive)
        {
            if (isActive)
            {
                canvas.worldCamera = baseCameraController.UICamera;

                // TODO: 見直した方がよさそう
                // if (isBattle)
                // {
                //     EventSystemManager.Instance.EventSystem.enabled = true;
                // }
            }
            else
            {
                // canvas.worldCamera = null;
                canvas.worldCamera = baseCameraController.UICamera;

                // TODO: 見直した方がよさそう
                // if (isBattle)
                // {
                //     EventSystemManager.Instance.EventSystem.enabled = false;
                // }
            }
        }

        public void ShowDialogMenu(DialogMenuType dialogMenuType, BaseMenuDialogPrefab.BaseParameter dialogParameter)
        {
            // 生成済みのものが残っていた場合は生成前に破棄を行う
            if (currentInstanceDialogGameObject != null)
            {
                Destroy(currentInstanceDialogGameObject);
                currentInstanceDialogGameObject = null;
            }

            // 生成
            var dialogMenuPrefabGameObject = dialogMenuPrefabs.GetGameObject(dialogMenuType);
            var dialogMenuPrefabInstanceGameObject = Instantiate(dialogMenuPrefabGameObject, menuDialogRootGameObject.transform);
            currentInstanceDialogGameObject = dialogMenuPrefabInstanceGameObject;

            // 初期化
            switch (dialogMenuType)
            {
                case DialogMenuType.YesNo:
                    var yesNoMenuDialogPrefab = dialogMenuPrefabInstanceGameObject.GetComponent<YesNoMenuDialogPrefab>();
                    yesNoMenuDialogPrefab.Initialize(sideArmSwitcherPrefab, baseUseCase, baseCameraController);
                    yesNoMenuDialogPrefab.OnCloseAction = () => OnCloseAction?.Invoke();
                    yesNoMenuDialogPrefab.Setup();
                    yesNoMenuDialogPrefab.Show(dialogParameter as YesNoMenuDialogPrefab.DialogParameter);
                    break;

                case DialogMenuType.EgidoDelivery:
                    var egidoDeliveryDialogPrefab = dialogMenuPrefabInstanceGameObject.GetComponent<EgidoDeliveryMenuDialogPrefab>();
                    egidoDeliveryDialogPrefab.Initialize(sideArmSwitcherPrefab, baseUseCase, baseCameraController);
                    egidoDeliveryDialogPrefab.OnCloseAction = () => OnCloseAction?.Invoke();
                    egidoDeliveryDialogPrefab.Setup();
                    egidoDeliveryDialogPrefab.Show(dialogParameter as EgidoDeliveryMenuDialogPrefab.DialogParameter);
                    break;

                case DialogMenuType.CreatureBox:
                    var creatureBoxMenuDialogPrefab = dialogMenuPrefabInstanceGameObject.GetComponent<CreatureBoxMenuDialogPrefab>();
                    creatureBoxMenuDialogPrefab.Initialize(sideArmSwitcherPrefab, baseUseCase, baseCameraController);
                    creatureBoxMenuDialogPrefab.OnCloseAction = () => OnCloseAction?.Invoke();
                    creatureBoxMenuDialogPrefab.Setup();
                    creatureBoxMenuDialogPrefab.Show(dialogParameter as CreatureBoxMenuDialogPrefab.DialogParameter);
                    break;

                case DialogMenuType.CreatureAdmission:
                    var admissionMenuDialogPrefab = dialogMenuPrefabInstanceGameObject.GetComponent<CreatureAdmissionMenuDialogPrefab>();
                    admissionMenuDialogPrefab.Initialize(sideArmSwitcherPrefab, baseUseCase, baseCameraController);
                    admissionMenuDialogPrefab.OnCloseAction = () => OnCloseAction?.Invoke();
                    admissionMenuDialogPrefab.Setup();
                    admissionMenuDialogPrefab.Show(dialogParameter as CreatureAdmissionMenuDialogPrefab.DialogParameter);
                    break;

                default:
                    break;
            }
        }

        private void OnDestroy()
        {
            if (currentInstanceDialogGameObject != null)
            {
                Destroy(currentInstanceDialogGameObject);
                currentInstanceDialogGameObject = null;
            }
        }
    }
}
