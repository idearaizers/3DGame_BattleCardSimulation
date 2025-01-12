using UnityEngine;
using System.Threading;

namespace Siasm
{
    /// <summary>
    /// MonoBehaviourでなくても良いものの管理を行うクラスを管理するクラス
    /// </summary>
    public class MainLogicManager : MonoBehaviour
    {
        [SerializeField]
        private MainDirectorController mainDirectorController;

        [SerializeField]
        private MainPlayerInputController mainScenePlayerInputController;

        [SerializeField]
        private MainTalkController mainTalkController;

        public MainDirectorController DirectorController => mainDirectorController;
        public MainTalkController TalkController => mainTalkController;

        public void Initialize(CancellationToken token, MainUIManager mainUIManager,
            MainCameraController mainCameraController, MainStateMachineController mainStateMachineController, MainUseCase mainUseCase)
        {
            mainDirectorController.Initialize(token);
            mainScenePlayerInputController.Initialize(mainUIManager, mainCameraController, mainUseCase);
            mainTalkController.Initialize(mainUIManager, mainStateMachineController, mainUseCase);
        }

        public void Setup()
        {
            mainDirectorController.Setup();
            mainScenePlayerInputController.Setup();
            mainTalkController.Setup();
        }
    }
}
