using System;
using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// スペースではなくロジックの場所に置いた方がいいかも
    /// </summary>
    public class MainQuestController : BaseQuestController
    {

        private MainUIManager mainUIManager;
        private MainUseCase mainUseCase;

        private BaseMainQuestModel currentBaseMainQuestModel;

        public void Initialize(MainUIManager mainUIManager, MainUseCase mainUseCase)
        {
            this.mainUIManager = mainUIManager;
            this.mainUseCase = mainUseCase;
        }

        /// <summary>
        /// モデルクラスの生成はこのコントローラーで行う
        /// </summary>
        public void Setup()
        {
            StartMainQuestOfProgress();
        }

        private void StartMainQuestOfProgress()
        {
            currentBaseMainQuestModel = mainUseCase.CreateBaseMainQuestModel();

            // 中身が空であれば終了する
            if (currentBaseMainQuestModel == null)
            {
                return;
            }

            var parameter = CreateMainQuestViewParameter();
            if (parameter != null)
            {
                // クエスト情報を表示する
                mainUIManager.ChangeActiveOfMainQuestView(true, parameter);

                // 操作情報を表示する
                foreach (var operationTutorialIndex in currentBaseMainQuestModel.OperationTutorialIndexes)
                {
                    mainUIManager.ChangeActiveOfOperationTutorialView(true, operationTutorialIndex);
                }
            }
        }

        public void UpdateMainQuestView()
        {
            var parameter = CreateMainQuestViewParameter();
            if (parameter != null)
            {
                // クエスト情報を表示する
                mainUIManager.ChangeActiveOfMainQuestView(true, parameter);

                // 操作情報を表示する
                foreach (var operationTutorialIndex in currentBaseMainQuestModel.OperationTutorialIndexes)
                {
                    mainUIManager.ChangeActiveOfOperationTutorialView(true, operationTutorialIndex);
                }
            }
        }

        /// <summary>
        /// クエスト情報を表示するための値を生成する
        /// </summary>
        /// <returns></returns>
        private MainQuestView.Parameter CreateMainQuestViewParameter()
        {
            // TODO: トータルの納品値から残りの値を算出する

            var totalEgidoNumberDelivered = SaveManager.Instance.LoadedSaveDataCache.SaveDataMainScene.TotalEgidoNumberDelivered;
            var detialText = $"{currentBaseMainQuestModel.DetialText}({totalEgidoNumberDelivered})";

            return new MainQuestView.Parameter
            {
                TitleText = currentBaseMainQuestModel.TitleText,
                DetialText = detialText
            };
        }

        /// <summary>
        /// 会話関係のクエストを進行中か確認してクリア状態にする
        /// </summary>
        public void IsProgressOfTalk(int targetCharacterId)
        {
            // 進行中のものがなければ終了
            if (currentBaseMainQuestModel == null)
            {
                return;
            }

            switch (currentBaseMainQuestModel)
            {
                // 処理しない
                case InteractMainQuestModel:
                case PickUpMainQuestModel:
                case DeliveryEgidoMainQuestModel:
                    break;

                case TalkMainQuestModel:
                    var talkMainQuestModel = currentBaseMainQuestModel as TalkMainQuestModel;
                    if (talkMainQuestModel.TargetCharacterId == targetCharacterId)
                    {
                        // 達成済みに変更する
                        mainUseCase.SetIsClearOfMainQuest(currentBaseMainQuestModel);

                        // 次のクエストを開始する
                        StartNextQuest();
                    }
                    break;

                default:
                    throw new ArgumentException(nameof(currentBaseMainQuestModel));
            }
        }

        /// <summary>
        /// 会話関係のクエストを進行中か確認してクリア状態にする
        /// </summary>
        public void IsProgressOfInteract(int targetObjectId)
        {
            // 進行中のものがなければ終了
            if (currentBaseMainQuestModel == null)
            {
                return;
            }

            switch (currentBaseMainQuestModel)
            {
                // 処理しない
                case TalkMainQuestModel:
                    break;

                case InteractMainQuestModel:
                    var interactMainQuestModel = currentBaseMainQuestModel as InteractMainQuestModel;
                    if (interactMainQuestModel.TargetObjectId == targetObjectId)
                    {
                        // 達成済みに変更する
                        mainUseCase.SetIsClearOfMainQuest(currentBaseMainQuestModel);

                        // 次のクエストを開始する
                        StartNextQuest();
                    }
                    break;

                default:
                    // throw new ArgumentException(nameof(currentBaseMainQuestModel));
                    break;
            }
        }


        /// <summary>
        /// 指定のアイテムを拾った
        /// </summary>
        public void IsProgressOfPickUp(int itemId)
        {
            // 進行中のものがなければ終了
            if (currentBaseMainQuestModel == null)
            {
                return;
            }

            switch (currentBaseMainQuestModel)
            {
                // 処理しない
                case TalkMainQuestModel:
                case InteractMainQuestModel:
                case DeliveryEgidoMainQuestModel:
                    break;

                case PickUpMainQuestModel:
                    var pickUpMainQuestModel = currentBaseMainQuestModel as PickUpMainQuestModel;
                    if (pickUpMainQuestModel.TargetObjectId == itemId)
                    {
                        // 達成済みに変更する
                        mainUseCase.SetIsClearOfMainQuest(currentBaseMainQuestModel);

                        // 次のクエストを開始する
                        StartNextQuest();

                        // // 仮
                        // // 指定のアイテムであればエギド機能を解放したので表示を行う
                        // // リポジトリクラスでフラグは変更している
                        // if (itemId == 1004)
                        // {
                        //     mainUIManager.UpdateViewOfEgido();
                        // }
                    }
                    break;

                default:
                    throw new ArgumentException(nameof(currentBaseMainQuestModel));
            }
        }




        /// <summary>
        /// 納品
        /// </summary>
        public void IsProgressOfDelivery(int totalDeliveryEgidoNumber)
        {
            // 進行中のものがなければ終了
            if (currentBaseMainQuestModel == null)
            {
                return;
            }

            switch (currentBaseMainQuestModel)
            {
                // 処理しない
                case TalkMainQuestModel:
                case InteractMainQuestModel:
                case PickUpMainQuestModel:
                    break;

                case DeliveryEgidoMainQuestModel:
                    var deliveryEgidoMainQuestModel = currentBaseMainQuestModel as DeliveryEgidoMainQuestModel;
                    if (deliveryEgidoMainQuestModel.EgidoNumber <= totalDeliveryEgidoNumber)
                    {
                        // 達成済みに変更する
                        mainUseCase.SetIsClearOfMainQuest(currentBaseMainQuestModel);

                        // 次のクエストを開始する
                        StartNextQuest();
                    }
                    break;

                default:
                    throw new ArgumentException(nameof(currentBaseMainQuestModel));
            }
        }




        /// <summary>
        /// 次のクエストを開始する
        /// </summary>
        private void StartNextQuest()
        {
            // UIの表示を非表示にする
            mainUIManager.ChangeActiveOfMainQuestView(false);

            // 操作情報を非表示にする
            foreach (var operationTutorialIndex in currentBaseMainQuestModel.OperationTutorialIndexes)
            {
                mainUIManager.ChangeActiveOfOperationTutorialView(false, operationTutorialIndex);
            }

            // 次のクエストを進行状態にする
            mainUseCase.SetProgressOfMainQuest(currentBaseMainQuestModel.NextId);

            // 進行中のクエストを完了したいのでnullにする
            currentBaseMainQuestModel = null;

            // 次のクエストの実行を行う
            StartMainQuestOfProgress();
        }

    }
}
