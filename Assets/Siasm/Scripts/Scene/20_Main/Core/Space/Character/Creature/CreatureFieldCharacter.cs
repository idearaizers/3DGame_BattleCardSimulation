using System;
using UnityEngine;

namespace Siasm
{
    public sealed class CreatureFieldCharacter : BaseFieldCharacter
    {
        public Action<CreatureFieldCharacterModel> OnSurveyAction { get; set; }
        public Action OnTalkFinishAction { get; set; }

        public override void Setup(BaseFieldCharacterModel baseFieldCharacterModel)
        {
            base.Setup(baseFieldCharacterModel);

            // 仮
            CommonFieldCharacterContent.FieldCharacterAnimation.Setup(baseFieldCharacterModel.CharacterId);
        }

        public override void OnInteract(Transform targetPlayerTransform)
        {
            // 顔の向きを変える
            ChangeFaceDirection(targetPlayerTransform);

            // 調査か会話を行う
            var creatureFieldCharacterModel = BaseFieldCharacterModel as CreatureFieldCharacterModel;
            if (creatureFieldCharacterModel.IsCreatureBox)
            {
                OnSurveyAction?.Invoke(creatureFieldCharacterModel);
            }
            else
            {
                PlayTalk(creatureFieldCharacterModel.CharacterId, OnTalkFinishAction);
            }
        }
    }
}
