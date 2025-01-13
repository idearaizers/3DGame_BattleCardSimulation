using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace Siasm
{
    public class OverlayManager : SingletonMonoBehaviour<OverlayManager>
    {
        [SerializeField]
        private Canvas overlayCanvas;

        [SerializeField]
        private TransitionController transitionController;

        [SerializeField]
        private LoadingView loadingView;

        [SerializeField]
        private OverlaySceneDirection overlaySceneDirection;

        public OverlaySceneDirection SceneDirection => overlaySceneDirection;

        private CancellationToken token;

        public void Initialize()
        {
            token = this.GetCancellationTokenOnDestroy();

            transitionController.Initialize();
            loadingView.Initialize();
            overlaySceneDirection.Initialize(token);

            transitionController.Disable();
            loadingView.Disable();
            overlaySceneDirection.Enable();
        }

        public void Setup(Camera uiCamera)
        {
            overlayCanvas.worldCamera = uiCamera;

            transitionController.Setup();
            loadingView.Setup();
            overlaySceneDirection.Setup();
        }

        public void ShowBlackPanel()
        {
            transitionController.Enable();
        }

        public void HideBlackPanel()
        {
            transitionController.Disable();
        }

        /// <summary>
        /// overlaySceneDirectionの階層下に格納する
        /// 格納時に位置を初期化する
        /// </summary>
        /// <param name="gameObject"></param>
        public void SetParentSceneDirection(GameObject gameObject)
        {
            gameObject.transform.SetParent(overlaySceneDirection.transform);

            var rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.localPosition = Vector3.zero;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.localScale = Vector3.one;
            rectTransform.rotation = Quaternion.identity;

            // rectを変更してから初期化する
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
        }

        public async UniTask FadeInAsync(float animationSpeed = 1.0f, Ease ease = Ease.Linear)
        {
            await transitionController.FadeInAsync(animationSpeed, ease);
        }

        public async UniTask FadeOutAsync(float animationSpeed = 1.0f, Ease ease = Ease.Linear)
        {
            await transitionController.FadeOutAsync(animationSpeed, ease);
        }

        /// <summary>
        /// 現状では文字だけ表示のためShowBlackPanelと合わせて使用している
        /// </summary>
        public void ShowLoadingView()
        {
            loadingView.Enable();
        }

        /// <summary>
        /// 現状では文字だけ表示のためHideBlackPanelと合わせて使用している
        /// </summary>
        public void HideLoadingView()
        {
            loadingView.Disable();
        }
    }
}
