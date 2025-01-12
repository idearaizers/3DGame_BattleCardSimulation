using Enhanced;
using UnityEngine;

namespace Siasm
{
    public sealed class MenuAnalyzePassiveAbilityController : BaseScrollController, IEnhancedScrollerDelegate
    {
        private SmallList<PassiveAbilityCellView.BaseViewPrameter> PassiveAbilityCellViewPrameters;

        public override void Initialize()
        {
            base.Initialize();

            EnhancedScroller.Delegate = this;
        }

        public void Setup(BaseBattleFighterModel baseBattleFighterModel)
        {
            this.PassiveAbilityCellViewPrameters = new SmallList<PassiveAbilityCellView.BaseViewPrameter>();

            // プレイヤーとエネミーで表示を出し分けるためのパラメータを作成する
            foreach (var passiveAbilityModel in baseBattleFighterModel.BasePassiveAbilityModels)
            {
                switch (baseBattleFighterModel)
                {
                    case PlayerBattleFighterModel:
                        PassiveAbilityCellViewPrameters.Add(new PassiveAbilityCellView.PlayerViewPrameter
                        {
                            PassiveAbilityModel = passiveAbilityModel
                        });
                        break;
                    case EnemyBattleFighterModel:
                        var enemyBattleFighterModel = baseBattleFighterModel as EnemyBattleFighterModel;
                        PassiveAbilityCellViewPrameters.Add(new PassiveAbilityCellView.EnemyViewPrameter
                        {
                            PassiveAbilityModel = passiveAbilityModel,
                            FighterLevel = enemyBattleFighterModel.FighterLevel
                        });
                        break;
                    default:
                        break;
                }
            }

            // 追加してからベースを実行
            base.Setup();
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return Mathf.CeilToInt((float)PassiveAbilityCellViewPrameters.Count / (float)NumberOfCellsPerRow);
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var cellView = scroller.GetCellView(EnhancedScrollerCellViewPrefab) as RowPassiveAbilityCellView;
            cellView.name = GetCellNameText(dataIndex);
            cellView.SetData(ref PassiveAbilityCellViewPrameters, dataIndex * NumberOfCellsPerRow);
            return cellView;
        }
    }
}
