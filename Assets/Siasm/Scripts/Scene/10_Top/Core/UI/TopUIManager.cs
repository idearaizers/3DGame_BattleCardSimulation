using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Siasm
{
    public class TopUIManager : MonoBehaviour
    {
        private const string logoDisplayState = "ProjectLogo_Display";

        [SerializeField]
        private Animator logoAnimator;

        public void Initialize() { }

        public void Setup() { }

        public async UniTask PlayDisplayLogoAndAnimationAsync(CancellationToken token)
        {
            logoAnimator.Play(logoDisplayState);
            await UniTask.WaitUntil(() => logoAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f, cancellationToken: token);
        }
    }
}
