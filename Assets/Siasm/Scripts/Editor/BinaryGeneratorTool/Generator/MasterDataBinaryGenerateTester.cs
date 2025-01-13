#if UNITY_EDITOR

using MessagePack;
using MessagePack.Resolvers;
using UnityEditor;
using UnityEngine;

namespace Siasm
{
    public static class MasterDataBinaryGenerateTester
    {
        [MenuItem("Siasm/MasterData/MasterDataBinaryGenerateTester", priority = 202)]
        private static void Execute()
        {
            // MessagePackの初期化
            var messagePackResolvers = CompositeResolver.Create(
                MasterMemoryResolver.Instance,
                GeneratedResolver.Instance,
                StandardResolver.Instance
            );

            var options = MessagePackSerializerOptions.Standard.WithResolver(messagePackResolvers);
            MessagePackSerializer.DefaultOptions = options;

            // マスターデータのバイナリを読み込み
            var path = "Assets/Siasm/MasterData/Binary/master_data.bytes";
            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

            // MemoryDatabaseをバイナリから作成
            var memoryDatabase = new MemoryDatabase(asset.bytes);

            // 変換が問題ないか確認のためテストで適当な項目をログ表示
            var index2001 = memoryDatabase.BattleFighterMasterDataTable.FindById(2001);
            Debug.Log(index2001.ProductName);
            Debug.Log(index2001.DeckMasterDataArray);
            Debug.Log(index2001.DeckMasterDataArray[0].AchievementLevel);
            Debug.Log(index2001.DeckMasterDataArray[1].AchievementLevel);

            // 変換が問題ないか確認のためテストで適当な項目をログ表示
            var index2002 = memoryDatabase.BattleFighterMasterDataTable.FindById(2002);
            Debug.Log(index2002.ProductName);
            Debug.Log(index2002.DeckMasterDataArray);
            Debug.Log(index2002.DeckMasterDataArray[0].AchievementLevel);
            Debug.Log(index2002.DeckMasterDataArray[1].AchievementLevel);
        }
    }
}

#endif
