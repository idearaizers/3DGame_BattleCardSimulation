using Cysharp.Threading.Tasks;

namespace Siasm
{
    /// <summary>
    /// TODO: まだ組み込みできていないので別で実装予定
    /// </summary>
    public class MainSceneAssetLoader
    {
        private readonly AssetCacheManager assetCacheManager;
        private readonly SaveManager saveManager;
        private bool isCompleted;

        // [Inject]
        // public MainSceneAssetLoader(AssetCacheManager assetCacheManager, SaveManager saveManager)
        // {
        //     this.assetCacheManager = assetCacheManager;
        //     this.saveManager = saveManager;
        // }

        public async UniTask InitialLoadAsync()
        {
            if (isCompleted)
            {
                return;
            }

            await PreloadContentAssets();

            isCompleted = true;
        }

        public async UniTask PreloadContentAssets()
        {
            await UniTask.CompletedTask;

            // セーブデータをキャッシュしている時だけ実行
            // 必要なら確認用のログを追加
            if (saveManager.LoadedSaveDataCache == null)
            {
                return;
            }
        }
    }
}
