using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Siasm
{
    public sealed class TitleScenePresenter : BaseScenePresenter
    {
        private readonly TitleCore titleCore;
        private readonly TitleUseCase titleUseCase;

        [Inject]
        public TitleScenePresenter(TitleCore titleCore, TitleUseCase titleUseCase)
        {
            this.titleCore = titleCore;
            this.titleUseCase = titleUseCase;
        }

        public override async UniTask StartAsync(CancellationToken token)
        {
            await base.StartAsync(token);

            Initialize();
            Setup(titleCore.TitleSpaceManager.TitleCameraController.UICamera);

            // BGMの変更
            AudioManager.Instance.PlayBGMOfLocal(BaseAudioPlayer.PlayType.Single, AudioBGMType.TopScene);

            // フェード処理
            OverlayManager.Instance.ShowBlackPanel();
            OverlayManager.Instance.FadeOutAsync().Forget();

            // 開始ボタンを表示
            titleCore.TitleUIManager.TitleUIContent.ShowTitleStart();
        }

        private void Initialize()
        {
            titleCore.Initialize(titleUseCase);
        }

        protected override void Setup(Camera uiCamera)
        {
            base.Setup(uiCamera);

            titleCore.Setup();
        }
    }
}
