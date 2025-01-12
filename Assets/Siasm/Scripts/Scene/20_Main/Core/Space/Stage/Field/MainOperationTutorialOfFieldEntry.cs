using System;
using UnityEngine;

namespace Siasm
{
    public class MainOperationTutorialOfFieldEntry : FieldEntry
    {
        [SerializeField]
        private int tutorialId;

        public Action<int> OnEntryIntAction { get; set; }

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

        /// <summary>
        /// 指定のエリアに侵入した際の処理
        /// </summary>
        public override void Entry()
        {
            OnEntryIntAction?.Invoke(tutorialId);
        }
    }
}
