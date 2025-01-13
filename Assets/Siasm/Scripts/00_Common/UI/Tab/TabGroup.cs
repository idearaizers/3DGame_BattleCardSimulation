using System;
using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// 表示項目を切り替える際はTabContentSwitcherで管理することを想定したクラス
    /// </summary>
    public class TabGroup : MonoBehaviour
    {
        [SerializeField]
        private BaseTab[] baseTabs;

        private int activeTabIndex;

        public Action<int> OnChangeActiveTab;

        private void Start()
        {
            foreach (var baseTab in baseTabs)
            {
                baseTab.OnClickAction += OnClickTab;
            }
        }

        public void Initialize() { }

        public void Setup() { }

        private void OnDestroy()
        {
            foreach (var baseTab in baseTabs)
            {
                baseTab.OnClickAction -= OnClickTab;
            }
        }

        public void SetActiveTab(int index)
        {
            for (int i = 0; i < baseTabs.Length; i++)
            {
                if (i == index)
                {
                    baseTabs[i].SetSelected();
                }
                else
                {
                    baseTabs[i].SetDeselected();
                }
            }
        }

        private void OnClickTab(BaseTab selectedTab)
        {
            var selectedTabIndex = Array.IndexOf(baseTabs, selectedTab);
            if (activeTabIndex == selectedTabIndex)
            {
                return;
            }

            // 変更前を非選択に変更
            baseTabs[activeTabIndex].SetDeselected();

            // 変更したいものを選択に変更
            baseTabs[selectedTabIndex].SetSelected();

            OnChangeActiveTab?.Invoke(selectedTabIndex);

            activeTabIndex = selectedTabIndex;
        }
    }
}
