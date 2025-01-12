#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEngine;
using MessagePack;
using MessagePack.Resolvers;

namespace Siasm
{
    public static class MasterDataBinaryGenerator
    {
        private const string masterDataPath = "Assets/Siasm/MasterData/Binary/master_data.bytes";

        [MenuItem("Siasm/MasterData/MasterDataBinaryGenerator", priority = 201)]
        private static void Execute()
        {
            Debug.Log("MasterDataBinaryGeneratorを実行中...");

            // MessagePackの初期化
            var messagePackResolvers = CompositeResolver.Create(
                MasterMemoryResolver.Instance,
                GeneratedResolver.Instance,
                StandardResolver.Instance
            );

            var options = MessagePackSerializerOptions.Standard.WithResolver(messagePackResolvers);
            MessagePackSerializer.DefaultOptions = options;

            // DatabaseBuilderを使ってバイナリデータを生成
            var databaseBuilder = new DatabaseBuilder();

            // Jsonデータからモデルクラスに変換したもので登録
            // BattleCard
            var battleCardMasterDataArray = BattleCardMasterDataConverter.GetBattleCardMasterDataArray();
            databaseBuilder.Append(battleCardMasterDataArray);

            // BattleStage
            // InventoryItem
            // InventoryPassiveAbility
            // MainQuest
            
            // BattleFighter
            var battleFighterMasterDatas = BattleFighterMasterDataConverter.GetBattleFighterMasterDataArray();
            databaseBuilder.Append(battleFighterMasterDatas);

            // ビルドを実行
            var databaseBinary = databaseBuilder.Build();

            // ビルドしたデータベースをファイルに保存
            var directory = Path.GetDirectoryName(masterDataPath);

            // ファイル是非を確認して必要であれば作成する
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // 指定したパスのアセットに内容を変更する
            File.WriteAllBytes(masterDataPath, databaseBinary);

            // Unityに反映する
            AssetDatabase.Refresh();

            Debug.Log("MasterDataBinaryGeneratorの実行を完了しました");
        }
    }
}

#endif
