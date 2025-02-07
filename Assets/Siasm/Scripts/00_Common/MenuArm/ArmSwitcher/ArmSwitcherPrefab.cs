using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using TMPro;

namespace Siasm
{
    public class ArmSwitcherPrefab : MonoBehaviour
    {
        private const string speedFloatText = "PlaySpeed";
        private const string onCursorStateName = "ArmSwitcher_OnCursor";

        private readonly Vector3 rightRotationOffset = new Vector3(90.0f, 0.0f, 0.0f);
        private readonly Vector3 leftRotationOffset = new Vector3(90.0f, 180.0f, 0.0f);

        [Header("Base関連")]
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private ParentConstraint parentConstraint;

        [Header("Content関連")]
        [SerializeField]
        private Button button;

        [SerializeField]
        private TextMeshProUGUI buttonText;

        [SerializeField]
        private ArmSwitcherCollider armSwitcherCollider;

        [Header("ButtonColor関連")]
        [SerializeField]
        private Image buttonImage;

        [SerializeField]
        private Color noneSelectedColor = new Color(0.137f, 0.208f, 0.294f, 1.000f);

        [SerializeField]
        private Color onCursorColor = new Color(0.484f, 0.953f, 0.436f, 1.000f);

        [SerializeField]
        private Color selectedColor = new Color(0.102f, 0.671f, 0.043f, 1.000f);

        /// <summary>
        /// カーソルOnの際に変更前のカラーを保持する
        /// </summary>
        private Color beforeCursorOnColor;

        /// <summary>
        /// カーソルOffの際に変更前のカラーに戻す際に使用
        /// </summary>
        private bool isSelected;

        public Action<ArmSwitcherPrefab> OnClickAction { get; set; }

        public void Initialize(Camera uiCamera, bool isLeftSide)
        {
            // Base関連
            canvas.worldCamera = uiCamera;

            // Content関連
            button.onClick.AddListener(OnButtonAction);

            armSwitcherCollider.OnCursorAction = OnCursor;
            armSwitcherCollider.OffCursorAction = OffCursor;

            // 初期化時はNoneの状態にする
            SetColorOfButtonImage(false);

            if (isLeftSide)
            {
                parentConstraint.SetRotationOffset(0, rightRotationOffset);
            }
            else
            {
                parentConstraint.SetRotationOffset(0, leftRotationOffset);
            }
        }

        public void Setup(string buttonText, bool isActive)
        {
            this.buttonText.text = buttonText;

            if (!isActive)
            {
                gameObject.SetActive(false);
            }
        }

        public void SetColorOfButtonImage(bool isSelected)
        {
            this.isSelected = isSelected;
            if (isSelected)
            {
                buttonImage.color = selectedColor;
            }
            else
            {
                buttonImage.color = noneSelectedColor;
            }
        }

        private void OnButtonAction()
        {
            AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);

            OnClickAction?.Invoke(this);
        }

        private void OnCursor()
        {
            AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.OnMouseCursor);

            beforeCursorOnColor = buttonImage.color;
            buttonImage.color = onCursorColor;

            animator.SetFloat(speedFloatText, 1);
            animator.Play(onCursorStateName, 0, 0.0f);
        }

        /// <summary>
        /// OnCursorの逆再生でアニメーションを実行
        /// </summary>
        private void OffCursor()
        {
            if (isSelected)
            {
                buttonImage.color = selectedColor;
            }
            else
            {
                buttonImage.color = beforeCursorOnColor;
            }

            animator.SetFloat(speedFloatText, -1);
            animator.Play(onCursorStateName, 0, 1.0f);
        }
    }
}
