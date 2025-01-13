using System;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Linq;

namespace Siasm
{
    public sealed class BattleDeckMenuPrefab : BaseMenuPrefab
    {
        private const string notChangeMessageText = "カードを設定しているので変更ができません";
        private const string deckSaveMessageText = "このデッキに変えますか？";

        [SerializeField]
        private TabGroup tabGroup;

        [SerializeField]
        private BattleDeckMenuCardDetialView battleDeckMenuCardDetialView;

        [SerializeField]
        private BattleDeckMenuCardScrollController battleDeckMenuCardScrollController;

        private GameObject currentSelectedGameObject;
        private List<MenuDeckCardModel> currentDeckCardModels;
        private List<MenuOwnCardModel> currentOwnCardModels;
        private int currentBeforeDeckIndex = -1;
        private int currentDeckIndex = -1;

        public Action<int> OnDeckChangeAction { get; set; }

        public override void Initialize(SideArmSwitcherPrefab sideArmSwitcherPrefab, BaseUseCase baseUseCase, BaseCameraController baseCameraController,
            PlayerBattleFighterSpawnController playerBattleFighterSpawnController, EnemyBattleFighterSpawnController enemyBattleFighterSpawnController)
        {
            base.Initialize(sideArmSwitcherPrefab, baseUseCase, baseCameraController, playerBattleFighterSpawnController, enemyBattleFighterSpawnController);

            var activeTabIndex = 0;
            tabGroup.SetActiveTab(activeTabIndex);
            tabGroup.OnChangeActiveTab = OnChangeActiveTab;

            battleDeckMenuCardDetialView.Initialize();
            battleDeckMenuCardScrollController.Initialize();
            battleDeckMenuCardScrollController.OnClickAction = OnSelectedBattleCard;
        }

        public override void Setup(bool isEnable)
        {
            base.Setup(isEnable);

            if (!isEnable)
            {
                return;
            }

            tabGroup.Setup();
            battleDeckMenuCardDetialView.Setup();

            // 非アクティブの時にセットアップを行うと参照エラーが出るのでアクティブに切り替えて実行して変更前に戻す
            var activeSelf = gameObject.activeSelf;
            gameObject.gameObject.SetActive(true);

            // 一旦、ダミーデータで対応
            var deckCardModelsDammy = new MenuDeckCardModel[] { };
            battleDeckMenuCardScrollController.Setup(deckCardModelsDammy);

            gameObject.gameObject.SetActive(activeSelf);
        }

        public override void UpdateContent(BaseMenuPrefabParameter baseMenuPrefabParameter)
        {
            base.UpdateContent(baseMenuPrefabParameter);

            if (!IsEnable)
            {
                return;
            }

            // TODO: 使用するデッキindexをセーブデータから取得予定

            currentBeforeDeckIndex = 0;
            tabGroup.SetActiveTab(currentBeforeDeckIndex);

            SetCardModelAsync(currentBeforeDeckIndex).Forget();
        }

        /// <summary>
        /// デッキの切り替えを実行
        /// </summary>
        /// <param name="selectedIndex"></param>
        private void OnChangeActiveTab(int selectedIndex)
        {
            // バトルボックスにカードが設定されているのか確認して文言を出し分けする
            var playerBattleFighterPrefab = PlayerBattleFighterSpawnController.InstanceBaseBattleFighterPrefab as PlayerBattleFighterPrefab;
            var isPut = playerBattleFighterPrefab.BattleFighterBoxView.IsPutBattleCardModel();

            // 現在選択しているデッキの場合はデッキ変更できないので閉じる
            if (currentBeforeDeckIndex == selectedIndex)
            {
                // セーブメニューを開く
                // カード詳細用のサイドアームを閉じる
                if (SideArmSwitcherPrefab.IsOpen)
                {
                    SideArmSwitcherPrefab.PlayCloseAnimation();
                }
            }
            // 選択中のデッキ以外の時
            else
            {
                if (isPut)
                {
                    SideArmSwitcherPrefab.PlayOpenDisplayAnimation(
                        notChangeMessageText,
                        () =>
                        {
                            // NOTE: バトルボックスにカードが設定されている場合は何もしない
                        },
                        () =>
                        {
                            // NOTE: キャンセルの際は何もしない
                        }
                    );
                }
                else
                {
                    SideArmSwitcherPrefab.PlayOpenDisplayAnimation(
                        deckSaveMessageText,
                        () =>
                        {
                            OnDeckChangeAction?.Invoke(selectedIndex);
                        },
                        () =>
                        {
                            // NOTE: キャンセルの際は何もしない
                        }
                    );
                }
            }

            SetCardModelAsync(selectedIndex).Forget();
        }

        /// <summary>
        /// 指定したデッキのカードモデルを基に表示を切り替える
        /// </summary>
        /// <returns></returns>
        private async UniTask SetCardModelAsync(int deckIndex)
        {
            currentDeckIndex = deckIndex;

            var deckCardModels = BaseUseCase.CreateDeckCardModels(deckIndex);
            currentDeckCardModels = deckCardModels.ToList();

            await PreLoadAssetAsync(deckCardModels);

            battleDeckMenuCardScrollController.Setup(deckCardModels);

            if (currentSelectedGameObject != null)
            {
                currentSelectedGameObject.GetComponent<MenuDeckCardCellView>()?.ChangeActiveOfSelectedImage(false);
            }
        }

        /// <summary>
        /// カードアセットをまとめて事前ロード
        /// </summary>
        /// <returns></returns>
        private async UniTask PreLoadAssetAsync(MenuDeckCardModel[] deckCardModels)
        {
            var cardIds = new List<int>();

            foreach (var deckCardModel in deckCardModels)
            {
                cardIds.Add(deckCardModel.CardId);
            }

            cardIds = cardIds.Distinct().ToList();

            foreach (var cardId in cardIds)
            {
                var itemSpriteAddress = string.Format(AddressConstant.BattleCardSpriteAddressStringFormat, cardId);
                if (!AssetCacheManager.Instance.Exist(itemSpriteAddress))
                {
                    await AssetCacheManager.Instance.LoadAssetAsync<Sprite>(itemSpriteAddress);
                }
            }
        }

        private void OnSelectedBattleCard(GameObject selectedGameObject, BattleCardModel selectedBattleCardModel)
        {
            // 選択済みのものがまたは同じものでなければ非選択状態にする
            if (currentSelectedGameObject != null &&
                currentSelectedGameObject != selectedGameObject)
            {
                currentSelectedGameObject.GetComponent<MenuDeckCardCellView>()?.ChangeActiveOfSelectedImage(false);
                currentSelectedGameObject.GetComponent<MenuOwnCardCellView>()?.ChangeActiveOfSelectedImage(false);
            }

            currentSelectedGameObject = selectedGameObject;

            var battleCardModel = BaseUseCase.CreateBattleCardModel(selectedBattleCardModel.CardId);
            battleDeckMenuCardDetialView.ShowCardDetial(battleCardModel);
        }
    }
}
