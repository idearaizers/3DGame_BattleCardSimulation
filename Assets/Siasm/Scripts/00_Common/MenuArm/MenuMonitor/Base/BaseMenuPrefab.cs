namespace Siasm
{
    public abstract class BaseMenuPrefab : BaseView
    {
        public abstract class BaseMenuPrefabParameter { }

        protected SideArmSwitcherPrefab SideArmSwitcherPrefab;
        protected BaseUseCase BaseUseCase;
        protected BaseCameraController BaseCameraController;
        protected bool IsActive;

        public virtual void Initialize(SideArmSwitcherPrefab sideArmSwitcherPrefab, BaseUseCase baseUseCase, BaseCameraController baseCameraController,
            BattleSpaceManager battleSpaceManager)
        {
            SideArmSwitcherPrefab = sideArmSwitcherPrefab;
            BaseUseCase = baseUseCase;
            BaseCameraController = baseCameraController;

            // マネージャーだと無限参照ループになる可能性があるからその下のものがよさそうやね
            // battleSpaceManager あとで整理した方がよさそう
        }

        public virtual void Setup(bool isActive)
        {
            IsActive = isActive;
        }

        public virtual void ShowChangeContent()
        {
            gameObject.SetActive(true);
        }

        public virtual void HideChangeContent()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// メニューを閉じた状態から開く際に使用する
        /// </summary>
        /// <param name="baseMenuPrefabParameter"></param>
        public virtual void UpdateContent(BaseMenuPrefabParameter baseMenuPrefabParameter) { }
    }
}
