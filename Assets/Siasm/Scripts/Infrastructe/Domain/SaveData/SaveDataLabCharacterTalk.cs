using System.Collections.Generic;

namespace Siasm
{
    [System.Serializable]
    public class SaveDataLabCharacter
    {
        public int CharacterId;
        public int TalkIndex;
    }

    [System.Serializable]
    public class SaveDataLabCharacterTalk
    {
        public List<SaveDataLabCharacter> SaveDataLabCharacters;
    }
}
