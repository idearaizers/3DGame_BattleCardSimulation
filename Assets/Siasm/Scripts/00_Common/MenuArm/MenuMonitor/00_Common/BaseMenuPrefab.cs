namespace Siasm
{
    public abstract class BaseMenuPrefab : BaseView
    {
        public abstract class BaseMenuPrefabParameter { }

        protected SideArmSwitcherPrefab SideArmSwitcherPrefab;
        protected BaseUseCase BaseUseCase;
        protected BaseCameraController BaseCameraController;
        protected PlayerBattleFighterSpawnController PlayerBattleFighterSpawnController;
        protected EnemyBattleFighterSpawnController EnemyBattleFighterSpawnController;

        /// <summary>
        /// シーンによって表示物を変えたい場合や機能として解放されるまで使用できないようにしたい際に使用
        /// </summary>
        protected bool IsEnable;

        public virtual void Initialize(SideArmSwitcherPrefab sideArmSwitcherPrefab, BaseUseCase baseUseCase, BaseCameraController baseCameraController,
            PlayerBattleFighterSpawnController playerBattleFighterSpawnController, EnemyBattleFighterSpawnController enemyBattleFighterSpawnController)
        {
            SideArmSwitcherPrefab = sideArmSwitcherPrefab;
            BaseUseCase = baseUseCase;
            BaseCameraController = baseCameraController;
            PlayerBattleFighterSpawnController = playerBattleFighterSpawnController;
            EnemyBattleFighterSpawnController = enemyBattleFighterSpawnController;
        }

        public virtual void Setup(bool isEnable)
        {
            IsEnable = isEnable;
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
