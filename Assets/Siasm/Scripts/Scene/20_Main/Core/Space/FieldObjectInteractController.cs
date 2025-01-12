using System;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

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

        /// <summary>
        /// モデルクラスは使用せずにセットアップを行っているが、
        /// 大量にある場合はモデルクラスから作成する形に変える
        /// </summary>
        public void Setup()
        {
            foreach (var fieldObjectOfFieldInteract in fieldObjectOfFieldInteracts)
            {
                fieldObjectOfFieldInteract.Initialize();
                fieldObjectOfFieldInteract.OnInteractAction = OnInteract;
            }
        }

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
                            // 仮でインタラクトidを設定する
                            // クエストを更新
                            mainQuestController.IsProgressOfInteract(102);

                            // 次の日にいく
                            OnNextDateAction?.Invoke();

                            // TODO: 次の日に進むと同じステートに変更しようとするみたいで処理を見直した方がいいかも
                        },
                        OnNoAction = () =>
                        {
                            // クローズし終わった後の処理にしないといけないな
                            // mainStateMachineController.ChangeMainState(MainStateMachineController.MainState.FreeExploration);
                        },
                        // IsOnCloseAction = false
                    };

                    // 仮SE
                    AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);
                    mainUIManager.MenuArmController.PlayShowDialogMenuAnimation(DialogMenuType.YesNo, dialogParameterOfHome);
                    mainStateMachineController.ChangeMainState(MainStateMachineController.MainState.InteractAction);
                    break;

                case FieldObjectType.OfficeTerminalOfDelivery:

                    // 仮
                    var saveDataOwnItem = mainUseCase.LoadedSaveDataCache.SaveDataOwnItems.FirstOrDefault(x => x.ItemId == ItemConstant.EgidoId);
                    var totalEgidoNumberDelivered = mainUseCase.LoadedSaveDataCache.SaveDataMainScene.TotalEgidoNumberDelivered;

                    // TODO: EgidoDeliveryDialogPrefab に処理を移動した方がよさそう
                    var dialogParameter = new EgidoDeliveryMenuDialogPrefab.DialogParameter
                    {
                        TitleText = $"エギドを納品しますか？\n所持エギド{saveDataOwnItem.ItemNumber} 納品エギド{totalEgidoNumberDelivered}",
                        OnYesAction = (number) =>
                        {
                            // NOTE: 数が足らない時の処理を追加

                            // エギドを減らす
                            // mainUseCase.LoadedSaveDataCache.SaveDataProgress.HoldEgidoNumber -= 500;
                            var saveDataOwnItem = mainUseCase.LoadedSaveDataCache.SaveDataOwnItems.FirstOrDefault(x => x.ItemId == ItemConstant.EgidoId);
                            saveDataOwnItem.ItemNumber -= number;

                            // 表示を更新
                            // mainUIManager.UpdateViewOfEgido();

                            // トータル納品エギドに加算する
                            mainUseCase.LoadedSaveDataCache.SaveDataMainScene.TotalEgidoNumberDelivered += number;

                            // クエストを更新
                            mainQuestController.IsProgressOfDelivery(mainUseCase.LoadedSaveDataCache.SaveDataMainScene.TotalEgidoNumberDelivered);

                            // 表示を更新
                            mainQuestController.UpdateMainQuestView();

                            // フリーに変更
                            // mainStateMachineController.ChangeMainState(MainStateMachineController.MainState.FreeExploration);

                        },
                        OnNoAction = () =>
                        {
                            // クローズし終わった後の処理にしないといけないな
                            // mainStateMachineController.ChangeMainState(MainStateMachineController.MainState.FreeExploration);
                        },
                    };

                    // 仮SE
                    AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);

                    mainUIManager.MenuArmController.PlayShowDialogMenuAnimation(DialogMenuType.EgidoDelivery, dialogParameter);
                    mainStateMachineController.ChangeMainState(MainStateMachineController.MainState.InteractAction);

                    break;
                
                case FieldObjectType.OfficeTerminalOfCreatureBox:

                    // 仮
                    // TODO: エギド納品用のものでの表示がいいかも
                    var aaa = new CreatureAdmissionMenuDialogPrefab.DialogParameter
                    {
                        TitleText = "収容ボックスの選択表示",
                        OnYesAction = () =>
                        {
                            // NOTE: 数が足らない時の処理を追加

                            // エギドを減らす
                            // mainUseCase.LoadedSaveDataCache.SaveDataProgress.HoldEgidoNumber -= 500;
                            // var saveDataOwnItem = mainUseCase.LoadedSaveDataCache.SaveDataOwnItems.FirstOrDefault(x => x.ItemId == ItemConstant.EgidoId);
                            // saveDataOwnItem.ItemNumber -= 500;

                            // // 表示を更新
                            // mainUIManager.UpdateViewOfEgido();

                            // // トータル納品エギドに加算する
                            // mainUseCase.LoadedSaveDataCache.SaveDataMainScene.TotalEgidoNumberDelivered += 500;

                            // // クエストを更新
                            // mainQuestController.IsProgressOfDelivery(mainUseCase.LoadedSaveDataCache.SaveDataMainScene.TotalEgidoNumberDelivered);

                            // フリーに変更
                            // mainStateMachineController.ChangeMainState(MainStateMachineController.MainState.FreeExploration);

                        },
                        OnNoAction = () =>
                        {
                            // クローズし終わった後の処理にしないといけないな
                            // mainStateMachineController.ChangeMainState(MainStateMachineController.MainState.FreeExploration);
                        },
                    };

                    // 仮SE
                    AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);

                    mainUIManager.MenuArmController.PlayShowDialogMenuAnimation(DialogMenuType.CreatureAdmission, aaa);
                    mainStateMachineController.ChangeMainState(MainStateMachineController.MainState.InteractAction);

                    break;

                case FieldObjectType.NonSecurityDoor:
                    Debug.Log("TODO: ドアの開閉処理");
                    break;

                case FieldObjectType.SecurityDoorOfLevel1:
                    if (mainUseCase.HasItemOfOwn(ItemConstant.SecurityKeyId))
                    {
                        // 
                        mainQuestController.IsProgressOfInteract(targetObjectId);

                        // 
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
