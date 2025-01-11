namespace Enhanced
{
    public interface IEnhancedScrollerDelegate
    {
        int GetNumberOfCells(EnhancedScroller enhancedScroller);

        float GetCellViewSize(EnhancedScroller enhancedScroller, int dataIndex);

        EnhancedScrollerCellView GetCellView(EnhancedScroller enhancedScroller, int dataIndex, int cellIndex);
    }
}
