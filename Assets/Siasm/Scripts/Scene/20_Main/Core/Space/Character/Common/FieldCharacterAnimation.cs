using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace Siasm
{
    /// <summary>
    /// NOTE: 見直し予定
    /// </summary>
    public class FieldCharacterAnimation : MonoBehaviour
    {
        [SerializeField]
        private Image image;

        // private BattleFighterAnimationTypeSprites battleFighterAnimationTypeSprites;

        // bool isPlayer, BattleFighterAnimationTypeSprites battleFighterAnimationTypeSprites
        public void Initialize()
        {
            // if (isPlayer)
            // {
            //     this.transform.localScale = Vector3.one;
            // }
            // // NOTE: エネミーの場合はスケールを使用して向きを変えている
            // else
            // {
            //     this.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            // }

            // this.battleFighterAnimationTypeSprites = battleFighterAnimationTypeSprites;

            // // 初期は待機状態に変更
            // SetImage(BattleFighterAnimationType.Idle);
        }

        public void Setup(int creatureId = -1)
        {
            if (creatureId != -1)
            {
                SetImage(creatureId).Forget();
            }
        }

        private async UniTask SetImage(int creatureId)
        {
            var itemSpriteAddress = string.Format(AddressConstant.CreatureSpriteAddressStringFormat, creatureId);
            if (AssetCacheManager.Instance.Exist(itemSpriteAddress))
            {
                var cachedSprite = AssetCacheManager.Instance.GetAsset<Sprite>(itemSpriteAddress);
                image.sprite = cachedSprite;
            }
            else
            {
                var cachedSprite = await AssetCacheManager.Instance.LoadAssetAsync<Sprite>(itemSpriteAddress);
                image.sprite = cachedSprite;
            }
        }

        // BattleFighterAnimationType battleFighterAnimationType
        public void SetImage()
        {
            // image.sprite = battleFighterAnimationTypeSprites.GetSprite(battleFighterAnimationType);
        }

        // faceDirection
        public void ChangeFaceDirection(float faceDirection)
        {
            // if (isPlayer)
            // {
            //     this.transform.localScale = Vector3.one;
            // }
            // // NOTE: エネミーの場合はスケールを使用して向きを変えている
            // else
            // {
            //     this.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            // }
        }
    }
}
