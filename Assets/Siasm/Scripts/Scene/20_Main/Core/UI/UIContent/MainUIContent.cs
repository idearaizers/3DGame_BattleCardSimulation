using UnityEngine;

namespace Siasm
{
    public class MainUIContent : MonoBehaviour
    {
        [SerializeField]
        private MessageTalkView messageTalkView;

        [SerializeField]
        private YesNoSelectView yesNoSelectView;

        [SerializeField]
        private ItemGiftView itemGiftView;

        public MessageTalkView MessageTalkView => messageTalkView;
        public YesNoSelectView YesNoSelectView => yesNoSelectView;
        public ItemGiftView ItemGiftView => itemGiftView;

        public void Initialize(MainUseCase mainUseCase)
        {
            messageTalkView.Initialize();
            messageTalkView.Disable();

            yesNoSelectView.Initialize();
            yesNoSelectView.Disable();

            itemGiftView.Initialize(mainUseCase);
            itemGiftView.Disable();
        }

        public void Setup() { }
    }
}
