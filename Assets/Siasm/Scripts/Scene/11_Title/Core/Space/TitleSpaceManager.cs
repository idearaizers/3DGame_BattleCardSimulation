using UnityEngine;

namespace Siasm
{
    public class TitleSpaceManager : MonoBehaviour
    {
        [SerializeField]
        private TitleCameraController titleCameraController;

        public TitleCameraController TitleCameraController => titleCameraController;

        public void Initialize()
        {
            titleCameraController.Initialize();
        }

        public void Setup()
        {
            titleCameraController.Setup();
        }
    }
}
