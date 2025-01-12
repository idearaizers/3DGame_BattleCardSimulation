using System;
using UnityEngine;

namespace Siasm
{
    [Serializable]
    public class MenuPrefabTypePrefabDictionary : SerializableDictionary<MenuPrefabType, GameObject> { }

    [CreateAssetMenu(fileName = "MenuPrefabTypePrefabs", menuName ="EnumAssetSetting/MenuPrefabTypePrefabs")]
    public class MenuPrefabTypePrefabs : ScriptableObject
    {
        [SerializeField]
        private MenuPrefabTypePrefabDictionary menuPrefabTypePrefabDictionary;

        public GameObject GetGameObject(MenuPrefabType menuPrefabType)
        {
            return menuPrefabTypePrefabDictionary[menuPrefabType];
        }
    }
}
