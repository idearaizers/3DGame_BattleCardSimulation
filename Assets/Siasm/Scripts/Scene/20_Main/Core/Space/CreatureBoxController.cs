using System;
using UnityEngine;
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
            if (creatureBoxModels == null)
            {
                Debug.Log("TODO: 仮でVector3.zeroを返す");
                return Vector3.zero;
            }

            var creatureBoxModel = creatureBoxModels.FirstOrDefault(x => x.CreatureId == creatureId);
            var position = SpawnPositions[creatureBoxModel.BoxIndex];
            return position;
        }

        private void OnSurvey(CreatureFieldCharacterModel creatureFieldCharacterModel)
        {
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
                    // NOTE: 処理なし
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
