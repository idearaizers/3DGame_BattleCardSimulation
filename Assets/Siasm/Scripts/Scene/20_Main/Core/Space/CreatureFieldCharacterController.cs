using System;

namespace Siasm
{
    public sealed class CreatureFieldCharacterController : BaseFieldCharacterController
    {
        public Action OnStartBattleAction { get; set; }

        public void Setup(CreatureFieldCharacterModel[] creatureFieldCharacterModels)
        {
            base.Setup();

            foreach (var creatureFieldCharacterModel in creatureFieldCharacterModels)
            {
                var baseFieldCharacter = Instantiate(BaseFieldCharacterPrefab, transform);
                BaseFieldCharacters.Add(baseFieldCharacter);

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
