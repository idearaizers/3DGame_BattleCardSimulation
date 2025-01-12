using UnityEngine;

namespace Siasm
{
    public class SceneStackMessage : MonoBehaviour
    {
        public BaseSceneMessage CurrentBaseSceneMessage { get; private set; }

        public void Initialize() { }

        public void Setup() { }

        public void SetSceneMessage(BaseSceneMessage sceneMessage)
        {
            CurrentBaseSceneMessage = sceneMessage;
        }
    }
}
