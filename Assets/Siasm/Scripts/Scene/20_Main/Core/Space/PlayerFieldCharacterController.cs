using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// プレイヤーは操作周りの処理があり共通化できないのでBaseFieldCharacterControllerは継承しないで管理
    /// </summary>
    public class PlayerFieldCharacterController : MonoBehaviour
    {
        [SerializeField]
        private PlayerFieldCharacter playerFieldCharacterPrefab;

        private PlayerFieldCharacter playerFieldCharacter;

        public Transform PlayerFieldCharacterTransform => playerFieldCharacter.transform;

        public void Initialize(Camera mainCamera)
        {
            playerFieldCharacter = Instantiate(playerFieldCharacterPrefab, transform) as PlayerFieldCharacter;
            playerFieldCharacter.Initialize(mainCamera);
        }

        /// <summary>
        /// モデルクラスの作成は省略して必要な値をここで生成
        /// NOTE: 見直した方がいいかも
        /// </summary>
        public void Setup(Vector3 spawnWorldPosition)
        {
            var faceDirection = -1;
            playerFieldCharacter.Setup(faceDirection, spawnWorldPosition);
        }

        public void StopMove()
        {
            playerFieldCharacter.StopMove();
        }

        public void ChangeActiveOfPlayerFieldContact(bool isActive)
        {
            playerFieldCharacter.PlayerFieldContact.ChangeActiveOfPlayerFieldContactActionView(isActive);
        }

        public void HandleUpdate()
        {
            playerFieldCharacter.HandleUpdate();
        }

        public void HandleFixedUpdate()
        {
            playerFieldCharacter.HandleFixedUpdate();
        }

        public void OnDesory()
        {
            Destroy(playerFieldCharacter.gameObject);
            playerFieldCharacter = null;
        }
    }
}
