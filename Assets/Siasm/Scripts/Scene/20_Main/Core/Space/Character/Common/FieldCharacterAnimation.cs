using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace Siasm
{
    /// <summary>
    /// NOTE: プレイヤーとそれ以外で管理を変えたいので見直し予定
    /// </summary>
    public class FieldCharacterAnimation : MonoBehaviour
    {
        [SerializeField]
        private Image image;

        public void Initialize() { }

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

        public void ChangeFaceDirection(float faceDirection)
        {
            // TODO: 
        }
    }
}
