using System.Collections.Generic;
using UnityEngine;

namespace Enhanced
{
    public class ScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
    {
        public EnhancedScroller m_scroller;
        public CellView m_cellPregab;

        private List<Data> m_list;

        private void Start()
        {
            m_list = new List<Data>();

            for (int i = 0; i < 100; i++)
            {
                var scrollerData = new Data { someText = i.ToString() };
                m_list.Add(scrollerData);
            }

            m_scroller.Delegate = this;
            m_scroller.ReloadData();
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return m_list.Count;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return 60.0f;
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var cellView = scroller.GetCellView(m_cellPregab) as CellView;
            
            cellView.name = "Cell " + dataIndex.ToString();
            
            cellView.SetData(m_list[dataIndex]);

            return cellView;
        }
    }
}
