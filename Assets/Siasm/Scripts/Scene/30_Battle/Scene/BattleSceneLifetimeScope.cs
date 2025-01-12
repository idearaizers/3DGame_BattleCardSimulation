using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Siasm
{
    public class BattleSceneLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private BattleCore battleCore;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(battleCore);
            builder.Register<BattleUseCase>(Lifetime.Singleton);
            builder.Register<BattleRepository>(Lifetime.Singleton);
            builder.RegisterEntryPoint<BattleScenePresenter>();
        }
    }
}
