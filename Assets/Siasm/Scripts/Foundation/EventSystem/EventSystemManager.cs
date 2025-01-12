using UnityEngine;
using UnityEngine.EventSystems;

namespace Siasm
{
    public class EventSystemManager : SingletonMonoBehaviour<EventSystemManager>
    {
        [SerializeField]
        private EventSystem eventSystem;

        public EventSystem EventSystem => eventSystem;

        public void Initialize() { }
    }
}
