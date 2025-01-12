using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Siasm
{
    public abstract class BaseMatchReelPrefab : BaseView
    {
        private const string attackLabelTextStringFormat = "残りボックス数 {0}";
        private const string reelMixAndMaxNumberStringFormat = "{0}～{1}";

        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private TextMeshProUGUI reelMixAndMaxNumberText;

        [SerializeField]
        private BattleCardReelView battleCardReelView;

        [SerializeField]
        private TextMeshProUGUI cardNameText;

        [SerializeField]
        private Image cardImage;

        [SerializeField]
        private TextMeshProUGUI remainingText;

        private IEnumerator currentCoroutine;

        public void Initialize(Camera mainCamera)
        {
            canvas.worldCamera = mainCamera;

            battleCardReelView.Initialize();
        }

        public void Setup() { }

        /// <summary>
        /// リール演出を開始する
        /// </summary>
        /// <param name="battleCardModel"></param>
        /// <param name="remainingBattleCardNumber">残り枚数で現在使用しているカードは含まない数を渡す</param>
        public void PlayReel(BattleCardModel battleCardModel, int remainingBattleCardNumber)
        {
            reelMixAndMaxNumberText.text = string.Format(reelMixAndMaxNumberStringFormat, battleCardModel.MinReelNumber, battleCardModel.MaxReelNumber);
            cardNameText.text = battleCardModel.CardName;

            UpdateViewAsync(battleCardModel).Forget();

            remainingText.text = string.Format(attackLabelTextStringFormat, remainingBattleCardNumber);

            currentCoroutine = PlayReelCoroutine(battleCardModel);
            StartCoroutine(currentCoroutine);
        }

        private async UniTask UpdateViewAsync(BattleCardModel battleCardModel)
        {
            var itemSpriteAddress = string.Format(AddressConstant.BattleCardSpriteAddressStringFormat, battleCardModel.CardId);
            if (AssetCacheManager.Instance.Exist(itemSpriteAddress))
            {
                var cachedSprite = AssetCacheManager.Instance.GetAsset<Sprite>(itemSpriteAddress);
                cardImage.sprite = cachedSprite;
            }
            else
            {
                var cachedSprite = await AssetCacheManager.Instance.LoadAssetAsync<Sprite>(itemSpriteAddress);
                cardImage.sprite = cachedSprite;
            }
        }

        /// <summary>
        /// StopCoroutineを別で適用
        /// </summary>
        /// <param name="battleCardModel"></param>
        /// <returns></returns>
        private IEnumerator PlayReelCoroutine(BattleCardModel battleCardModel)
        {
            var minNumber = battleCardModel.MinReelNumber;
            var maxNumber = battleCardModel.MaxReelNumber;
            var currentNumber = minNumber;

            battleCardReelView.Setup(battleCardModel);

            while (true)
            {
                battleCardReelView.Apply(currentNumber);
                yield return new WaitForSeconds(0.04f);     // 0.05f

                currentNumber++;
                if (currentNumber > maxNumber)
                {
                    currentNumber = minNumber;
                }
            }
        }

        public void StopReelCoroutine(int number)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            battleCardReelView.Apply(number);
        }
    }
}
