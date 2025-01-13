using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// TODO: アイテムで解放に変更予定
    /// </summary>
    public sealed class EncyclopediaMenuPrefab : BaseMenuPrefab
    {
        [SerializeField]
        private CreatreAnalyzeView creatreAnalyzeView;

        [SerializeField]
        private AdmissionRecordView admissionRecordView;

        public override void Initialize(SideArmSwitcherPrefab sideArmSwitcherPrefab, BaseUseCase baseUseCase, BaseCameraController baseCameraController,
            PlayerBattleFighterSpawnController playerBattleFighterSpawnController, EnemyBattleFighterSpawnController enemyBattleFighterSpawnController)
        {
            base.Initialize(sideArmSwitcherPrefab, baseUseCase, baseCameraController, playerBattleFighterSpawnController, enemyBattleFighterSpawnController);

            creatreAnalyzeView.Initialize(baseUseCase);
            creatreAnalyzeView.OnClickAction = OnClickOfAnalyze;

            admissionRecordView.Initialize();
            admissionRecordView.OnClickAction = OnClick;

            admissionRecordView.Enable();
        }

        public override void Setup(bool isEnable)
        {
            base.Setup(isEnable);

            if (!isEnable)
            {
                return;
            }

            var currentIndex = 0;

            if (BaseUseCase == null)
            {
                return;
            }

            var creatureRecordModels = BaseUseCase.CreatureRecordModels(currentIndex);

            if (creatureRecordModels == null)
            {
                return;
            }

            this.gameObject.SetActive(true);
            creatreAnalyzeView.Setup(creatureRecordModels, currentIndex);
            admissionRecordView.Setup(creatureRecordModels, currentIndex);
            this.gameObject.SetActive(false);
        }

        public override void UpdateContent(BaseMenuPrefabParameter baseMenuPrefabParameter)
        {
            base.UpdateContent(baseMenuPrefabParameter);
        }

        private void OnClickOfAnalyze(int selectedIndex)
        {
            admissionRecordView.ShowSelected(selectedIndex);
        }

        private void OnClick(CreatureRecordModel creatureRecordModel)
        {
            creatreAnalyzeView.Show(creatureRecordModel);
        }
    }
}
