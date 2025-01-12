namespace Siasm
{
    /// <summary>
    /// 仮で実装
    /// </summary>
    public sealed class DummyMenuPrefab : BaseMenuPrefab
    {
        public override void Initialize(SideArmSwitcherPrefab sideArmSwitcherPrefab, BaseUseCase baseUseCase, BaseCameraController baseCameraController,
            PlayerBattleFighterSpawnController playerBattleFighterSpawnController, EnemyBattleFighterSpawnController enemyBattleFighterSpawnController)
        {
            base.Initialize(sideArmSwitcherPrefab, baseUseCase, baseCameraController, playerBattleFighterSpawnController, enemyBattleFighterSpawnController);
        }

        public override void Setup(bool isEnable)
        {
            base.Setup(isEnable);

            // 使用しない場合は実行しない
            if (!isEnable)
            {
                return;
            }
        }
    }
}
