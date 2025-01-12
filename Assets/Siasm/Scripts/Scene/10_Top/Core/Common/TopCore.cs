using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;

namespace Siasm
{
    /// <summary>
    /// Presenterで行っている役割ですが仲介しているCoreでも同様の処理を行っています
    /// </summary>
    public class TopCore : MonoBehaviour
    {
        [SerializeField]
        private TopSpaceManager topSpaceManager;

        [SerializeField]
        private TopUIManager topUIManager;

        [Space]
        [SerializeField]
        private AssetReference titleSceneAssetRefrence;

        public TopSpaceManager SpaceManager => topSpaceManager;
        public TopUIManager UIManager => topUIManager;

        public void Initialize()
        {
            topSpaceManager.Initialize();
            topUIManager.Initialize();
        }

        public void Setup()
        {
            topSpaceManager.Setup();
            topUIManager.Setup();
        }

        public void LoadTitleScene(ISceneCustomLoader sceneCustomLoader)
        {
            SceneLoadManager.Instance.LoadSceneAsync(titleSceneAssetRefrence, sceneCustomLoader).Forget();
        }
    }
}
