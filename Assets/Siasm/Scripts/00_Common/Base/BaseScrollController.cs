using UnityEngine;
using UnityEngine.UI;
using Enhanced;

namespace Siasm
{
    public abstract class BaseScrollController : BaseView
    {
        private const string cellNameStringFormat = "Cell {0} to {1}";

        [SerializeField]
        private EnhancedScroller enhancedScroller;

        [SerializeField]
        private EnhancedScrollerCellView enhancedScrollerCellViewPrefab;

        [SerializeField]
        private int numberOfCellsPerRow;

        /// <summary>
        /// セルの高さ
        /// </summary>
        [SerializeField]
        private float cellSize = 272.0f;

        [SerializeField]
        private ScrollRect scrollRect;

        protected EnhancedScroller EnhancedScroller => enhancedScroller;
        protected EnhancedScrollerCellView EnhancedScrollerCellViewPrefab => enhancedScrollerCellViewPrefab;
        protected int NumberOfCellsPerRow => numberOfCellsPerRow;
        protected ScrollRect ScrollRect => scrollRect;

        public virtual void Initialize() { }

        /// <summary>
        /// NOTE: 表示状態で使用しないとエラーになるため注意
        /// TODO: 基本的には表示する際に実行した方がよさそうだが必要なSetUpの頻度と合わせて見直し予定
        /// </summary>
        public void Setup()
        {
            // 更新前のポジションを保持して反映時にその位置にジャンプする
            var scrollposition = enhancedScroller.NormalizedScrollPosition;
            enhancedScroller.ReloadData(scrollposition);
        }

        public virtual float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return cellSize;
        }

        /// <summary>
        /// Cell 0 to 3 などで表示を行う
        /// </summary>
        /// <param name="dataIndex"></param>
        /// <returns></returns>
        public string GetCellNameText(int dataIndex)
        {
            var startNumber = dataIndex * NumberOfCellsPerRow;
            var endNumber = (dataIndex * NumberOfCellsPerRow) + NumberOfCellsPerRow - 1;
            return string.Format(cellNameStringFormat, startNumber, endNumber);
        }
    }
}
