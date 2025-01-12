using System.Linq;

namespace Siasm
{
    public class CreatureFieldCharacterModel : BaseFieldCharacterModel
    {
        public int CharacterLevel { get; set; }
        public bool IsCreatureBox { get; set; }     // Boxにいる場合はtrueにする

    }

    public class CreatureFieldCharacterModelFactory
    {
        public CreatureFieldCharacterModel[] CreateCreatureFieldCharacterModels(SaveDataCache saveDataCache)
        {
            var creatureFieldCharacterMasterData = new EnemyFieldCharacterMasterData();
            var creatureFieldCharacterMasterDataModels = creatureFieldCharacterMasterData.CreatureEnemyCharacterMasterDataModels();

            var creatureFieldCharacterModels = creatureFieldCharacterMasterDataModels.Select(model => new CreatureFieldCharacterModel
            {
                CharacterId = model.CharacterId,
                Position = model.Position,
                CharacterLevel = model.CharacterLevel,
                FaceDirection = model.FaceDirection,
                IsCreatureBox = false
            });

            return creatureFieldCharacterModels.ToArray();
        }
    }
}
