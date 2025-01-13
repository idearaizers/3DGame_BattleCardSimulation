using UnityEngine;

namespace Siasm
{
    public sealed class MainSceneMessage : BaseSceneMessage
    {
        public Vector3 SpawnWorldPosition { get; set; }
        public int DestroyedCreatureId { get; set; }
        public int DestroyedCreatureLevel { get; set; }
    }
}
