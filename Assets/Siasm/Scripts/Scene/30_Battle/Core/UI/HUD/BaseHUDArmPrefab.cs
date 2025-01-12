using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

namespace Siasm
{
    public abstract class BaseHUDArmPrefab : BaseView
    {
        /// <summary>
        /// ボタンの画像の向きの指定
        /// </summary>
        public enum ButtonOrientationType
        {
            None = 0,
            Center,
            Right,
            Left
        }

        protected virtual string ShowStateName => "---_Show";
        protected virtual string OnCursorStateName => "---_OnCursor";

        private const string playSpeedFloatString = "PlaySpeed";

        private readonly Dictionary<ButtonOrientationType, Vector3> buttonOrientationVector3Dictionary = new Dictionary<ButtonOrientationType, Vector3>
        {
            { ButtonOrientationType.Center, new Vector3(90.0f, -90.0f,  0.0f) },
            { ButtonOrientationType.Right,  new Vector3(90.0f,  0.0f,   0.0f) },
            { ButtonOrientationType.Left,   new Vector3(90.0f,  180.0f, 0.0f) }
        };

        [Header("Base関連")]
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private ParentConstraint parentConstraint;

        [Header("Canvas関連")]
        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private Button button;

        [SerializeField]
        private Image buttonImage;

        [SerializeField]
        private HUDArmSwitcherCollider hudArmSwitcherCollider;

        [Header("Color関連")]
        [SerializeField]
        private Color noneSelectedColor = new Color(0.137f, 0.208f, 0.294f, 1.000f);

        [SerializeField]
        private Color onCursorColor = new Color(0.486f, 0.953f, 0.435f, 1.000f);

        /// <summary>
        /// 決定を実行した場合はカーソルを乗せた際のアニメーション処理を実行させたくないのでその際に使用
        /// </summary>
        private bool isCursorAnimation;

        public Action OnClickAction { get; set; }

        public virtual void Initialize(Camera uiCamera, ButtonOrientationType buttonOrientationType)
        {
            // Base関連
            canvas.worldCamera = uiCamera;

            // Canvas関連
            button.onClick.AddListener(OnButtonAction);
            buttonImage.color = noneSelectedColor;
            hudArmSwitcherCollider.OnCursorAction = OnCursorAnimation;
            hudArmSwitcherCollider.OffCursorAction = OffCursorAnimation;

            // ボタンの画像の向きを変更
            parentConstraint.SetRotationOffset(0, buttonOrientationVector3Dictionary[buttonOrientationType]);
        }

        public void Setup() { }

        public void PlayShowAnimation()
        {
            // 初期化してからアニメーションを実行
            buttonImage.color = noneSelectedColor;
            isCursorAnimation = false;

            animator.SetFloat(playSpeedFloatString, 1);
            animator.Play(ShowStateName, 0, 0.0f);
        }

        public void PlayHideAnimation()
        {
            animator.SetFloat(playSpeedFloatString, -1);
            animator.Play(ShowStateName, 0, 1.0f);
        }

        private void OnButtonAction()
        {
            AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);

            isCursorAnimation = true;
            OnClickAction?.Invoke();
        }

        private void OnCursorAnimation()
        {
            if (isCursorAnimation)
            {
                return;
            }

            AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.OnMouseCursor);

            buttonImage.color = onCursorColor;

            animator.SetFloat(playSpeedFloatString, 1);
            animator.Play(OnCursorStateName, 0, 0.0f);
        }

        private void OffCursorAnimation()
        {
            if (isCursorAnimation)
            {
                return;
            }

            buttonImage.color = noneSelectedColor;

            // 逆再生でアニメーションを実行
            animator.SetFloat(playSpeedFloatString, -1);
            animator.Play(OnCursorStateName, 0, 1.0f);
        }
    }
}
