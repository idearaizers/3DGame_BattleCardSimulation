using System.Linq;
using UnityEngine;

namespace Siasm
{
    public class LabFieldCharacterModel : BaseFieldCharacterModel
    {
        public LabCharacterType LabCharacterType{ get; set; }
    }

    public class LabFieldCharacterModelFactory
    {
        public LabFieldCharacterModel[] CreateLabFieldCharacterModels(SaveDataCache saveDataCache)
        {
            var labFieldCharacterMasterData = new LabFieldCharacterMasterData();
            var labFieldCharacterMasterDataModels = labFieldCharacterMasterData.GetLabFieldCharacterMasterDataModels();

            var labFieldCharacterModels = labFieldCharacterMasterDataModels.Select(model => new LabFieldCharacterModel
            {
                CharacterId = model.CharacterId,
                LabCharacterType = model.LabCharacterType,
                Position = model.Position,
                FaceDirection = model.FaceDirection,
            });

            return labFieldCharacterModels.ToArray();
        }
    }
}
