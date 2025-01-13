namespace Siasm
{
    public enum FieldObjectType
    {
        None = 0,

        // オフィス関連
        OfficeTerminalOfDelivery = 1001,    // 納品用ターミナル
        OfficeTerminalOfCreatureBox = 1002, // 収容クリシェミナ選択ターミナル
        OfficeTerminalOfBackHome = 1003,    // 家に帰るターミナル

        // 電車関連
        TrainTerminal = 2001,           // 電車に乗る

        // ドア関連
        NonSecurityDoor = 3001,         // セキュリティのない普通のドア
        SecurityDoorOfLevel1 = 3002,    // セキュリティドア_レベル1
        SecurityDoorOfLevel2 = 3003,    // セキュリティドア_レベル2
        SecurityDoorOfLevel3 = 3004,    // セキュリティドア_レベル3
        SecurityDoorOfLevel4 = 3005,    // セキュリティドア_レベル4
        SecurityDoorOfLevel5 = 3006     // セキュリティドア_レベル5
    }
}
