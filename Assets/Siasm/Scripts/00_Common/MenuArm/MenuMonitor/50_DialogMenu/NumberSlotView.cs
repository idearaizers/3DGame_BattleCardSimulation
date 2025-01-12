using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Siasm
{
    public class NumberSlotView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI numberText;

        [SerializeField]
        private Button countUpButton;

        [SerializeField]
        private Button countDownButton;

        public int CurrentNumber { get; private set;}

        public Action OnClickAction { get; set; }

        public void Initialize()
        {
            CurrentNumber = 0;
            numberText.text = CurrentNumber.ToString();

            countUpButton.onClick.AddListener(OnCountUpButton);
            countDownButton.onClick.AddListener(OnCountDownButton);
        }

        public void Setup()
        {
            // 
        }

        private void OnCountUpButton()
        {
            if (CurrentNumber == 9)
            {
                CurrentNumber = 0;
            }
            else
            {
                CurrentNumber++;
            }

            numberText.text = CurrentNumber.ToString();

            OnClickAction?.Invoke();
        }

        private void OnCountDownButton()
        {
            if (CurrentNumber == 0)
            {
                CurrentNumber = 9;
            }
            else
            {
                CurrentNumber--;
            }

            numberText.text = CurrentNumber.ToString();

            OnClickAction?.Invoke();
        }
    }
}
