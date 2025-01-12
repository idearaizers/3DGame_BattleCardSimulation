using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Siasm
{
    public class CreatureDropItemController : MonoBehaviour
    {
        [SerializeField]
        private FieldItemPickUpOfFieldInteract fieldItemPickUpOfFieldInteract;

        private MainUseCase mainUseCase;
        private CreatureBoxController creatureBoxController;
        private FieldItemPickUpController fieldItemPickUpController;

        public void Initialize(MainUseCase mainUseCase, CreatureBoxController creatureBoxController, FieldItemPickUpController fieldItemPickUpController)
        {
            this.mainUseCase = mainUseCase;
            this.creatureBoxController = creatureBoxController;
            this.fieldItemPickUpController = fieldItemPickUpController;
        }

        public void Setup()
        {
            // 
        }

        public void DropItem(int creatureId, int creatureLevel)
        {
            Debug.Log($"TODO: アイテムドロップ処理 {creatureId} {creatureLevel}");

            // 撃破状態に変える
            mainUseCase.DestroyedEnemyOfItemDrop(creatureId);

            // TODO: エフェクトを出して見た目を撃破状態に変える

            // 抽選を行ってドロップさせるアイテムを表示する
            var creatureAdmissionOfDestroyedMasterData = new EnemyBattleFighterOfDropItemMasterData();
            var creatureAdmissionOfDestroyedModel = creatureAdmissionOfDestroyedMasterData.GetEnemyAdmissionOfDestroyedModel(creatureId, creatureLevel);
            var dropItemId = creatureAdmissionOfDestroyedModel.CreatureAdmissionOfDestroyedLevelModels[0].DropItemId;

            // アイテムモデルに変換して落とすかな
            // TODO: アイテムをドロップさせる

            // 仮
            var fieldItemPickUpOfFieldInteractGameObject = Instantiate(fieldItemPickUpOfFieldInteract, this.transform);

            // 座標を取得する
            // 仮で0番目のものを取得
            var position = creatureBoxController.GetSpawnPosition(creatureId);

            // ワールドで配置
            // 仮でoffset
            position.y += 0.5f;
            position.z -= 3.0f;

            fieldItemPickUpOfFieldInteractGameObject.transform.position = position;

            // 初期化
            fieldItemPickUpOfFieldInteractGameObject.Initialize();
            fieldItemPickUpOfFieldInteractGameObject.OnPickUpAction = OnPickUp;
        }

        /// <summary>
        /// FieldItemPickUpController に処理をまとめてもいいかも
        /// </summary>
        /// <param name="fieldItemPickUpOfFieldInteract"></param>
        /// <param name="itemId"></param>
        /// <param name="itemNumber"></param>
        /// <param name="playerFieldCharacter"></param>
        private void OnPickUp(FieldItemPickUpOfFieldInteract fieldItemPickUpOfFieldInteract, int itemId, int itemNumber, PlayerFieldCharacter playerFieldCharacter)
        {
            // 拾ったアイテムを削除
            Destroy(fieldItemPickUpOfFieldInteract.gameObject);
            fieldItemPickUpOfFieldInteract = null;

            // 拾った表示をする
            fieldItemPickUpController.PickUpItem(fieldItemPickUpOfFieldInteract, itemId, itemNumber, playerFieldCharacter, true);
        }
    }
}
