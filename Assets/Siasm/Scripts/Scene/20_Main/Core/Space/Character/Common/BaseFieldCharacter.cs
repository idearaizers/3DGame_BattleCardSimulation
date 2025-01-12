using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// PlayerFieldCharacter 以外が継承している
    /// </summary>
    public abstract class BaseFieldCharacter : MonoBehaviour
    {
        private string renameGameObjectStringFormat = "{0} / {1}";

        [SerializeField]
        private FieldCharacterMovement fieldCharacterMovement;

        [SerializeField]
        private CommonFieldCharacterContent commonFieldCharacterContent;

        private MainTalkController mainTalkController;
        private MainQuestController mainQuestController;

        public BaseFieldCharacterModel BaseFieldCharacterModel { get; private set; }

        public CommonFieldCharacterContent CommonFieldCharacterContent => commonFieldCharacterContent;

        public virtual void Initialize(MainTalkController mainTalkController, MainQuestController mainQuestController, Camera mainCamera)
        {
            this.mainTalkController = mainTalkController;
            this.mainQuestController = mainQuestController;

            // 初期化
            fieldCharacterMovement.Initialize();
            commonFieldCharacterContent.Initialize(mainCamera);
            commonFieldCharacterContent.OnInteractAction = OnInteract;
        }

        public virtual void Setup(BaseFieldCharacterModel baseFieldCharacterModel)
        {
            this.BaseFieldCharacterModel = baseFieldCharacterModel;

            fieldCharacterMovement.Setup();
            // commonFieldCharacterContent.Setup(baseFieldCharacterModel.CharacterId);
            commonFieldCharacterContent.Setup();

            // 座標を変更
            transform.position = baseFieldCharacterModel.Position;

            // 把握しやすいようにGameObjectの名称をリネーム
            gameObject.name = string.Format(renameGameObjectStringFormat, gameObject.name, baseFieldCharacterModel.CharacterId);
        }

        public virtual void OnInteract(Transform targetPlayerTransform)
        {
            // 顔の向きを変える
            ChangeFaceDirection(targetPlayerTransform);

            // 会話を行う
            PlayTalk(BaseFieldCharacterModel.CharacterId);
        }

        /// <summary>
        /// プレイヤーとx座標を比較して顔の向きを変える
        /// </summary>
        /// <param name="playerFieldCharacter"></param>
        public void ChangeFaceDirection(Transform targetPlayerTransform)
        {
            var faceDirection = GetFaceDirection(targetPlayerTransform);

            // NOTE: 見直し予定
            fieldCharacterMovement.SetFaceDirection(faceDirection);
            commonFieldCharacterContent.FieldCharacterAnimation.ChangeFaceDirection(faceDirection);
        }

        private float GetFaceDirection(Transform targetPlayerTransform)
        {
            var targetPositonX = targetPlayerTransform.position.x; 
            var sourcePositionX = this.transform.position.x; 
            var faceDirection = Mathf.Sign(targetPositonX - sourcePositionX);
            return faceDirection;
        }

        public void PlayTalk(int characterId, Action onTalkFinish = null)
        {
            // 会話関係のクエストを進行中か確認する
            mainQuestController.IsProgressOfTalk(characterId);

            // 会話を開始
            mainTalkController.PlayTalkAsync(characterId, onTalkFinish).Forget();
        }
    }
}
