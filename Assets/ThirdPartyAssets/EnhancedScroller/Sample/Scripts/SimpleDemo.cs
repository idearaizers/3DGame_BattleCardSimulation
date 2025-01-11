using UnityEngine;

namespace Enhanced
{
    public class SimpleDemo : MonoBehaviour, IEnhancedScrollerDelegate
    {
        private SmallList<Data> _data;

        public EnhancedScroller scroller;

        public EnhancedScrollerCellView cellViewPrefab;

        private void Start()
        {
            Application.targetFrameRate = 60;

            scroller.Delegate = this;

            LoadLargeData();
        }

        private void LoadLargeData()
        {
            _data = new SmallList<Data>();

            for (int i = 0; i < 1000; i++)
            {
                _data.Add(new Data()
                {
                    someText = "Cell Data Index " + i.ToString()
                });
            }

            scroller.ReloadData();
        }

        private void LoadSmallData()
        {
            _data = new SmallList<Data>();

            _data.Add(new Data() { someText = "A" });
            _data.Add(new Data() { someText = "B" });
            _data.Add(new Data() { someText = "C" });

            scroller.ReloadData();
        }

        public void LoadLargeDataButton_OnClick()
        {
            LoadLargeData();
        }

        public void LoadSmallDataButton_OnClick()
        {
            LoadSmallData();
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return _data.Count;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return (dataIndex % 2 == 0 ? 30f : 100f);
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var cellView = scroller.GetCellView(cellViewPrefab) as CellView;

            cellView.name = "Cell " + dataIndex.ToString();

            cellView.SetData(_data[dataIndex]);

            return cellView;
        }
    }
}
