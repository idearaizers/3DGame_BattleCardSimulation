using System;
using UnityEngine;
using DG.Tweening;

namespace Siasm
{
    public class AdmissionRecordView : BaseView
    {
        [SerializeField]
        private GameObject slidePanelGameObject;

        [SerializeField]
        private TabButton encyclopediaTabButton;

        [SerializeField]
        private CreatureRecordScrollController creatureRecordScrollController;

        private GameObject currentSelectedGameObject;

        public Action<CreatureRecordModel> OnClickAction { get; set; }

        public void Initialize()
        {
            encyclopediaTabButton.Initialize();
            encyclopediaTabButton.OnClickAction = OnEncyclopediaTabButton;

            creatureRecordScrollController.Initialize();
            creatureRecordScrollController.OnClickAction = OnClick;

            currentSelectedGameObject = null;
        }

        public void Setup(CreatureRecordModel[] creatureRecordModels, int currentIndex)
        {
            encyclopediaTabButton.Setup();
            creatureRecordScrollController.Setup(creatureRecordModels, currentIndex);
        }

        private void OnEncyclopediaTabButton(bool isActive)
        {
            var targetPosition = (isActive == true)
                ? new Vector3(-0.93f, 0.0f, 0.0f)
                : new Vector3(-0.03f, 0.0f, 0.0f);

            var sequence = DOTween.Sequence();
            sequence.Append
                    (
                        slidePanelGameObject.transform.DOLocalMove(targetPosition, 0.2f)
                    )
                    .SetLink(gameObject);
        }

        private void OnClick(GameObject selectedGameObject, CreatureRecordModel creatureRecordModel)
        {
            if (currentSelectedGameObject != null &&
                currentSelectedGameObject != selectedGameObject)
            {
                currentSelectedGameObject.GetComponent<CreatureRecordCellView>().ChangeActiveOfSelectedImage(false);
            }

            currentSelectedGameObject = selectedGameObject;

            OnClickAction?.Invoke(creatureRecordModel);
        }

        public void ShowSelected(int selectedIndex)
        {
            // TODO: 指定したオブジェクトを選択状態にする
        }
    }
}
