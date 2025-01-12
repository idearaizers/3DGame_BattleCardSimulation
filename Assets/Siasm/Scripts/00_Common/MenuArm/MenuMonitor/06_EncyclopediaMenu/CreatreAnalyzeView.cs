using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Siasm
{
    public class CreatreAnalyzeView : BaseView
    {
        [SerializeField]
        private Button previousDitialButton;

        [SerializeField]
        private TextMeshProUGUI createrNameText;

        [SerializeField]
        private Button nextDitialButton;

        [Space]
        [SerializeField]
        private TabContentSwitcher tabContentSwitcher;

        [Space]
        [SerializeField]
        private CreatreStatusView creatreStatusView;

        [SerializeField]
        private CreatreRecordView creatreRecordView;

        private CreatureRecordModel[] currentCreatureRecordModels;

        public int CurrentIndex { get; set; }

        public Action<int> OnClickAction { get; set; }

        public void Initialize(BaseUseCase BaseUseCase)
        {
            var activeTabIndex = 0;
            tabContentSwitcher.Initialize(activeTabIndex);
            creatreStatusView.Initialize(BaseUseCase);
            creatreRecordView.Initialize(BaseUseCase);

            previousDitialButton.onClick.AddListener(OnPreviousDitialButton);
            nextDitialButton.onClick.AddListener(OnNextDitialButton);
        }

        public void Setup(CreatureRecordModel[] creatureRecordModels, int currentIndex)
        {
            // 仮
            CurrentIndex = currentIndex;

            currentCreatureRecordModels = creatureRecordModels;

            // createrNameText

            tabContentSwitcher.Setup();
            creatreStatusView.Setup();

            // エラー回避のため有効にしてから実行する
            creatreRecordView.gameObject.SetActive(true);
            creatreRecordView.Setup();
            creatreRecordView.gameObject.SetActive(false);

            UpdateView();
        }

        public void Show(CreatureRecordModel creatureRecordModel)
        {
            var selectedIndex = Array.IndexOf(currentCreatureRecordModels, creatureRecordModel);
            CurrentIndex = selectedIndex;

            UpdateView();
        }

        private void OnPreviousDitialButton()
        {
            if (CurrentIndex <= 0)
            {
                CurrentIndex = currentCreatureRecordModels.Length - 1;
            }
            else
            {
                CurrentIndex--;
            }

            UpdateView();

            OnClickAction?.Invoke(CurrentIndex);
        }

        private void OnNextDitialButton()
        {
            if (CurrentIndex >= currentCreatureRecordModels.Length - 1)
            {
                CurrentIndex = 0;
            }
            else
            {
                CurrentIndex++;
            }

            UpdateView();

            OnClickAction?.Invoke(CurrentIndex);
        }

        private void UpdateView()
        {
            // エラー回避用
            if (currentCreatureRecordModels.Length == 0)
            {
                return;
            }

            // 
            var currentCreatureRecordModel = currentCreatureRecordModels[CurrentIndex];

            // マスターデータからの取得に変更
            // var enemyBattleFighterOfNameMasterData = new EnemyBattleFighterOfNameMasterData();
            // var creatureName = enemyBattleFighterOfNameMasterData.NameDictionary[currentCreatureRecordModel.CreatureId];
            var creatureName = "???";

            // 
            createrNameText.text = $"{creatureName}(Lv.{currentCreatureRecordModel.CreatureLevel})";

            // 
            creatreStatusView.UpdateView(currentCreatureRecordModel);
            creatreRecordView.UpdateView(currentCreatureRecordModel, 0);    // 仮で0で更新


            // 
            // // 取得した情報を基に再度必要な情報を代入する
            // // 名前を設定
            // var enemyBattleFighterOfNameMasterData = new EnemyBattleFighterOfNameMasterData();
            // enemyBattleFighterModel.FighterName = enemyBattleFighterOfNameMasterData.NameDictionary[prepareEnemyBattleFighterModel.FighterId];

            // // レベルを設定
            // enemyBattleFighterModel.FighterLevel = prepareEnemyBattleFighterModel.FighterLevel;

            // // レベルによるパラメータを設定
            // var enemyBattleFighterOfLevelParameterMasterData = new EnemyBattleFighterOfLevelParameterMasterData();
            // var parameterDictionary = enemyBattleFighterOfLevelParameterMasterData.ParameterDictionary[prepareEnemyBattleFighterModel.FighterLevel];

            // // HPを設定
            // enemyBattleFighterModel.HealthModel = new HealthModel
            // {
            //     MaxPoint = parameterDictionary.Item1,
            //     CurrentPoint = parameterDictionary.Item1
            // };

            // // 思考力を設定
            // enemyBattleFighterModel.ThinkingModel = new ThinkingModel
            // {
            //     MaxPoint = parameterDictionary.Item2,
            //     CurrentPoint = parameterDictionary.Item2,
            //     ElapsedTurn = 0
            // };

            // // 初期のバトルボックス数を設定
            // if (prepareEnemyBattleFighterModel.FighterLevel >= 40)
            //     enemyBattleFighterModel.CurrentBattleBoxNumber = 5;
            // else if (prepareEnemyBattleFighterModel.FighterLevel >= 30)
            //     enemyBattleFighterModel.CurrentBattleBoxNumber = 4;
            // else if (prepareEnemyBattleFighterModel.FighterLevel >= 20)
            //     enemyBattleFighterModel.CurrentBattleBoxNumber = 3;
            // else if (prepareEnemyBattleFighterModel.FighterLevel >= 10)
            //     enemyBattleFighterModel.CurrentBattleBoxNumber = 2;
            // else
            //     enemyBattleFighterModel.CurrentBattleBoxNumber = 1;

            // // 最大バトルボックス数を設定
            // enemyBattleFighterModel.MaxBattleBoxNumber = enemyBattleFighterModel.CurrentBattleBoxNumber + 4;

        }
    }
}
