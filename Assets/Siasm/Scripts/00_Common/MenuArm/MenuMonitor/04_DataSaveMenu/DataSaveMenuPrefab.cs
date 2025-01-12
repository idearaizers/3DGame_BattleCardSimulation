using System;

namespace Siasm
{
    public sealed class DataSaveMenuPrefab : BaseSaveDataMenuPrefab
    {
        private const string detialStringFormat = "ファイル{0}にセーブしますか？";

        public override void Setup(bool isEnable)
        {
            base.Setup(isEnable);

            if (!isEnable)
            {
                return;
            }
        }

        /// <summary>
        /// セーブデータがない場所にもセーブ可能
        /// </summary>
        /// <param name="selectedSaveSlotCellView"></param>
        /// <param name="isData"></param>
        protected override void OnClickCellView(SaveSlotCellView selectedSaveSlotCellView, bool isData)
        {
            // サイドアームだが開いていれば閉じる
            if (SideArmSwitcherPrefab.IsOpen)
            {
                SideArmSwitcherPrefab.PlayCloseAnimation();
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
                    OnYesButton(selectedIndex);
                },
                () =>
                {
                    // NOTE: キャンセルの際は何もしない
                }
            );
        }

        private void OnYesButton(int selectedIndex)
        {
            SaveManager.Instance.SaveJsonData(selectedIndex);

            // 仮
            // セーブ後に表示を最新に更新する
            Setup(true);
        }
    }
}
