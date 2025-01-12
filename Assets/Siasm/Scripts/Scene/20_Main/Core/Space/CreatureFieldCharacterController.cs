using System;

namespace Siasm
{
    /// <summary>
    /// クリシェミナの生成と管理を行う
    /// ここではフィールドに配置するものだけで、Box管理は別で行う
    /// </summary>
    public sealed class CreatureFieldCharacterController : BaseFieldCharacterController
    {
        public Action OnStartBattleAction { get; set; }

        public void Setup(CreatureFieldCharacterModel[] creatureFieldCharacterModels)
        {
            base.Setup();

            // モデルクラスを基に生成
            foreach (var creatureFieldCharacterModel in creatureFieldCharacterModels)
            {
                // 生成と格納
                var baseFieldCharacter = Instantiate(BaseFieldCharacterPrefab, transform);
                BaseFieldCharacters.Add(baseFieldCharacter);

                // 初期化
                var creatureFieldCharacter = baseFieldCharacter as CreatureFieldCharacter;
                creatureFieldCharacter.Initialize(MainTalkController, MainQuestController, MainCamera);
                creatureFieldCharacter.Setup(creatureFieldCharacterModel);
                creatureFieldCharacter.OnTalkFinishAction = OnTalkFinish;
            }
        }

        private void OnTalkFinish()
        {
            OnStartBattleAction?.Invoke();
        }
    }
}
