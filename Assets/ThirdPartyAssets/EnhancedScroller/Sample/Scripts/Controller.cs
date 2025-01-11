using UnityEngine;

namespace Enhanced
{
    public class Controller : MonoBehaviour, IEnhancedScrollerDelegate
    {
        private SmallList<Data> _data;

        public EnhancedScroller scroller;
        
        public EnhancedScrollerCellView cellViewPrefab;

        public int numberOfCellsPerRow = 3;

        private void Start()
        {
            Application.targetFrameRate = 60;

            scroller.Delegate = this;

            LoadData();
        }

        private void LoadData()
        {
            _data = new SmallList<Data>();

            for (int i = 0; i < 5; i++)
            {
                _data.Add(new Data() { someText = i.ToString() });
            }

            scroller.ReloadData();
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return Mathf.CeilToInt((float)_data.Count / (float)numberOfCellsPerRow);
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return 100;
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            CellView02 cellView = scroller.GetCellView(cellViewPrefab) as CellView02;

            cellView.name = "Cell " + (dataIndex * numberOfCellsPerRow).ToString() + "to " + ((dataIndex * numberOfCellsPerRow) + numberOfCellsPerRow - 1).ToString();

            cellView.SetData(ref _data, dataIndex * numberOfCellsPerRow);

            return cellView;
        }
    }
}
