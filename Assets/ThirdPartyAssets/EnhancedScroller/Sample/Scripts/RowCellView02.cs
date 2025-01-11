using UnityEngine;
using UnityEngine.UI;

namespace Enhanced
{
    public class RowCellView02 : MonoBehaviour
    {
        public GameObject container;
        public Text text;

        public void SetData(Data data)
        {
            container.SetActive(data != null);

            if (data != null)
            {
                text.text = data.someText;
            }
        }
    }
}
