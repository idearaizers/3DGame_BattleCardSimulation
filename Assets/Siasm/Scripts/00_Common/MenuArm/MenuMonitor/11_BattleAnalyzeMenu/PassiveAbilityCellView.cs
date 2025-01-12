using Enhanced;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Siasm
{
    /// <summary>
    /// もしモデルが空の場合はContainerとなっているGameObjectを非アクティブにする必要がある
    /// また必要になったらContainerGameObjectの参照を追加する
    /// </summary>
    public sealed class PassiveAbilityCellView : EnhancedScrollerCellView
    {
        private const string unOpenedStringFormat = "Lv.{0}で解放";

        public abstract class BaseViewPrameter
        {
            public BasePassiveAbilityModel PassiveAbilityModel { get; set; }
        }

        public sealed class PlayerViewPrameter : BaseViewPrameter { }

        public sealed class EnemyViewPrameter : BaseViewPrameter
        {
            public int FighterLevel { get; set; }

            /// <summary>
            /// パッシブアビリティが開放されているかどうか
            /// NOTE: PassiveAbilityModelから取得の方がいいかも
            /// </summary>
            /// <returns></returns>
            public bool GetIsReleaseOfPassiveAbility()
            {
                if (FighterLevel >= PassiveAbilityModel.ReleaseLevel)
                {
                    return true;
                }

                return false;
            }
        }

        [Space]
        [SerializeField]
        private PassiveAbilityTypeSprites passiveAbilityTypeSprites;

        // 解放
        [Space]
        [SerializeField]
        private GameObject openPanelGameObject;

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private TextMeshProUGUI nameText;

        [SerializeField]
        private TextMeshProUGUI descriptionText;

        // 未解放
        [Space]
        [SerializeField]
        private GameObject unOpenedPanelGameObject;

        [SerializeField]
        private TextMeshProUGUI unOpenedText;

        private void Start() { }

        /// <summary>
        /// Cell数が1つだけのため表示の出し分けはせずに表示
        /// </summary>
        /// <param name="baseViewPrameter"></param>
        public void SetData(BaseViewPrameter baseViewPrameter)
        {
            switch (baseViewPrameter)
            {
                case PlayerViewPrameter:
                    {
                        openPanelGameObject.SetActive(true);
                        unOpenedPanelGameObject.SetActive(false);

                        nameText.text = baseViewPrameter.PassiveAbilityModel.PassiveAbilityName;
                        iconImage.sprite = passiveAbilityTypeSprites.GetSprite(baseViewPrameter.PassiveAbilityModel.PassiveAbilityType);
                        var battlePassiveAbilityConstant = new BattlePassiveAbilityConstant();
                        var parameter = battlePassiveAbilityConstant.GetParameter(baseViewPrameter.PassiveAbilityModel);
                        descriptionText.text = parameter.Description;
                    }
                    break;
                case EnemyViewPrameter:
                    {
                        // 必要なレベルに到達しているかどうかで表示を出し分ける
                        var enemyViewPrameter = baseViewPrameter as EnemyViewPrameter;
                        if (enemyViewPrameter.GetIsReleaseOfPassiveAbility())
                        {
                            openPanelGameObject.SetActive(true);
                            unOpenedPanelGameObject.SetActive(false);

                            nameText.text = baseViewPrameter.PassiveAbilityModel.PassiveAbilityName;
                            iconImage.sprite = passiveAbilityTypeSprites.GetSprite(baseViewPrameter.PassiveAbilityModel.PassiveAbilityType);
                            var battlePassiveAbilityConstant = new BattlePassiveAbilityConstant();
                            var parameter = battlePassiveAbilityConstant.GetParameter(baseViewPrameter.PassiveAbilityModel);
                            descriptionText.text = parameter.Description;
                        }
                        else
                        {
                            openPanelGameObject.SetActive(false);
                            unOpenedPanelGameObject.SetActive(true);

                            unOpenedText.text = string.Format(unOpenedStringFormat, baseViewPrameter.PassiveAbilityModel.ReleaseLevel);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
