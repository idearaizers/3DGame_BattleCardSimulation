using UnityEngine;

namespace Siasm
{
    public abstract class BaseBattleCardOperationController : MonoBehaviour
    {
        public virtual void DrawHandCard()
        {
            // 継承先で実装
        }

        /// <summary>
        /// 墓地のカードをデッキにランダムで戻す
        /// </summary>
        public virtual void DeckReload()
        {
            // 継承先で実装しているが共通化してもいいかも
        }
    }
}
