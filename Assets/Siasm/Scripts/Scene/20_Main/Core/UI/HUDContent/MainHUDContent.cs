using System.Linq;
using UnityEngine;

namespace Siasm
{
    public class MainHUDContent : MonoBehaviour
    {
        [SerializeField]
        private MainQuestView mainQuestView;

        [SerializeField]
        private OperationTutorialView operationTutorialView;

        // [SerializeField]
        // private HoldEgidoView holdEgidoView;

        public void Initialize(MainUseCase mainUseCase)
        {
            mainQuestView.Initialize();
            operationTutorialView.Initialize();
            // holdEgidoView.Initialize();

            mainQuestView.Disable();
            operationTutorialView.Enable(); // 有効にしたままにする

            // // 解放されていれば表示を行う
            // var saveDataOwnItem = mainUseCase.LoadedSaveDataCache.SaveDataOwnItems.FirstOrDefault(x => x.ItemId == ItemConstant.EgidoId);
            // if (saveDataOwnItem != null)
            // {
            //     holdEgidoView.Enable();
            // }
            // else
            // {
            //     holdEgidoView.Disable();
            // }
        }

        public void Setup(int holdEgidoNumber)
        {
            // holdEgidoView.Apply(holdEgidoNumber);
        }

        /// <summary>
        /// 必要ならアニメーションで表示にする
        /// </summary>
        /// <param name="parameter"></param>
        public void ShowMainQuestView(MainQuestView.Parameter parameter)
        {
            mainQuestView.Enable();
            mainQuestView.Apply(parameter);
        }

        /// <summary>
        /// 必要ならアニメーションで非表示にする
        /// </summary>
        public void HideMainQuestView()
        {
            mainQuestView.Disable();
        }

        /// <summary>
        /// 必要ならアニメーションで表示にする
        /// </summary>
        /// <param name="number"></param>
        public void ShowHoldEgidoView(int number)
        {
            // holdEgidoView.Enable();
            // holdEgidoView.Apply(number);
            // mainQuestView.Apply(parameter);
        }

        /// <summary>
        /// 必要ならアニメーションで表示にする
        /// </summary>
        /// <param name="targetIndex"></param>
        public void ShowOperationTutorialView(int targetIndex)
        {
            operationTutorialView.Show(targetIndex);
        }

        /// <summary>
        /// 必要ならアニメーションで非表示にする
        /// </summary>
        /// <param name="targetIndex"></param>
        public void HideOperationTutorialView(int targetIndex)
        {
            operationTutorialView.Hide(targetIndex);
        }
    }
}
