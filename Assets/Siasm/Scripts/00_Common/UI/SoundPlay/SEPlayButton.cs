using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Siasm
{
    public sealed class SEPlayButton : BaseSEPlay, IPointerEnterHandler
    {
        [SerializeField]
        private Button button;

        [SerializeField]
        private ButtonType decideButtonType = ButtonType.Decide;

        [SerializeField]
        private ButtonType selectButtonType = ButtonType.Selected;

        private void Reset()
        {
            button = this.GetComponent<Button>();
            if (button == null)
            {
                Debug.LogWarning("Buttonコンポーネントが存在しないため参照できませんでした");
            }
        }

        private void Start()
        {
            button.onClick.AddListener(OnClickButton);
        }

        private void OnClickButton()
        {
            if (decideButtonType == ButtonType.None)
            {
                return;
            }

            AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (selectButtonType == ButtonType.None)
            {
                return;
            }

            AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.OnMouseCursor);
        }
    }
}
