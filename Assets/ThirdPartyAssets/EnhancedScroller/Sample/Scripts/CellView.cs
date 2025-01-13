using UnityEngine.UI;

namespace Enhanced
{
    public class CellView : EnhancedScrollerCellView
    {
        public Text m_nameTextUI;

        public void SetData(Data data)
        {
            m_nameTextUI.text = data.someText;
        }
    }
}
