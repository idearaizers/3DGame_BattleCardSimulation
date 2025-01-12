using UnityEngine;
using UnityEngine.EventSystems;

namespace Siasm
{
    public class EventSystemSwitch : MonoBehaviour
    {
        private static EventSystem eventSystem;

        private void Start()
        {
            eventSystem = EventSystem.current;
        }

        private void OnDestroy()
        {
            eventSystem = null;
        }

        public static void Enable()
        {
            if (eventSystem == null)
            {
                return;
            }

            eventSystem.enabled = true;
        }

        public static void Disable()
        {
            if (eventSystem == null)
            {
                return;
            }

            eventSystem.enabled = false;
        }
    }
}
