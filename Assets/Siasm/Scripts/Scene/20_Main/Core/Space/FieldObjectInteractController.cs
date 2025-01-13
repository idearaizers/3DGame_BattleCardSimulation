using System;
using System.Linq;
using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// プレイヤーの進行度に関係なく常に一定で配置するものに対して管理するクラス
    /// </summary>
    public class FieldObjectInteractController : MonoBehaviour
    {
        [SerializeField]
        private FieldObjectOfFieldInteract[] fieldObjectOfFieldInteracts;

        private MainUIManager mainUIManager;
        private MainStateMachineController mainStateMachineController;
        private MainUseCase mainUseCase;
        private MainQuestController mainQuestController;

        public Action OnNextDateAction { get; set; }

        public void Initialize(MainUIManager mainUIManager, MainStateMachineController mainStateMachineController, MainUseCase mainUseCase, MainQuestController mainQuestController)
        {
            this.mainUIManager = mainUIManager;
            this.mainStateMachineController = mainStateMachineController;
            this.mainUseCase = mainUseCase;
            this.mainQuestController = mainQuestController;
        }

        public void Setup()
        {
            foreach (var fieldObjectOfFieldInteract in fieldObjectOfFieldInteracts)
            {
                fieldObjectOfFieldInteract.Initialize();
                fieldObjectOfFieldInteract.OnInteractAction = OnInteract;
            }
        }

        /// <summary>
        /// TODO: インターフェースまたはType毎のクラス処理を作成して拡張しやすい作りに見直し予定
        /// </summary>
        /// <param name="fieldObjectType"></param>
        /// <param name="targetObjectId"></param>
        private void OnInteract(FieldObjectType fieldObjectType, int targetObjectId)
        {
            switch (fieldObjectType)
            {
                case FieldObjectType.OfficeTerminalOfBackHome:
                case FieldObjectType.TrainTerminal:
                    // 家に帰る処理
                    var dialogParameterOfHome = new YesNoMenuDialogPrefab.DialogParameter
                    {
                        TitleText = "今日の仕事を終えて自宅に帰りますか？",
                        OnYesAction = () =>
                        {
                            // クエストを更新
                            mainQuestController.IsProgressOfInteract(102);

                            // 次の日にいく
                            OnNextDateAction?.Invoke();
                        },
                        OnNoAction = () =>
                        {
                            // NOTE: 処理なし
                        }
                    };

                    AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);
                    mainUIManager.MenuArmController.PlayShowDialogMenuAnimation(DialogMenuType.YesNo, dialogParameterOfHome);
                    mainStateMachineController.ChangeMainState(MainStateMachineController.MainState.InteractAction);
                    break;

                case FieldObjectType.OfficeTerminalOfDelivery:

                    var saveDataOwnItem = mainUseCase.LoadedSaveDataCache.SaveDataOwnItems.FirstOrDefault(x => x.ItemId == ItemConstant.EgidoId);
                    var totalEgidoNumberDelivered = mainUseCase.LoadedSaveDataCache.SaveDataMainScene.TotalEgidoNumberDelivered;

                    // TODO: EgidoDeliveryDialogPrefabに処理を移動した方がよさそう
                    var dialogParameter = new EgidoDeliveryMenuDialogPrefab.DialogParameter
                    {
                        TitleText = $"エギドを納品しますか？\n所持エギド{saveDataOwnItem.ItemNumber} 納品エギド{totalEgidoNumberDelivered}",
                        OnYesAction = (number) =>
                        {
                            // TODO: 数が足らない時の処理を追加

                            // エギドを減らす
                            var saveDataOwnItem = mainUseCase.LoadedSaveDataCache.SaveDataOwnItems.FirstOrDefault(x => x.ItemId == ItemConstant.EgidoId);
                            saveDataOwnItem.ItemNumber -= number;

                            // トータル納品エギドに加算する
                            mainUseCase.LoadedSaveDataCache.SaveDataMainScene.TotalEgidoNumberDelivered += number;

                            // クエストを更新
                            mainQuestController.IsProgressOfDelivery(mainUseCase.LoadedSaveDataCache.SaveDataMainScene.TotalEgidoNumberDelivered);

                            // 表示を更新
                            mainQuestController.UpdateMainQuestView();
                        },
                        OnNoAction = () =>
                        {
                            // NOTE: 処理なし
                        }
                    };

                    AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);

                    mainUIManager.MenuArmController.PlayShowDialogMenuAnimation(DialogMenuType.EgidoDelivery, dialogParameter);
                    mainStateMachineController.ChangeMainState(MainStateMachineController.MainState.InteractAction);

                    break;

                case FieldObjectType.OfficeTerminalOfCreatureBox:

                    var admissionDialogParameter = new CreatureAdmissionMenuDialogPrefab.DialogParameter
                    {
                        TitleText = "収容ボックスの選択表示",
                        OnYesAction = () =>
                        {
                            // TODO: 
                        },
                        OnNoAction = () =>
                        {
                            // NOTE: 処理なし
                        }
                    };

                    AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);

                    mainUIManager.MenuArmController.PlayShowDialogMenuAnimation(DialogMenuType.CreatureAdmission, admissionDialogParameter);
                    mainStateMachineController.ChangeMainState(MainStateMachineController.MainState.InteractAction);

                    break;

                case FieldObjectType.NonSecurityDoor:
                    Debug.Log("TODO: ドアの開閉処理");
                    break;

                case FieldObjectType.SecurityDoorOfLevel1:
                    if (mainUseCase.HasItemOfOwn(ItemConstant.SecurityKeyId))
                    {
                        mainQuestController.IsProgressOfInteract(targetObjectId);
                        Debug.Log("TODO: キーがあるのでドアを開く");
                    }
                    else
                    {
                        Debug.Log("TODO: キーがないのでドアが開けない");
                    }
                    break;

                case FieldObjectType.SecurityDoorOfLevel2:
                    Debug.Log("TODO: ドアの開閉処理");
                    break;

                case FieldObjectType.None:
                default:
                    break;
            }
        }
    }
}
