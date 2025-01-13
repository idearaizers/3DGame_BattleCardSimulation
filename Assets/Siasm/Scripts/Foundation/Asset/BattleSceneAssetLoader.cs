using System.Linq;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Siasm
{
    public class BattleSceneAssetLoader
    {
        private readonly List<string> cachedKeys = new List<string>()
        {
            BattleCommonSETypeAudioClips.AssetName
        };

        /// <summary>
        /// バトルに必要なアセットをUseCaseのIdから用意する
        /// </summary>
        public void AddCachedKeysOfPreloadAsset(BattleUseCase battleUseCase)
        {
            // プレイヤーファイター関連
            var playerBattleFighterAssetPath = string.Format(BattleAddressConstant.BattleFighterAssetStringFromat, battleUseCase.BattleModel.PlayerBattleFighterModel.FighterId);
            cachedKeys.Add(playerBattleFighterAssetPath);

            // エネミーファイター関連
            var enemyBattleFighterAssetPath = string.Format(BattleAddressConstant.BattleFighterAssetStringFromat, battleUseCase.BattleModel.EnemyBattleFighterModel.FighterId);
            cachedKeys.Add(enemyBattleFighterAssetPath);

            // プレイヤーとエネミーのカード関連
            var battleDeckModels = battleUseCase.BattleModel.BattlePlayerDeckModel.BattleDeckModels;
            var cardIds = new List<int>();

            // プレイヤーの全てのカードを事前ロードする
            foreach (var battleDeckModel in battleDeckModels)
            {
                var playerCardIds = battleDeckModel.BattleCardModels.Select(x => x.CardId);
                cardIds.AddRange(playerCardIds);
            }

            var enemyCardIds = battleUseCase.BattleModel.EnemyBattleFighterModel.BattleDeckModel.BattleCardModels.Select(x => x.CardId);
            cardIds.AddRange(enemyCardIds);
            cardIds = cardIds.Distinct().ToList();

            foreach (var cardId in cardIds)
            {
                var cardImageSpriteAddress = string.Format(AddressConstant.BattleCardSpriteAddressStringFormat, cardId);
                cachedKeys.Add(cardImageSpriteAddress);
            }

            // アナライズ用の画像をキャッシュ
            var creatureSpriteAddressOfPlayer = string.Format(AddressConstant.CreatureSpriteAddressStringFormat, battleUseCase.BattleModel.PlayerBattleFighterModel.FighterId);
            cachedKeys.Add(creatureSpriteAddressOfPlayer);

            var creatureSpriteAddressOfEnemy = string.Format(AddressConstant.CreatureSpriteAddressStringFormat, battleUseCase.BattleModel.EnemyBattleFighterModel.FighterId);
            cachedKeys.Add(creatureSpriteAddressOfEnemy);
        }

        /// <summary>
        /// cachedKeysを基にアセットを事前キャッシュする
        /// NOTE: 型の指定は見直した方がよさそう
        /// </summary>
        /// <returns></returns>
        public async UniTask LoadAndChachOfPreloadAsset()
        {
            var assets = new List<string>();

            foreach (var cachedKey in cachedKeys)
            {
                if (!AssetCacheManager.Instance.Exist(cachedKey))
                {
                    assets.Add(cachedKey);
                }
            }

            foreach (var asset in assets)
            {
                if (asset.Contains(AddressConstant.BattleCardSpriteAddressName) ||
                    asset.Contains(AddressConstant.CreatureSpriteAddressName))
                {
                    await AssetCacheManager.Instance.LoadAssetAsync<Sprite>(asset);
                }
                else
                {
                    await AssetCacheManager.Instance.LoadAssetAsync<ScriptableObject>(asset);
                }
            }
        }

        public void UnLoadChachedAsset()
        {
            foreach (var cachedKey in cachedKeys)
            {
                AssetCacheManager.Instance.Release(cachedKey);
            }
        }
    }
}
