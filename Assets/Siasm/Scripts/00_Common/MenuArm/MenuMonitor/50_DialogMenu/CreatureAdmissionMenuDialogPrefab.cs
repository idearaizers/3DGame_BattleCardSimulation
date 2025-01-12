using System;
using UnityEngine;
using TMPro;

namespace Siasm
{
    public sealed class CreatureAdmissionMenuDialogPrefab : BaseMenuDialogPrefab
    {
        public class DialogParameter : BaseParameter
        {
            public string TitleText { get; set; }
            public Action OnYesAction { get; set; }
            public Action OnNoAction { get; set; }
            public bool IsOnCloseAction { get; set; } = true;   // 仮でメニューを閉じない
        }

        public class Parameter
        {
            public int CreatureId { get; set; }
        }

        [SerializeField]
        private TextMeshProUGUI titleText;

        [SerializeField]
        private AdmissionView[] admissionViews;

        private DialogParameter currentDialogParameter;
        private Parameter[] currentParameters;

        public override void Initialize(SideArmSwitcherPrefab sideArmSwitcherPrefab, BaseUseCase baseUseCase, BaseCameraController baseCameraController)
        {
            base.Initialize(sideArmSwitcherPrefab, baseUseCase, baseCameraController);

            foreach (var admissionView in admissionViews)
            {
                admissionView.Initialize(baseUseCase);
                admissionView.OnClickAction = OnAdmissionButton;
            }

            // 案
            // コストを使用してルーレットを回して、その中から一体だけ選択できるようにするかな
            // 今日のリストアップという形でもいいかも
            // んで、レベルなどに応じて費用を払って収容する形がいいかも
            // たまにへんなものが混ざっているものがあってもいいかも。レベル+5とかでもいいかも
        }

        public override void Setup()
        {
            base.Setup();

        }

        /// <summary>
        /// 表示
        /// </summary>
        /// <param name="dialogParameter"></param>
        public override void Show(BaseParameter dialogParameter)
        {
            currentDialogParameter = dialogParameter as DialogParameter;

            titleText.text = currentDialogParameter.TitleText;

            // TODO: 開くたびに抽選になるので未抽選の時だけやった方がよさそう
            // TODO: 最大収容している時はレベルアップのものが表示でもよさそう
            // TODO: 機能拡張でできるようにするかな
            // TODO: 最初は NOTE DATA 表示でもいいかも

            // 中身の抽選を行う
            currentParameters = BaseUseCase.GetAdmissionParameters();

            // admissionButtons に中身を反映する
            for (int i = 0; i < admissionViews.Length; i++)
            {
                admissionViews[i].Setup(currentParameters[i]);
            }

            // 表示する
            Enable();
        }

        private void OnAdmissionButton(AdmissionView admissionView)
        {
            var selectedIndex = Array.IndexOf(admissionViews, admissionView);

            // カード詳細用のサイドアームを開く
            SideArmSwitcherPrefab.PlayOpenDisplayAnimation(
                $"{currentParameters[selectedIndex].CreatureId}を収容しますか？",
                () =>
                {
                    // NOTE: エギドを消費して収容するかな

                    // 収容する
                    var currentParameter = currentParameters[selectedIndex];
                    BaseUseCase.CreatureAdmission(currentParameter);

                    // NOTE: 収容時の演出の再生を行う
                },
                () =>
                {
                    // NOTE: キャンセルの際は何もしない
                }
            );
        }
    }
}
