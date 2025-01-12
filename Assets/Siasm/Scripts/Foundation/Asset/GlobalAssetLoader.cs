using Cysharp.Threading.Tasks;
using VContainer;

namespace Siasm
{
    /// <summary>
    /// 起動から終了まで常にメモリ上に置くアセット
    /// シーン共通で使用するものを管理
    /// </summary>
    public class GlobalAssetLoader
    {
        private readonly AssetCacheManager assetCacheManager;
        private readonly SaveManager saveManager;
        private bool isCompleted;

        [Inject]
        public GlobalAssetLoader(AssetCacheManager assetCacheManager, SaveManager saveManager)
        {
            this.assetCacheManager = assetCacheManager;
            this.saveManager = saveManager;
        }

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

            // 仮
            // var cardTasks = uniquCard.Select(x => x.assetCacheManager.LoadAssetAsync<Sprite>("adress"));
            // await UniTask.WhenAll();

            // 仮
            // await assetCacheManager.LoadAssetAsync<Sprite>("aaa");
        }
    }
}
