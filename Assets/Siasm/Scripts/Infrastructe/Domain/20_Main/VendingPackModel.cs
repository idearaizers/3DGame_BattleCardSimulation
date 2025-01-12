using UnityEngine.UI;

namespace Siasm
{
    public class VendingPackModel
    {
        public string PackName { get; set; }
        public Image PackImage { get; set; }
        public bool IsPurchased { get; set; }   // 購入済み
    }
}
