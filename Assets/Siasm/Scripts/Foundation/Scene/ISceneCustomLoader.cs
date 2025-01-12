using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Siasm
{
    public interface ISceneCustomLoader
    {
        UniTask LoadSceneAsync(AssetReference assetReference);
    }
}
