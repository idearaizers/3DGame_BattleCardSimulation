namespace Siasm
{
    /// <summary>
    /// 仮で実装
    /// </summary>
    public sealed class DummyMenuPrefab : BaseMenuPrefab
    {
        public override void Initialize(SideArmSwitcherPrefab sideArmSwitcherPrefab, BaseUseCase baseUseCase, BaseCameraController baseCameraController, BattleSpaceManager battleSpaceManager)
        {
            base.Initialize(sideArmSwitcherPrefab, baseUseCase, baseCameraController, battleSpaceManager);
        }

        public override void Setup(bool isActive)
        {
            base.Setup(isActive);

            // 使用しない場合は実行しない
            if (!isActive)
            {
                return;
            }
        }
    }
}
