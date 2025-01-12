using UnityEngine;

namespace Siasm
{
    public sealed class MainSceneMessage : BaseSceneMessage
    {
        public Vector3 SpawnWorldPosition { get; set; }

        // NOTE: 必要なら顔の向きも追加

        public int DestroyedCreatureId { get; set; }    // 撃破したid
        public int DestroyedCreatureLevel { get; set; }    // 撃破したid
    }
}
