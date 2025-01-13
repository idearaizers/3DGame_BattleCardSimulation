using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// TabGroupとSwitchContentを紐付けて表示の出し分けを管理するクラス
    /// </summary>
    public class TabContentSwitcher : MonoBehaviour
    {
        [SerializeField]
        private TabGroup tabGroup;

        [SerializeField]
        private SwitchContent[] switchContents;

        public int ActiveIndex { get; private set; }
        public bool InputEnabled { get; private set; } = true;

        public void Initialize(int activeTabIndex)
        {
            tabGroup.SetActiveTab(activeTabIndex);
            InitializeActiveContent(activeTabIndex);

            tabGroup.OnChangeActiveTab += ChangeContent;
        }

        public void Setup() { }

        public void InitializeActiveContent(int index)
        {
            for (int i = 0; i < switchContents.Length; i++)
            {
                var tabContent = switchContents[i];
                tabContent.gameObject.SetActive(index == i);
            }

            ActiveIndex = index;
        }

        private void ChangeContent(int index)
        {
            ChangeActiveTabAsync(index).Forget();
        }

        private async UniTaskVoid ChangeActiveTabAsync(int inputIndex)
        {
            if (!InputEnabled || inputIndex == ActiveIndex)
            {
                return;
            }

            InputEnabled = false;

            var nextContent = switchContents[inputIndex];
            nextContent.BeforeShowAsync().Forget();

            var activeContent = switchContents[ActiveIndex];
            await activeContent.HideAsync();
            await nextContent.ShowAsync();

            ActiveIndex = inputIndex;
            InputEnabled = true;
        }

        private void OnDestroy()
        {
            tabGroup.OnChangeActiveTab -= ChangeContent;
        }
    }
}
