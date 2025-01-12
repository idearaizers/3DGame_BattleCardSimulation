using VContainer.Unity;
using UnityEngine;

namespace Siasm
{
    public class BasePagePresenter : IInitializable
    {
        public virtual void Initialize()
        {
            Debug.Log($"{this} => Initialize");
        }
    }
}
