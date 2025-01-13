using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Linq;

namespace Siasm
{
    public class MainTalkController : MonoBehaviour
    {
        private MainUIManager mainUIManager;
        private MainStateMachineController mainStateMachineController;
        private MainUseCase mainUseCase;

        private BaseTalkModel[] currentBaseTalkModels;
        private int currentTalkIndex;
        private Action onTalkFinish;

        public void Initialize(MainUIManager mainUIManager, MainStateMachineController mainStateMachineController, MainUseCase mainUseCase)
        {
            this.mainUIManager = mainUIManager;
            this.mainStateMachineController = mainStateMachineController;
            this.mainUseCase = mainUseCase;
        }

        public void Setup() { }

        public async UniTask PlayTalkAsync(int characterId, Action onTalkFinish = null)
        {
            this.onTalkFinish = onTalkFinish;

            mainStateMachineController.ChangeMainState(MainStateMachineController.MainState.InteractAction);

            // TODO: マジックマンバーになっているので見直し予定
            var detialIndex = 0;
            if (characterId == 201 ||
                characterId == 301)
            {
                // 進行度を確認する
                var isData = mainUseCase.LoadedSaveDataCache.SaveDataLabCharacterTalk.SaveDataLabCharacters.FirstOrDefault(x => x.CharacterId == characterId);

                // データがある場合
                if (isData != null)
                {
                    if (isData.TalkIndex == 0)
                    {
                        // 1にする
                        detialIndex = 1;
                    }
                    else
                    {
                        // TODO: 
                    }
                }
                // データがない場合
                else
                {
                    // 0を再生
                    detialIndex = 0;

                    // 追加
                    var saveDataLabCharacter = new SaveDataLabCharacter
                    {
                        CharacterId = characterId,
                        TalkIndex = 0
                    };
                    mainUseCase.LoadedSaveDataCache.SaveDataLabCharacterTalk.SaveDataLabCharacters.Add(saveDataLabCharacter);
                }
            }

            var baseTalkModels = await mainUseCase.CreateBaseTalkModels(characterId, detialIndex);

            // 会話モデルを基に会話を行う
            currentBaseTalkModels = baseTalkModels;
            currentTalkIndex = 0;

            // 再生を実行
            PlayExecute();
        }

        /// <summary>
        /// 一つずつ取り出して適切な型にキャストしてから実行を行う
        /// </summary>
        private void PlayExecute()
        {
            // マスターデータからトークモデルに変換して使用する
            var baseTalkModel = currentBaseTalkModels[currentTalkIndex];

            // マスターデータのモデルクラスからアクション用のモデルを生成して実行する
            switch (baseTalkModel)
            {
                case TalkMessageModel:
                    var talkMessageModel = baseTalkModel as TalkMessageModel;
                    mainUIManager.MainUIContent.MessageTalkView.OnFinishAction = NextExecute;
                    mainUIManager.MainUIContent.MessageTalkView.PlayMessage(talkMessageModel.TalkName, talkMessageModel.TalkMessage);
                    break;

                case TalkGiftModel:
                    var talkGiftModel = baseTalkModel as TalkGiftModel;
                    mainUIManager.MainUIContent.ItemGiftView.OnFinishAction = NextExecute;
                    mainUIManager.MainUIContent.ItemGiftView.ShowAndGiftItemAsync(talkGiftModel.ItemId).Forget();
                    break;

                default:
                    break;
            }
        }

        private void NextExecute()
        {
            currentTalkIndex++;
            if (currentBaseTalkModels.Length > currentTalkIndex)
            {
                PlayExecute();
                return;
            }

            mainStateMachineController.ChangeMainState(MainStateMachineController.MainState.FreeExploration);

            onTalkFinish?.Invoke();
        }
    }
}
