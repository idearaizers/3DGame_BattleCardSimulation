using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Siasm
{
    public sealed class NewPlayDialog : BaseDialog
    {
        public static readonly string AssetAddress = "NewPlayDialogPrefab";

        private const string saveSlotNameFormat = "SaveSlot{0}";
        private int saveSlotIndex;

        [SerializeField]
        private TextMeshProUGUI saveSlotName;

        [SerializeField]
        private Button cancelButton;

        [SerializeField]
        private Button decideButton;

        public Action OnCancelAction { get; set; }
        public Action<int> OnDecideAction { get; set; }

        public override void Initialize()
        {
            cancelButton.onClick.AddListener(OnCancel);
            decideButton.onClick.AddListener(OnDecide);
        }

        public void Apply(int saveSlotIndex)
        {
            this.saveSlotIndex = saveSlotIndex;
            saveSlotName.text = string.Format(saveSlotNameFormat, saveSlotIndex);
        }

        private void OnCancel()
        {
            AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Cancel);

            OnCancelAction?.Invoke();
        }

        private void OnDecide()
        {
            AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);

            OnDecideAction?.Invoke(saveSlotIndex);
        }
    }
}
