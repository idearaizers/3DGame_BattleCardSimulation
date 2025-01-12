using UnityEngine;

namespace Siasm
{
    public static class PlayerPrefsStorage
    {
        public static T Get<T>() where T : new()
        {
            var key = Key<T>();

            if (!PlayerPrefs.HasKey(key))
            {
                Debug.Log($"<color=yellow>データがないためPlayerPrefsから新しく作成しました</color> => key名: {key}");
                return new T();
            }

            Debug.Log($"<color=yellow>データをPlayerPrefsから取得しました</color> => key名: {key}");

            var json = PlayerPrefs.GetString(key);
            return JsonUtility.FromJson<T>(json);
        }

        public static void Set<T>(T data) where T : new()
        {
            Debug.Log($"<color=yellow>データをPlayerPrefsに保存しました</color> => data名: {data}");

            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(Key<T>(), json);
        }

        public static void Delete<T>() where T : new()
        {
            Debug.Log($"<color=yellow>データをPlayerPrefsから削除しました</color> => class名: {typeof(T)}");

            PlayerPrefs.DeleteKey(Key<T>());
        }

        private static string Key<T>() where T : new()
        {
            return typeof(T).Name;
        }
    }
}
