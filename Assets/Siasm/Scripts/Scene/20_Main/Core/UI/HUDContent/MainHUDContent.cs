using UnityEngine;

namespace Siasm
{
    public class MainHUDContent : MonoBehaviour
    {
        [SerializeField]
        private MainQuestView mainQuestView;

        [SerializeField]
        private OperationTutorialView operationTutorialView;

        public void Initialize(MainUseCase mainUseCase)
        {
            mainQuestView.Initialize();
            operationTutorialView.Initialize();

            mainQuestView.Disable();
            operationTutorialView.Enable();
        }

        public void Setup(int holdEgidoNumber)
        {
            // TODO: 
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
            // TODO: 
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
