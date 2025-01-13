using System;
using Enhanced;
using UnityEngine;

namespace Siasm
{
    public sealed class BattleCardCellView : EnhancedScrollerCellView
    {
        [SerializeField]
        private GameObject containerGameObject;

        [SerializeField]
        private BattleCard battleCard;

        public Action<BattleCardModel> OnClickAction { get; set; }

        public void SetData(BattleCardModel battleCardModel)
        {
            containerGameObject.SetActive(battleCardModel != null);

            if (battleCardModel != null)
            {
                battleCard.Initialize();
                battleCard.Apply(battleCardModel);
            }
        }
    }
}
