using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using TMPro;

namespace Siasm
{
    public class SideArmSwitcherPrefab : MonoBehaviour
    {
        private const string speedFloatText = "PlaySpeed";
        private const string onShowStateName = "SideArmSwitcher_Show";

        private readonly Vector3 rightRotationOffset = new Vector3(90.0f, 180.0f, 0.0f);

        [Header("Base関連")]
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private ParentConstraint parentConstraint;

        [Header("Content関連")]
        [SerializeField]
        private TextMeshProUGUI detialText;

        /// <summary>
        /// NOTE: ボタンの文言を変更したい場合は専用の処理を追加する
        /// </summary>
        [SerializeField]
        private Button yesButton;

        /// <summary>
        /// NOTE: ボタンの文言を変更したい場合は専用の処理を追加する
        /// </summary>
        [SerializeField]
        private Button noButton;

        private Action currentOnYesAction;
        private Action currentOnNoAction;

        public bool IsOpen { get; private set; }

        public void Initialize(Camera uiCamera)
        {
            // Base関連
            canvas.worldCamera = uiCamera;

            // Content関連
            yesButton.onClick.AddListener(OnYesButton);
            noButton.onClick.AddListener(OnNoButton);

            // 表示する場所に合わせて向きを調整
            parentConstraint.SetRotationOffset(0, rightRotationOffset);

            // 最初は閉じている状態
            IsOpen = false;
        }

        public void Setup() { }

        public void PlayOpenDisplayAnimation(string detialText, Action onYesAction, Action onNoAction)
        {
            this.detialText.text = detialText;
            currentOnYesAction = onYesAction;
            currentOnNoAction = onNoAction;

            PlayOpenAnimation();
        }

        private void OnYesButton()
        {
            currentOnYesAction?.Invoke();
            PlayCloseAnimation();
        }

        private void OnNoButton()
        {
            currentOnNoAction?.Invoke();
            PlayCloseAnimation();
        }

        private void PlayOpenAnimation()
        {
            // 開ききっていないが、開いている状態にする
            IsOpen = true;

            animator.SetFloat(speedFloatText, 1);
            animator.Play(onShowStateName, 0, 0.0f);
        }

        /// <summary>
        /// Openの逆再生でアニメーションを実行
        /// </summary>
        public void PlayCloseAnimation()
        {
            // 閉じきっていないが、閉じている状態にする
            IsOpen = false;

            animator.SetFloat(speedFloatText, -1);
            animator.Play(onShowStateName, 0, 1.0f);
        }
    }
}
