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

            // // テーブルからデータを検索してテスト用のログを表示
            // var stage = memoryDatabase.StageMasterDataTable.FindById("stage-01-002");
            // Debug.Log(stage.Name); // 迷いの湿地帯

            // // テスト
            // var characterMasterDataTable = memoryDatabase.CharacterMasterDataTable.FindById(2);
            // Debug.Log(characterMasterDataTable.Name); // フシギダネ2

            // テスト
            var aaa = memoryDatabase.BattleFighterMasterDataTable.FindById(2001);
            Debug.Log(aaa.ProductName);
            Debug.Log(aaa.DeckMasterDataArray);
            Debug.Log(aaa.DeckMasterDataArray[0].AchievementLevel);
            Debug.Log(aaa.DeckMasterDataArray[1].AchievementLevel);

            // 
            var bbb = memoryDatabase.BattleFighterMasterDataTable.FindById(2002);
            Debug.Log(bbb.ProductName);
            Debug.Log(bbb.DeckMasterDataArray);
            Debug.Log(bbb.DeckMasterDataArray[0].AchievementLevel);
            Debug.Log(bbb.DeckMasterDataArray[1].AchievementLevel);

        }
    }
}

#endif
