using UnityEngine;
using System.Collections.Generic;

namespace Siasm
{
    /// <summary>
    /// プレイヤー以外のラボ職員や通行人、クリシェミナなどNPC用に使用
    /// NOTE: このクラスの役割としてはSpawnで生成までを管理にした方がいいかも
    /// </summary>
    public abstract class BaseFieldCharacterController : MonoBehaviour
    {
        [SerializeField]
        private BaseFieldCharacter baseFieldCharacterPrefab;

        protected MainTalkController MainTalkController;
        protected MainQuestController MainQuestController;
        protected MainUIManager MainUIManager;
        protected MainStateMachineController MainStateMachineController;
        protected Camera MainCamera;

        protected List<BaseFieldCharacter> BaseFieldCharacters;

        protected BaseFieldCharacter BaseFieldCharacterPrefab => baseFieldCharacterPrefab;

        public void Initialize(MainTalkController mainTalkController, MainQuestController mainQuestController, MainUIManager mainUIManager,
            MainStateMachineController mainStateMachineController, Camera mainCamera)
        {
            MainTalkController = mainTalkController;
            MainQuestController = mainQuestController;
            MainUIManager = mainUIManager;
            MainStateMachineController = mainStateMachineController;
            MainCamera = mainCamera;
        }

        public void Setup()
        {
            BaseFieldCharacters = new List<BaseFieldCharacter>();
        }

        public void OnDesory()
        {
            foreach (var baseFieldCharacter in BaseFieldCharacters)
            {
                Destroy(baseFieldCharacter.gameObject);
            }

            BaseFieldCharacters.Clear();
        }
    }
}
