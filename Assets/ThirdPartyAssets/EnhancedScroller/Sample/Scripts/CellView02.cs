namespace Enhanced
{
    public class CellView02 : EnhancedScrollerCellView
    {
        public RowCellView02[] rowCellViews;

        // NOTE: ここにアクションかな

        public void SetData(ref SmallList<Data> data, int startingIndex)
        {
            for (int i = 0; i < rowCellViews.Length; i++)
            {
                // NOTE: ここでアクションの登録かな
                rowCellViews[i].SetData(startingIndex + i < data.Count ? data[startingIndex + i] : null);
            }
        }
    }
}
