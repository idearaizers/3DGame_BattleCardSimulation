using System;

namespace Siasm
{
    public abstract class BaseMenuDialogPrefab : BaseView
    {
        public abstract class BaseParameter { }

        protected SideArmSwitcherPrefab SideArmSwitcherPrefab;
        protected BaseUseCase BaseUseCase;
        protected BaseCameraController BaseCameraController;

        public Action OnCloseAction { get; set; }

        public virtual void Initialize(SideArmSwitcherPrefab sideArmSwitcherPrefab, BaseUseCase baseUseCase, BaseCameraController baseCameraController)
        {
            SideArmSwitcherPrefab = sideArmSwitcherPrefab;
            BaseUseCase = baseUseCase;
            BaseCameraController = baseCameraController;
        }

        public virtual void Setup() { }
    }
}
