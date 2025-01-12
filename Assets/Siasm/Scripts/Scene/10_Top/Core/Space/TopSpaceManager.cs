using UnityEngine;

namespace Siasm
{
    public class TopSpaceManager : MonoBehaviour
    {
        [SerializeField]
        private TopCameraController topCameraController;

        public TopCameraController TopCameraController => topCameraController;

        public void Initialize()
        {
            topCameraController.Initialize();
        }

        public void Setup()
        {
            topCameraController.Setup();
        }
    }
}
