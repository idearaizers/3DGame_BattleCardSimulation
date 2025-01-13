using UnityEngine;

namespace Siasm
{
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
    }
}
