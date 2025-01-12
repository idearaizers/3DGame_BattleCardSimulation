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

        // 
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

        public override void Setup(bool isActive)
        {
            base.Setup(isActive);

            // 使用しない場合は実行しない
            if (!isActive)
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

            // 使用しない場合は実行しない
            if (!IsActive)
            {
                return;
            }

            // TODO: 現在選択中のデッキの中身を表示する
            // TODO: 現在使用しているデッキindexを取得する
            currentBeforeDeckIndex = 0;
            tabGroup.SetActiveTab(currentBeforeDeckIndex);

            // 更新
            // OnChangeActiveTab(currentDeckIndex);
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
            // デッキを変えれるときの表示
            // このターンにあと何回かえれるのか表示したい
            // NOTE: ゲームデザインとして何回でもできるでもいいかも。単純に手間になるのと、あとは簡単なデメリットだけあればいいかな
            // NOTE: 回数よりもTPが減るとかでもいいかも
            else
            {
                // NOTE: 一旦仮で文言と処理を出し分け
                if (isPut)
                {
                    SideArmSwitcherPrefab.PlayOpenDisplayAnimation(
                        notChangeMessageText,
                        () =>
                        {
                            // NOTE: バトルボックスにカードが設定されている場合は何もしない
                            // NOTE: 表示内容を出し分けした方がいいかも。UIの構成を整理すれば出し分けできそう
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

            // // セーブメニューを開く
            // // カード詳細用のサイドアームを閉じる
            // if (SideArmSwitcherPrefab.IsOpen)
            // {
            //     SideArmSwitcherPrefab.PlayCloseAnimation();
            // }

            var deckCardModels = BaseUseCase.CreateDeckCardModels(deckIndex);

            currentDeckCardModels = deckCardModels.ToList();

            // 
            List<int> cardIds = new List<int>();

            // 
            foreach (var deckCardModel in deckCardModels)
            {
                cardIds.Add(deckCardModel.CardId);
            }

            // 
            cardIds = cardIds.Distinct().ToList();

            // 
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

            battleDeckMenuCardScrollController.Setup(deckCardModels);

            // カード選択表示を非アクティブに変えるかな
            // 表示している詳細表示はそのままで
            if (currentSelectedGameObject != null)
            {
                // 仮
                currentSelectedGameObject.GetComponent<MenuDeckCardCellView>()?.ChangeActiveOfSelectedImage(false);
            }
        }

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
            // menuCardDetialView.ShowCardDetial(battleCardModel);

            // 
            var battleCardModel = BaseUseCase.CreateBattleCardModel(selectedBattleCardModel.CardId);
            battleDeckMenuCardDetialView.ShowCardDetial(battleCardModel);
        }





        // private void SetItemModel()
        // {
        //     if (SaveManager.Instance.LoadedSaveDataCache == null)
        //     {
        //         // 非表示になるので単に処理させないだけでもいいかも
        //         statusParameterView.Setup(null);
        //         statusPassiveView.Setup(null);
        //     }
        //     else
        //     {
        //         var battleFighterStatusModel = BaseUseCase.CreateBattleFighterStatusModel();
        //         statusParameterView.Setup(battleFighterStatusModel);

        //         // 仮
        //         var activeSelf = statusPassiveView.gameObject.activeSelf;
        //         statusPassiveView.gameObject.SetActive(true);

        //         var battleFighterPassiveModel = BaseUseCase.CreateBattleFighterPassiveModel();
        //         statusPassiveView.Setup(battleFighterPassiveModel);

        //         statusPassiveView.gameObject.SetActive(activeSelf);
        //     }
        // }
    }
}
