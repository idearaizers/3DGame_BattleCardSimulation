using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// ファイターの生成を管理するクラス
    /// </summary>
    public abstract class BaseBattleFighterSpawnController : MonoBehaviour
    {
        [SerializeField]
        private BaseBattleFighterPrefab baseBattleFighterPrefab;

        private BattleObjectPoolContainer battleObjectPoolContainer;
        private Camera mainCamera;

        public BaseBattleFighterPrefab InstanceBaseBattleFighterPrefab { get; private set; }

        public void Initialize(BattleObjectPoolContainer battleObjectPoolContainer, Camera mainCamera)
        {
            this.battleObjectPoolContainer = battleObjectPoolContainer;
            this.mainCamera = mainCamera;
        }

        public void Setup(bool isPlayer, BaseBattleFighterModel baseBattleFighterModel)
        {
            // アセットを取得
            var battleFighterAnimationTypeSprites = GetBattleFighterAnimationTypeSprites(baseBattleFighterModel);

            // 生成
            InstanceBaseBattleFighterPrefab = Instantiate(baseBattleFighterPrefab, transform);

            // 初期化
            InstanceBaseBattleFighterPrefab.Initialize(baseBattleFighterModel, isPlayer, mainCamera, battleObjectPoolContainer, battleFighterAnimationTypeSprites);
            InstanceBaseBattleFighterPrefab.Setup();
        }

        /// <summary>
        /// アセットを取得
        /// NOTE: 将来的にはバリエーション豊かにしたいのでそれに合わせた管理方法に変えた方が良さそう
        /// </summary>
        /// <param name="baseBattleFighterModel"></param>
        /// <returns></returns>
        private BattleFighterAnimationTypeSprites GetBattleFighterAnimationTypeSprites(BaseBattleFighterModel baseBattleFighterModel)
        {
            var assetPath = string.Format(BattleAddressConstant.BattleFighterAssetStringFromat, baseBattleFighterModel.FighterId);
            var battleFighterAnimationTypeSprites = AssetCacheManager.Instance.GetAsset<ScriptableObject>(assetPath);
            return battleFighterAnimationTypeSprites as BattleFighterAnimationTypeSprites;
        }

        public void OnDesory()
        {
            if (InstanceBaseBattleFighterPrefab != null)
            {
                Destroy(InstanceBaseBattleFighterPrefab.gameObject);
                InstanceBaseBattleFighterPrefab = null;
            }
        }
    }
}
