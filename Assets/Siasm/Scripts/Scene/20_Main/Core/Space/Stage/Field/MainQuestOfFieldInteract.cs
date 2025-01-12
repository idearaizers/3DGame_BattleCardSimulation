using System;
using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// 調べる用のやつ
    /// </summary>
    public sealed class MainQuestOfFieldInteract : FieldInteract
    {
        [SerializeField]
        private int tutorialId;

        public int TutorialId => tutorialId;

        /// <summary>
        /// 開発用の備考テキスト
        /// </summary>
        [SerializeField]
        private string developmentDescription;

        public void ChangeActiveGameObject(int tutorialId)
        {
            if (this.tutorialId == tutorialId)
            {
                this.gameObject.SetActive(true);
            }
        }

        // /// <summary>
        // /// 指定のエリアに侵入した際の処理
        // /// </summary>
        // public override void Entry(int number)
        // {
        //     OnEntryAction?.Invoke(tutorialId);
        // }
    }
}
