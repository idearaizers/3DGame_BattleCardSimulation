using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    /// <summary>
    /// カードに属性表示を付けるかどうかについて
    /// 選択中の表示などと合わせると情報過多になるので現状では説明テキストだけに設定
    /// 基本的には属性相性はダメージの補正なのでバトルボックスにカードを設定するところで相性のアラートが表示されていればいいかも
    /// </summary>
    public class PlayerBattleCardPrefab : MonoBehaviour, IMouseClickAction
    {
        [SerializeField]
        private Image cardImage;

        private bool isSelected;
        private Camera mainCamera;

        public BattleCardModel CurrentBattleCardModel { get; private set; }

        public Action<BattleCardModel> OnMousePointerEntoryAction { get; set; }
        public Action<PlayerBattleCardPrefab> OnReturnPositionAction { get; set; }
        public Action<PlayerBattleCardPrefab> OnMouseDragEndAction { get; set; }
        public Action<PlayerBattleCardPrefab, BattleCardModel> OnMouseDragEndOfChangeCardAction { get; set; }

        public void Initialize(Camera mainCamera)
        {
            this.mainCamera = mainCamera;
        }

        public void Setup(BattleCardModel battleCardModel)
        {
            CurrentBattleCardModel = battleCardModel;

            UpdateViewAsync(battleCardModel).Forget();
        }

        private async UniTask UpdateViewAsync(BattleCardModel battleCardModel)
        {
            // 画像を取得して反映する
            var itemSpriteAddress = string.Format(AddressConstant.BattleCardSpriteAddressStringFormat, battleCardModel.CardId);

            // アセットがある場合
            if (AssetCacheManager.Instance.Exist(itemSpriteAddress))
            {
                var cachedSprite = AssetCacheManager.Instance.GetAsset<Sprite>(itemSpriteAddress);
                cardImage.sprite = cachedSprite;
            }
            // アセットがない場合
            else
            {
                var cachedSprite = await AssetCacheManager.Instance.LoadAssetAsync<Sprite>(itemSpriteAddress);
                cardImage.sprite = cachedSprite;
            }
        }

        // bool isHand
        public void SetIsHand(CardPlaceType CardPositionType)
        {
            // IsHand = isHand;
            CurrentBattleCardModel.SetCardPlaceType(CardPositionType);
        }

        public GameObject GetGameObjectOfMouseLeftDragBegin()
        {
            // 手札にある時はドラッグできるので自身を返す
            if (CurrentBattleCardModel.CardPlaceType == CardPlaceType.Hand)
            {
                return this.gameObject;
            }

            // 手札にない時はドラッグできないのでnullを返す
            return null;
        }

        public void OnMouseLeftDragging() { }

        public void OnMouseLeftDragged()
        {
            var rayOfMainCamera = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHitOfMainCamera;
            if (Physics.Raycast(rayOfMainCamera, out raycastHitOfMainCamera))
            {
                // カードをドラッグしてバトルボックスに配置した時の処理
                var battleBoxPrefab = raycastHitOfMainCamera.collider.GetComponent<BattleBoxPrefab>();
                if (battleBoxPrefab != null &&
                    battleBoxPrefab.IsPlayer)
                {
                    // セットしているカードがなければ自身をセットする
                    if (battleBoxPrefab.CurrentBattleCardModel == null)
                    {
                        battleBoxPrefab.OnMouseDragEnd(this);
                        MouseDragEnd();
                    }
                    // セットしているカードがあれば自身と入れ替える
                    else
                    {
                        battleBoxPrefab.OnMouseDragEnd(this);
                        OnMouseDragEndOfChangeCardAction?.Invoke(this, battleBoxPrefab.CurrentBattleCardModel);
                    }

                    // 下記は不要なのでコメントアウト
                    // あとで整理したい
                    // 手札からなくなったので変更する
                    // IsHand = false;
                    // CurrentBattleCardModel.SetCardPlaceType(CardPlaceType.None);
                }
                // バトルボックスにドラッグしていなければ移動前の場所に戻す
                else
                {
                    // 移動前の場所に手札のカードを戻す
                    OnReturnPositionAction?.Invoke(this);
                }
            }
        }

        public void OnMousePointerEntory()
        {
            // 手札になければ実行しない
            if (CurrentBattleCardModel.CardPlaceType != CardPlaceType.Hand)
            {
                return;
            }

            // 選択中であれば実行しない
            if (isSelected)
            {
                return;
            }

            isSelected = true;

            AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);

            var tmpPosition = transform.localPosition;
            tmpPosition.z = 0.015f;
            transform.localPosition = tmpPosition;

            OnMousePointerEntoryAction?.Invoke(CurrentBattleCardModel);
        }

        public void OnMousePointerExit()
        {
            // 手札になければ実行しない
            if (CurrentBattleCardModel.CardPlaceType != CardPlaceType.Hand)
            {
                return;
            }

            isSelected = false;

            var tmpPosition = transform.localPosition;
            tmpPosition.z = 0.0f;
            transform.localPosition = tmpPosition;
        }

        public void OnMouseLeftClick() { }

        public void OnMouseRightClick() { }

        public void MouseDragEnd()
        {
            OnMouseDragEndAction?.Invoke(this);
        }
    }
}
