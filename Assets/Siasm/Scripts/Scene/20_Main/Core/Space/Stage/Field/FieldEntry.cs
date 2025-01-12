using System;
using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// 必要なら継承して使用
    /// </summary>
    public class FieldEntry : MonoBehaviour
    {
        public Action OnEntryAction { get; set; }

        public virtual void Initialize() { }

        /// <summary>
        /// 指定のエリアに侵入した際の処理
        /// </summary>
        public virtual void Entry()
        {
            OnEntryAction?.Invoke();
        }
    }
}
