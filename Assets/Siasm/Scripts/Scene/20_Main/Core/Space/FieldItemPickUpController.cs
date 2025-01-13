using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Siasm
{
    public class FieldItemPickUpController : MonoBehaviour
    {
        [SerializeField]
        private FieldItemPickUpOfFieldInteract[] fieldItemPickUpOfFieldInteracts;

        private MainUIManager mainUIManager;
        private MainStateMachineController mainStateMachineController;
        private MainUseCase mainUseCase;
        private MainQuestController mainQuestController;

        public void Initialize(MainUIManager mainUIManager, MainStateMachineController mainStateMachineController, MainUseCase mainUseCase, MainQuestController mainQuestController)
        {
            this.mainUIManager = mainUIManager;
            this.mainStateMachineController = mainStateMachineController;
            this.mainUseCase = mainUseCase;
            this.mainQuestController = mainQuestController;
        }

        public void Setup()
        {
            foreach (var fieldItemPickUpOfFieldInteract in fieldItemPickUpOfFieldInteracts)
            {
                fieldItemPickUpOfFieldInteract.Initialize();
                fieldItemPickUpOfFieldInteract.OnPickUpAction = OnPickUp;
            }

            // 獲得済みであれば非表示にする
            var pickedIndexs = mainUseCase.LoadedSaveDataCache.SaveDataPickUp.PickedIndexs;
            foreach (var pickedIndex in pickedIndexs)
            {
                fieldItemPickUpOfFieldInteracts[pickedIndex].gameObject.SetActive(false);
            }
        }

        private void OnPickUp(FieldItemPickUpOfFieldInteract fieldItemPickUpOfFieldInteract, int itemId, int itemNumber, PlayerFieldCharacter playerFieldCharacter)
        {
            PickUpItem(fieldItemPickUpOfFieldInteract, itemId, itemNumber, playerFieldCharacter);
        }

        public void PickUpItem(FieldItemPickUpOfFieldInteract fieldItemPickUpOfFieldInteract, int itemId, int itemNumber, PlayerFieldCharacter playerFieldCharacter, bool isHide = false)
        {
            AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);

            mainStateMachineController.ChangeMainState(MainStateMachineController.MainState.InteractAction);

            if (!isHide)
            {
                // Objectを非表示にする
                var targetIndex = Array.IndexOf(fieldItemPickUpOfFieldInteracts, fieldItemPickUpOfFieldInteract);
                fieldItemPickUpOfFieldInteracts[targetIndex].gameObject.SetActive(false);

                // 拾った情報を保持する
                mainUseCase.AddPickedIndex(targetIndex);
            }

            // 調べる表示を消すかな
            playerFieldCharacter.PlayerFieldContact.RemoveCurrentFieldInteract();

            // 拾ったアイテムを表示する
            mainUIManager.MainUIContent.ItemGiftView.ShowAndGiftItemAsync(itemId, itemNumber).Forget();
            mainUIManager.MainUIContent.ItemGiftView.OnFinishAction = () =>
            {
                mainStateMachineController.ChangeMainState(MainStateMachineController.MainState.FreeExploration);

                // 拾ったアイテムがエギドであればエギドの個数を表示する
                if (itemId == ItemConstant.EgidoId)
                {
                    mainUIManager.UpdateViewOfEgido();
                }
            };

            // クエストを更新する
            mainQuestController.IsProgressOfPickUp(itemId);
        }
    }
}
