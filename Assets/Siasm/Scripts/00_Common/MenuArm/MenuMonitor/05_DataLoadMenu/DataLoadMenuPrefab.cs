using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;

namespace Siasm
{
    public sealed class DataLoadMenuPrefab : BaseSaveDataMenuPrefab
    {
        private const string detialStringFormat = "ファイル{0}をロードしますか？";

        [Space]
        [SerializeField]
        private AssetReference mainSceneAssetRefrence;

        public override void Setup(bool isActive)
        {
            base.Setup(isActive);

            // 使用しない場合は実行しない
            if (!isActive)
            {
                return;
            }
        }

        protected override void OnClickCellView(SaveSlotCellView selectedSaveSlotCellView, bool isData)
        {
            // セーブデータがなければ開かない
            if (!isData)
            {
                // サイドアームだが開いていれば閉じる
                if (SideArmSwitcherPrefab.IsOpen)
                {
                    SideArmSwitcherPrefab.PlayCloseAnimation();
                }

                return;
            }

            // 選択中のindexを取得
            var selectedIndex = Array.IndexOf(SaveSlotCellViews, selectedSaveSlotCellView);

            // 表示用に+1した数を使用する
            var selectedViewIndex = selectedIndex + 1;

            // サイドアームを開く
            SideArmSwitcherPrefab.PlayOpenDisplayAnimation(
                string.Format(detialStringFormat, selectedViewIndex),
                () =>
                {
                    OnYesButton(selectedIndex).Forget();
                },
                () =>
                {
                    // NOTE: キャンセルの際は何もしない
                }
            );
        }

        private async UniTask OnYesButton(int selectedIndex)
        {
            // BGMをフェードアウト
            AudioManager.Instance.FadeOutBGM();

            // フェードアウト処理
            await OverlayManager.Instance.FadeInAsync();

            // セーブデータをロードする
            SaveManager.Instance.LoadAndCacheSaveData(selectedIndex);

            // メインシーンを読み込む
            var mainSceneCustomLoader = new MainSceneCustomLoader(AssetCacheManager.Instance);
            SceneLoadManager.Instance.LoadSceneAsync(mainSceneAssetRefrence, mainSceneCustomLoader).Forget();
        }
    }
}
