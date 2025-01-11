using System;
using UnityEngine;

namespace Enhanced
{
    public abstract class EnhancedScrollerCellView : MonoBehaviour
    {
        /// <summary>
        /// ここをプロパティにすると動作しないので注意
        /// CellViewが入ってくるみたいで継承基のクラス名が入ってくるのかも
        /// なんかnullのままになっているけどそのままでも動くみたい
        /// GridCellRowでもいいかも
        /// </summary>
        public string CellIdentifier;

        [NonSerialized]
        public int CellIndex;

        [NonSerialized]
        public int DataIndex;

        [NonSerialized]
        public bool Active;

        public virtual void RefreshCellView() { }
    }
}
