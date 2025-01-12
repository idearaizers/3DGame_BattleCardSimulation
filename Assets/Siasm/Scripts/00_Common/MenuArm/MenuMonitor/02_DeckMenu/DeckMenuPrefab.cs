using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Siasm
{
    public sealed class DeckMenuPrefab : BaseMenuPrefab
    {
        private const string deckSaveMessageText = "変更内容を保存しますか？";

        [SerializeField]
        private TabGroup tabGroup;

        [SerializeField]
        private MenuCardDetialView menuCardDetialView;

        [SerializeField]
        private MenuDeckCardScrollController menuDeckCardScrollController;

        [SerializeField]
        private MenuOwnCardScrollController menuOwnCardScrollController;

        [SerializeField]
        private MenuCardDragController menuCardDragController;

        private GameObject currentSelectedGameObject;
        private List<MenuDeckCardModel> currentDeckCardModels;
        private List<MenuOwnCardModel> currentOwnCardModels;

        private int currentDeckIndex = -1;

        public override void Initialize(SideArmSwitcherPrefab sideArmSwitcherPrefab, BaseUseCase baseUseCase, BaseCameraController baseCameraController,
            PlayerBattleFighterSpawnController playerBattleFighterSpawnController, EnemyBattleFighterSpawnController enemyBattleFighterSpawnController)
        {
            base.Initialize(sideArmSwitcherPrefab, baseUseCase, baseCameraController, playerBattleFighterSpawnController, enemyBattleFighterSpawnController);

            var activeTabIndex = 0;
            tabGroup.SetActiveTab(activeTabIndex);
            tabGroup.OnChangeActiveTab = OnChangeActiveTab;

            menuDeckCardScrollController.Initialize();
            menuDeckCardScrollController.OnClickAction = OnSelectedBattleCard;
            menuDeckCardScrollController.MenuCardScrollRect.OnDraggingAction = OnDragging;
            menuDeckCardScrollController.MenuCardScrollRect.OnEndDragAction = OnEndDrag;

            menuOwnCardScrollController.Initialize();
            menuOwnCardScrollController.OnClickAction = OnSelectedBattleCard;
            menuOwnCardScrollController.MenuCardScrollRect.OnDraggingAction = OnDragging;
            menuOwnCardScrollController.MenuCardScrollRect.OnEndDragAction = OnEndDrag;

            menuCardDragController.Initialize(baseCameraController);
            menuCardDragController.OnDragDeckCard = OnDragDeckCard;
            menuCardDragController.OnDragOwnCard = OnDragOwnCard;

            menuCardDetialView.Initialize();
        }

        public override void Setup(bool isEnable)
        {
            base.Setup(isEnable);

            if (!isEnable)
            {
                return;
            }

            // 非アクティブの時にセットアップを行うと参照エラーが出るのでアクティブに切り替えて実行
            var activeSelf = gameObject.activeSelf;
            gameObject.SetActive(true);
            SetDeckCardModelAndOwnCardModel();
            gameObject.SetActive(activeSelf);

            menuCardDragController.Setup();
            menuCardDetialView.Setup();
        }

        private void SetDeckCardModelAndOwnCardModel()
        {
            if (SaveManager.Instance.LoadedSaveDataCache == null)
            {
                // セーブデータを読み込んでいなければエラー回避用にダミーデータを入れる
                var deckCardModelsDammy = new MenuDeckCardModel[] { };
                menuDeckCardScrollController.Setup(deckCardModelsDammy);

                var ownCardModelsDammy = new MenuOwnCardModel[] { };
                menuOwnCardScrollController.Setup(ownCardModelsDammy);
            }
            else
            {
                SetCardModelAsync(deckIndex: 0).Forget();
            }
        }

        /// <summary>
        /// 指定したデッキのカードモデルを基に表示を切り替える
        /// </summary>
        /// <returns></returns>
        private async UniTask SetCardModelAsync(int deckIndex)
        {
            currentDeckIndex = deckIndex;

            if (SideArmSwitcherPrefab.IsOpen)
            {
                SideArmSwitcherPrefab.PlayCloseAnimation();
            }

            var deckCardModels = BaseUseCase.CreateDeckCardModels(deckIndex);
            var ownCardModels = BaseUseCase.CreateOwnCardModels();

            currentDeckCardModels = deckCardModels.ToList();
            currentOwnCardModels = ownCardModels.ToList();

            List<int> cardIds = new List<int>();

            foreach (var deckCardModel in deckCardModels)
            {
                cardIds.Add(deckCardModel.CardId);
            }

            foreach (var ownCardModel in ownCardModels)
            {
                cardIds.Add(ownCardModel.CardId);
            }

            cardIds = cardIds.Distinct().ToList();

            foreach (var cardId in cardIds)
            {
                // 画像を取得して反映する
                var itemSpriteAddress = string.Format(AddressConstant.BattleCardSpriteAddressStringFormat, cardId);

                // アセットがない場合
                if (!AssetCacheManager.Instance.Exist(itemSpriteAddress))
                {
                    var cachedSprite = await AssetCacheManager.Instance.LoadAssetAsync<Sprite>(itemSpriteAddress);
                }
            }

            menuDeckCardScrollController.Setup(deckCardModels);
            menuOwnCardScrollController.Setup(ownCardModels);
        }

        public override void UpdateContent(BaseMenuPrefabParameter baseMenuPrefabParameter)
        {
            base.UpdateContent(baseMenuPrefabParameter);
        }

        /// <summary>
        /// デッキの切り替えを実行
        /// </summary>
        /// <param name="selectedIndex"></param>
        private void OnChangeActiveTab(int selectedIndex)
        {
            SetCardModelAsync(selectedIndex).Forget();
        }

        /// <summary>
        /// TODO: 引数のBattleCardModelにはカードidしか格納されていないので整理した方がよさそう
        /// </summary>
        /// <param name="selectedGameObject"></param>
        /// <param name="selectedBattleCardModel"></param>
        private void OnSelectedBattleCard(GameObject selectedGameObject, BattleCardModel selectedBattleCardModel)
        {
            // TODO: 下に出ているがこれだとわからないね
            // NOTE: 画面デザインを調整した方がよさそう

            // 選択済みのものがまたは同じものでなければ非選択状態にする
            if (currentSelectedGameObject != null &&
                currentSelectedGameObject != selectedGameObject)
            {
                // 仮
                currentSelectedGameObject.GetComponent<MenuDeckCardCellView>()?.ChangeActiveOfSelectedImage(false);
                currentSelectedGameObject.GetComponent<MenuOwnCardCellView>()?.ChangeActiveOfSelectedImage(false);
            }

            currentSelectedGameObject = selectedGameObject;

            // TODO: 今はカード詳細で使用している
            // TODO: カードidで紐づけをしているのでこれは作りを直した方がよさそう
            // TODO: デッキ用のカードモデルを作ってそのまま中身を表示できるようにした方がいいね
            var battleCardModel = BaseUseCase.CreateBattleCardModel(selectedBattleCardModel.CardId);
            menuCardDetialView.ShowCardDetial(battleCardModel);
        }

        private void OnDragging(MenuCardScrollRect.ScrollType scrollType)
        {
            // 何もない箇所をドラッグした際のエラー回避用に追加
            if (scrollType == MenuCardScrollRect.ScrollType.None)
            {
                return;
            }

            // カードの表示
            if (menuCardDragController.CurrentMenuDragCardPrefab == null)
            {
                menuCardDragController.ShowDraggingCard(scrollType);
            }

            // 移動処理
            if (menuCardDragController.CurrentMenuDragCardPrefab != null)
            {
                menuCardDragController.MovingDragCard();
            }
        }

        private void OnEndDrag()
        {
            // 移動終了処理
            menuCardDragController.MovedDragCard();
        }

        /// <summary>
        /// デッキ側のカードを所持カードエリアにドラッグ移動した時の処理
        /// </summary>
        private void OnDragDeckCard(MenuDeckCardModel menuDeckCardModel)
        {
            if (menuDeckCardModel == null)
            {
                return;
            }

            // セーブメニューを開く
            ShowSaveDeckCardOfAideArm();

            AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);

            // セーブはここではしないので保持している値に変更を行う

            currentDeckCardModels.Remove(menuDeckCardModel);
            menuDeckCardScrollController.Setup(currentDeckCardModels.ToArray());

            // 所持側のカードを増やす
            var currentOwnCardModel = currentOwnCardModels.FirstOrDefault(x => x.CardId == menuDeckCardModel.CardId);
            currentOwnCardModel.OwnNumber++;
            menuOwnCardScrollController.Setup(currentOwnCardModels.ToArray());
        }

        /// <summary>
        /// 所持側のカードをデッキカードエリアにドラッグ移動した時の処理
        /// NOTE: 所持枚数はデッキに設定していない個数にするかな
        /// NOTE: 複数のデッキでカードを設定していた場合はその分だけ減る、使用数は共通にするかな
        /// </summary>
        private void OnDragOwnCard(MenuOwnCardModel menuOwnCardModel)
        {
            if (menuOwnCardModel == null)
            {
                return;
            }

            // 所持側のカードを減らす
            var currentOwnCardModel = currentOwnCardModels.FirstOrDefault(x => x.CardId == menuOwnCardModel.CardId);

            // 0以下の時は処理しない
            if (currentOwnCardModel.OwnNumber <= 0)
            {
                return;
            }

            // セーブメニューを開く
            ShowSaveDeckCardOfAideArm();

            currentOwnCardModel.OwnNumber--;
            menuOwnCardScrollController.Setup(currentOwnCardModels.ToArray());

            AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);

            // デッキ側のカードを増やす
            // 仮
            // インスタンスしないといけないのでBattleCardModelは格納する形がいいかも
            var menuDeckCardModel = new MenuDeckCardModel
            {
                CardId = menuOwnCardModel.CardId
            };

            currentDeckCardModels.Add(menuDeckCardModel);

            // カードの並び順を整える
            currentDeckCardModels.Sort((a, b) => a.CardId - b.CardId);
            menuDeckCardScrollController.Setup(currentDeckCardModels.ToArray());
        }

        /// <summary>
        /// 保存用にサイドアームを開く
        /// </summary>
        private void ShowSaveDeckCardOfAideArm()
        {
            if (!SideArmSwitcherPrefab.IsOpen)
            {
                SideArmSwitcherPrefab.PlayOpenDisplayAnimation(
                    deckSaveMessageText,
                    () =>
                    {
                        BaseUseCase.SaveDeckCard(currentDeckIndex, currentDeckCardModels);
                        BaseUseCase.SaveOwnCard(currentOwnCardModels);
                    },
                    () =>
                    {
                        // NOTE: キャンセルの際は何もしない
                    }
                );
            }
        }
    }
}
