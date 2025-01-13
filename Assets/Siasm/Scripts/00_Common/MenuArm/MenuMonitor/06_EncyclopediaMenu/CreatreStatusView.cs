using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

namespace Siasm
{
    public class CreatreStatusView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI ditialText;

        [SerializeField]
        private Image creatureImage;

        private BaseUseCase BaseUseCase;

        public void Initialize(BaseUseCase BaseUseCase)
        {
            this.BaseUseCase = BaseUseCase;
        }

        public void Setup() { }

        public void UpdateView(CreatureRecordModel currentCreatureRecordModel)
        {
            var battleFighterStatusModel = BaseUseCase.CreateBattleFighterStatusModelOfEnemy(currentCreatureRecordModel.CreatureId, currentCreatureRecordModel.CreatureLevel);
            ditialText.text = $"MAX HP:{battleFighterStatusModel.MaxHealthPoint}\nMAX TP:{battleFighterStatusModel.MaxThinkingPoint}\n開始バトルボックス数:{battleFighterStatusModel.BeginBattleBoxNumber}\n最大バトルボックス数:{battleFighterStatusModel.MaxBattleBoxNumber}\n耐性:---\n弱点:---";

            SetImage(currentCreatureRecordModel.CreatureId).Forget();
        }

        private async UniTask SetImage(int creatureId)
        {
            var itemSpriteAddress = string.Format(AddressConstant.CreatureSpriteAddressStringFormat, creatureId);
            if (AssetCacheManager.Instance.Exist(itemSpriteAddress))
            {
                var cachedSprite = AssetCacheManager.Instance.GetAsset<Sprite>(itemSpriteAddress);
                creatureImage.sprite = cachedSprite;
            }

            await UniTask.CompletedTask;
        }
    }
}
