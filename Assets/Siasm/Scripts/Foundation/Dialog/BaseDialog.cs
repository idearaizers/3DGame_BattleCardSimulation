using UnityEngine;
using TMPro;
// using DG.Tweening;

namespace Siasm
{
    /// <summary>
    /// Acitonは各継承先で定義する
    /// </summary>
    public abstract class BaseDialog : MonoBehaviour
    {
        public class Parameter
        {
            public string Message { get; set; }
        }

        [SerializeField]
        private TextMeshProUGUI messageText;

        public virtual void Initialize() { }

        public void ApplyParameter(Parameter parameter)
        {
            if (messageText == null)
            {
                return;
            }

            messageText.text = parameter.Message;
        }

        // [SerializeField]
        // private IDialogAnimation dialogAnimation;

        // [SerializeField]
        // private Button closeButton;

        // [SerializeField]
        // private Button dicideButton;

        // private Action closeAction;
        // private Action closeImmediatelyAction;

        // /// <summary>
        // /// 閉じるコールバックを登録
        // /// </summary>
        // /// <param name="closeAction"></param>
        // /// <param name="closeImmediatelyAction"></param>
        // public void SetCloseAction(Action closeAction, Action closeImmediatelyAction = null)
        // {
        //     this.closeAction = closeAction;
        //     this.closeImmediatelyAction = closeImmediatelyAction;

        //     closeButton.onClick.AddListener(() => closeAction?.Invoke());
        // }

        // /// <summary>
        // /// 決定のコールバックを登録
        // /// </summary>
        // /// <param name="decideAction"></param>
        // public void SetDecideAction(Action decideAction)
        // {
        //     dicideButton.onClick.AddListener(() => 
        //     {
        //         decideAction?.Invoke();
        //         closeAction?.Invoke();
        //     });
        // }

        // /// <summary>
        // /// 閉じるコールバックを呼び出す
        // /// </summary>
        // public void InvokeCloseAction(bool immediate = false)
        // {
        //     closeAction?.Invoke();
        // }

        // /// <summary>
        // /// アニメーションして開く
        // /// </summary>
        // /// <returns></returns>
        // public async UniTask PlayOpneAsync()
        // {
        //     await UniTask.CompletedTask;
        // }

        // /// <summary>
        // /// アニメーションして閉じる
        // /// </summary>
        // /// <returns></returns>
        // public async UniTask PlayCoseAsync()
        // {
        //     await UniTask.CompletedTask;
        // }

        // /// <summary>
        // /// 即閉じる
        // /// </summary>
        // public void CloseImmediately()
        // {
        //     closeImmediatelyAction?.Invoke();
        // }




        // bu
        // private const float duration = 0.25f;

        // [SerializeField]
        // private CanvasGroup canvasGroup;

        // public void Open()
        // {
        //     SetActive(true);
        // }

        // public void Close()
        // {
        //     SetActive(false);
        // }

        // public void OpenFade()
        // {
        //     // NOTE: フェードだとライトが強い感じに見えるのでデザインを調整する必要がある
        //     // サイズの変更
        //     // panel.transform.DOScale(new Vector3(0, 0, 0), 0.2f);

        //     // 
        //     // var canvasGroup = Canvas.GetComponent<CanvasGroup>();
        //     canvasGroup.alpha = 0.0f;

        //     this.transform.localScale = new Vector3(0.75f, 0.75f, 1.0f);

        //     // 
        //     var sequence = DOTween.Sequence();
        //     sequence.Append(canvasGroup.DOFade(1.0f, duration).OnStart(Enable));
        //     sequence.Join(this.transform.DOScale(Vector3.one, duration));
        //     sequence.Play();
        // }

        // public void CloseFade()
        // {
        //     // var canvasGroup = Canvas.GetComponent<CanvasGroup>();
        //     canvasGroup.alpha = 1.0f;
        //     // canvasGroup.DOFade(0.0f, duration).OnComplete(Disable);

        //     var sequence = DOTween.Sequence();
        //     sequence.Append(canvasGroup.DOFade(0.0f, duration).OnComplete(Disable));
        //     sequence.Join(this.transform.DOScale(new Vector3(0.75f, 0.75f, 1.0f), duration));
        //     sequence.Play();
        // }
    }
}
