using UnityEngine;

namespace Siasm
{
    public class AutoDontDestroyOnLoad : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
