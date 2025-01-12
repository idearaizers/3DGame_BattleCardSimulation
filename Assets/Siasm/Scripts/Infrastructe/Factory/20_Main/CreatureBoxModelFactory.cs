using System.Linq;

namespace Siasm
{
    /// <summary>
    /// NOTE: 見直し予定
    /// NOTE: CreatureFieldCharacterModel があると管理がしやすいかも
    /// </summary>
    public class CreatureBoxModel
    {
        public int StageIndex { get; set; }     // 収容する場所
        public int BoxIndex { get; set; }       // 収容しているボックスの位置
        public int CreatureId { get; set; }
        public int CreatureLevel { get; set; }
        public bool IsDestroyed { get; set; }    // 撃破済みかどうか。日にち増加でリセットさせる
    }

    public class CreatureBoxModelFactory
    {
        public CreatureBoxModel[] CreateCreatureBoxModels(SaveDataCache saveDataCache)
        {
            var saveDataCreatureBoxs = saveDataCache.SaveDataCreatureBoxs;
            var CreatureBoxModels = saveDataCreatureBoxs.Select(x => new CreatureBoxModel
            {
                StageIndex = x.StageIndex,
                BoxIndex = x.BoxIndex,
                CreatureId = x.CreatureId,
                CreatureLevel = x.CreatureLevel,
                IsDestroyed = x.IsDestroyed
            });

            return CreatureBoxModels.ToArray();
        }
    }
}
