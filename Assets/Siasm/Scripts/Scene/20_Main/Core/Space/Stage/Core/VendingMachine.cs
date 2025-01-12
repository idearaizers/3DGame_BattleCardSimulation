using UnityEngine;

namespace Siasm
{
    public class VendingMachine : MonoBehaviour
    {
        [SerializeField]
        private FieldInteract fieldInteract;

        public void Initialize()
        {
            // モデルデータを基に生成

            fieldInteract.Initialize();
            fieldInteract.OnInteractAction = OnContanct;


            // this.mainTalkController = mainTalkController;
            // this.mainQuestController = mainQuestController;

            // // 移動の初期化
            // baseFieldCharacterMovement.Initialize(faceDirection);

            // // アニメーションの初期化
            // baseFieldCharacterAnimation.Initialize();
            // baseFieldCharacterAnimation.ChangeFaceDirection(faceDirection);

            // // 交流の初期化
            // fieldInteract.Initialize();
            // fieldInteract.OnInteractAction = OnInteract;

            // // 把握しやすいようにGameObjectの名称をリネーム
            // gameObject.name = string.Format(renameGameObjectStringFormat, gameObject.name, characterId);

            // // 座標を変更
            // transform.position = position;

        }

        private void OnContanct(Transform targetPlayerTransform)
        {
            Debug.Log($"VendingMachineを調べた => {gameObject.name}");

            // ベンダー購入画面を表示する
            // プレイヤーを移動できないようにする。ステートの変更
            // 完了時にステートを戻す。共通処理に登録するかな
        }


        // protected override void OnInteract(PlayerFieldCharacter playerFieldCharacter)
        // {
        //     base.OnInteract(playerFieldCharacter);

        //     PlayTalk(labFieldCharacterModel.CharacterId);
        // }


    }
}
