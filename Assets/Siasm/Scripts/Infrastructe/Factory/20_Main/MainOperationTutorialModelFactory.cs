using System.Linq;

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
            // // 解放しているがクリアしていないチュートリアルの一覧を取得する
            // var notClearSaveDataOperationTutorials = saveDataCache.SaveDataOperationTutorials.Where(SaveDataOperationTutorial => SaveDataOperationTutorial.IsClear == false).ToArray();
            // if (notClearSaveDataOperationTutorials.Length == 0)
            // {
            //     return null;
            // }

            // // クリアしていない中から一番idが低いものを取得するため、昇順に並び変える
            // notClearSaveDataOperationTutorials = notClearSaveDataOperationTutorials.OrderBy(x => x.TutorialId).ToArray();

            // // 0番目に格納されているもので検索を行う
            // var mainOperationTutorialMasterData = new MainOperationTutorialMasterData();
            // var mainOperationTutorialMasterDataModel = mainOperationTutorialMasterData.GetMainOperationTutorialMasterData(notClearSaveDataOperationTutorials[0]);

            // return new MainOperationTutorialModel
            // {
            //     TutorialId = mainOperationTutorialMasterDataModel.TutorialId,
            //     TitleText = mainOperationTutorialMasterDataModel.TitleText,
            //     DetialText = mainOperationTutorialMasterDataModel.DetialText,
            //     SubDetialText = mainOperationTutorialMasterDataModel.SubDetialText
            // };

            return null;
        }
    }
}
