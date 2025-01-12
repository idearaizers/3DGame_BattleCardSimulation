using UnityEngine;

namespace Siasm
{
    public class LabCharacterModel : BaseFieldCharacter
    {
        private LabCharacterType labCharacterType;

        // [SerializeField]
        // private LabCharacterFieldContact labCharacterSubmitContact;

        // public LabCharacterFieldContact LabCharacterSubmitContact => labCharacterSubmitContact;

        public void Initialize(LabCharacterType labCharacterType)
        {
            // this.labCharacterType = labCharacterType;
            // labCharacterSubmitContact.Initialize(this);
        }

        // public override TalkParameter GetTalkParameter()
        // {
        //     return LabCharacteTalkConstant.TalkParameterDictionary[labCharacterType];
        // }
    }
}
