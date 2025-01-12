using System;
using UnityEngine;

namespace Siasm
{
    [Serializable]
    public class DialogMenuPrefabDictionary : SerializableDictionary<DialogMenuType, GameObject> { }

    [CreateAssetMenu(fileName = "DialogMenuPrefabs", menuName ="EnumAssetSetting/DialogMenuPrefabs")]
    public class DialogMenuPrefabs : ScriptableObject
    {
        [SerializeField]
        private DialogMenuPrefabDictionary dialogMenuPrefabDictionary;

        public GameObject GetGameObject(DialogMenuType dialogType)
        {
            return dialogMenuPrefabDictionary[dialogType];
        }
    }
}
