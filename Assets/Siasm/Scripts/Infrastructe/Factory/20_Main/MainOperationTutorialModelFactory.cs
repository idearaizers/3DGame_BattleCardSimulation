namespace Siasm
{
    public class MainOperationTutorialModel
    {
        public int TutorialId { get; set; }
        public string TitleText { get; set; }
        public string DetialText { get; set; }
        public string SubDetialText { get; set; }
    }

    /// <summary>
    /// 解放しているがクリアしていないチュートリアルの中からIdが一番低いものを取得する
    /// </summary>
    /// <param name="saveDataCache"></param>
    /// <returns></returns>
    public class MainOperationTutorialModelFactory
    {
        public MainOperationTutorialModel CreateMainOperationTutorialModel(SaveDataCache saveDataCache)
        {
            // TODO: 

            return null;
        }
    }
}
