using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    /// <summary>
    /// AudioManager.Instanceで参照できるのでこれは不要かも
    /// 管理が複雑になった際にこちらを使用する
    /// </summary>
    public class ButtonAudioPlayer : MonoBehaviour
    {
        private enum ButtonType
        {
            None = 0,
            Decide,
            Cancel
        }

        // private readonly Dictionary<ButtonType, string> buttonTypeDictionary = new ()
        // {
        //     { ButtonType.Decide, AudioSEConstant.DecideSE },
        //     { ButtonType.Cancel, AudioSEConstant.CancelSE }
        // };

        [SerializeField]
        private Button button;

        [SerializeField]
        private ButtonType buttonType;

        private void Start()
        {
            button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            // AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, buttonTypeDictionary[buttonType]);
        }
    }
}
