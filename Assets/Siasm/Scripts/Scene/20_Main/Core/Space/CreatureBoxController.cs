using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Linq;

namespace Siasm
{
    public class CreatureBoxController : BaseFieldCharacterController
    {
        private readonly Vector3[] SpawnPositions = new Vector3[]
        {
            new Vector3( -3.5f, 30.85f, 139.3f),
            new Vector3( 15.3f, 30.85f, 139.3f),
            new Vector3( 33.8f, 30.85f, 139.3f),
            new Vector3(-21.0f, 30.85f, 139.3f),
            new Vector3(-40.3f, 30.85f, 139.3f)
        };

        public Action<int, int> OnStartBattleAction { get; set; }

        private CreatureBoxModel[] creatureBoxModels;

        public void Setup(CreatureBoxModel[] creatureBoxModels)
        {
            this.creatureBoxModels = creatureBoxModels;

            base.Setup();

            // モデルクラスを基に生成
            // NOTE: 一旦、第一階層の数だけ設定
            foreach (var creatureBoxModel in creatureBoxModels)
            {
                // 生成と格納
                var baseFieldCharacter = Instantiate(BaseFieldCharacterPrefab, transform);
                BaseFieldCharacters.Add(baseFieldCharacter);

                // 仮でここでモデルクラスを生成
                var creatureFieldCharacterModel = new CreatureFieldCharacterModel
                {
                    CharacterId = creatureBoxModel.CreatureId,
                    Position = Vector3.zero,
                    FaceDirection = 1,
                    CharacterLevel = creatureBoxModel.CreatureLevel,
                    IsCreatureBox = true
                };

                // 撃破状態であれば見た目を変える
                if (creatureBoxModel.IsDestroyed)
                {
                    Debug.Log("TODO: 撃破状態であれば見た目を変える");
                }

                // 初期化
                var creatureFieldCharacter = baseFieldCharacter as CreatureFieldCharacter;
                creatureFieldCharacter.Initialize(MainTalkController, MainQuestController, MainCamera);
                creatureFieldCharacter.Setup(creatureFieldCharacterModel);
                creatureFieldCharacter.OnSurveyAction = OnSurvey;
            }

            for (int i = 0; i < creatureBoxModels.Length; i++)
            {
                BaseFieldCharacters[i].transform.position = SpawnPositions[i];
            }
        }

        public Vector3 GetSpawnPosition(int creatureId)
        {
            // 仮処理
            if (creatureBoxModels == null)
            {
                Debug.Log("TODO: 仮でVector3.zeroを返す");
                return Vector3.zero;
            }

            var creatureBoxModel = creatureBoxModels.FirstOrDefault(x => x.CreatureId == creatureId);

            // 仮
            var position = SpawnPositions[creatureBoxModel.BoxIndex];
            return position;
        }

        private void OnSurvey(CreatureFieldCharacterModel creatureFieldCharacterModel)
        {
            // 一旦、仮で確認画面を出してから挑戦させるかな
            // MainUIContent YesNoSelectView

            // 取得した情報を基に再度必要な情報を代入する
            // 名前を設定
            // マスターデータから取得に変更
            // var enemyBattleFighterOfNameMasterData = new EnemyBattleFighterOfNameMasterData();
            // var fighterName = enemyBattleFighterOfNameMasterData.NameDictionary[creatureFieldCharacterModel.CharacterId];
            var fighterName = "???";

            var selectTitleText = $"{fighterName}(Lv.{creatureFieldCharacterModel.CharacterLevel})がこちらをニラんでいるようだ\n戦ってエギドを奪いますか？";

            var dialogParameterOfHome = new CreatureBoxMenuDialogPrefab.DialogParameter
            {
                TitleText = selectTitleText,
                OnYesAction = () =>
                {
                    // バトルを開始する
                    OnStartBattleAction?.Invoke(creatureFieldCharacterModel.CharacterId, creatureFieldCharacterModel.CharacterLevel);
                },
                OnNoAction = () =>
                {
                    // 
                },
                CreatureId = creatureFieldCharacterModel.CharacterId,
                CreatureLevel = creatureFieldCharacterModel.CharacterLevel
            };

            AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);
            MainUIManager.MenuArmController.PlayShowDialogMenuAnimation(DialogMenuType.CreatureBox, dialogParameterOfHome);
            MainStateMachineController.ChangeMainState(MainStateMachineController.MainState.InteractAction);
        }
    }
}
