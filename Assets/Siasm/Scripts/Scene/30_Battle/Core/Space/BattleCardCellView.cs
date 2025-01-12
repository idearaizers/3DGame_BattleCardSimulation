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

        // [SerializeField]
        // private ScrollCardView scrollCardView;

        private BattleCardModel battleCardModel;

        public Action<BattleCardModel> OnClickAction { get; set; }

        public void SetData(BattleCardModel battleCardModel)
        {
            this.battleCardModel = battleCardModel;

            containerGameObject.SetActive(battleCardModel != null);

            if (battleCardModel != null)
            {
                battleCard.Apply(battleCardModel);

                // scrollCardView.Initialize(battleCardModel);
                // scrollCardView.OnClickAction = OnClick;
            }
        }

        private void OnClick()
        {
            OnClickAction?.Invoke(battleCardModel);
        }
    }
}
