using System.Collections.Generic;

namespace Siasm
{
    public enum CharacterNameType
    {
        None = 0,

        // ビジネサー関連
        Businessar1 = 100,

        // フルネス関連
        Fullness1 = 200,

        // モブ研究員関連
        Mob = 300,

        // クリシェミナ関連
        Cliche = 400
    }

    public class LabFieldCharacterOfNameMasterData
    {
        public Dictionary<CharacterNameType, string> CharacterNameDictionary = new Dictionary<CharacterNameType, string>()
        {
            { CharacterNameType.Businessar1, "ビジネサー ゼーナ" }  // NOTE: または代表にする
        };
    }
}
