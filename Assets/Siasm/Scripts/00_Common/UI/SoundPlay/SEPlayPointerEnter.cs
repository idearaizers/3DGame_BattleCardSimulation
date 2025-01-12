using UnityEngine;
using UnityEngine.EventSystems;

namespace Siasm
{
    public sealed class SEPlayPointerEnter : BaseSEPlay, IPointerEnterHandler
    {
        [SerializeField]
        private ButtonType selectButtonType = ButtonType.OnMouseCard;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (selectButtonType == ButtonType.None)
            {
                return;
            }

            var battleCommonSETypeAudioClipsScriptableObject = AssetCacheManager.Instance.GetAsset<ScriptableObject>(BattleCommonSETypeAudioClips.AssetName);
            var battleCommonSETypeAudioClips = battleCommonSETypeAudioClipsScriptableObject as BattleCommonSETypeAudioClips;
            AudioManager.Instance.PlaySEOfAudioClip(BaseAudioPlayer.PlayType.Single, battleCommonSETypeAudioClips.GetAudioClip(AudioSEType.OnMouseCard));
        }
    }
}
