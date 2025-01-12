using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Siasm
{
    public class PlayerFieldContact : MonoBehaviour
    {
        [SerializeField]
        private PlayerFieldContactActionView playerFieldContactActionView;

        private PlayerFieldCharacter playerFieldCharacter;
        private InputAction inputActionOfFire;

        private List<FieldInteract> fieldInteracts = new List<FieldInteract>();

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="playerFieldCharacter"></param>
        /// <param name="mainCamera">調べる表示で使用</param>
        public void Initialize(PlayerFieldCharacter playerFieldCharacter)
        {
            this.playerFieldCharacter = playerFieldCharacter;

            playerFieldContactActionView.Initialize();
            playerFieldContactActionView.Disable();

            var mainScenePlayerInputAction = new MainScenePlayerInputAction();
            mainScenePlayerInputAction.Enable();
            inputActionOfFire = mainScenePlayerInputAction.FindAction("Fire");
        }

        public void Setup() { }

        /// <summary>
        /// 先頭にあるfieldInteractsを消す
        /// 中身が空であれば非表示にする
        /// </summary>
        public void RemoveCurrentFieldInteract()
        {
            // 先頭を消す
            fieldInteracts.RemoveAt(0);

            // 中身がなければ非表示にする
            if (fieldInteracts.Count == 0)
            {
                playerFieldContactActionView.Disable();
            }
        }

        public void HandleUpdate()
        {
            var isFire = inputActionOfFire.WasPressedThisFrame();
            if (isFire)
            {
                if (fieldInteracts.Count == 0)
                {
                    return;
                }

                fieldInteracts[0].Interact(playerFieldCharacter);
            }
        }

        public void ChangeActiveOfPlayerFieldContactActionView(bool isActive)
        {
            if (isActive)
            {
                // 中身がなければ処理を終了する
                if (fieldInteracts.Count == 0)
                {
                    return;
                }

                playerFieldContactActionView.Enable();
            }
            else
            {
                playerFieldContactActionView.Disable();
            }
        }

        /// <summary>
        /// NOTE: インターフェースに変えた方がいいかも
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter(Collider collision)
        {
            var fieldEntry = collision.GetComponent<FieldEntry>();
            if (fieldEntry != null)
            {
                fieldEntry.Entry();
            }

            var fieldContact = collision.GetComponent<FieldInteract>();
            if (fieldContact != null)
            {
                fieldInteracts.Add(fieldContact);
                playerFieldContactActionView.Show(fieldContact.FieldInteractType);
            }
        }

        /// <summary>
        /// NOTE: インターフェースに変えた方がいいかも
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerExit(Collider collision)
        {
            var fieldContact = collision.GetComponent<FieldInteract>();
            if (fieldContact != null)
            {
                fieldInteracts.Remove(fieldContact);
                if (fieldInteracts.Count != 0)
                {
                    return;
                }

                playerFieldContactActionView.Disable();
            }
        }
    }
}
