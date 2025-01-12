using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// これって変更だよね？？
    /// あとはマスターデータの取得かな
    /// 基本的にはUsecaseで使用かな
    /// ファクトトリーくらすはここにかくみたいモデルデータの
    /// セーブデータへの反映はここでするかな
    /// </summary>
    public sealed class BattleRepository : BaseRepository
    {
        // ここにはInjectではなくコンストラクタでもいいみたい？
        public BattleRepository()
        {
            // 
        }

        /// <summary>
        /// マスターデータからバトルデータを取得してModelデータの作成を行う
        /// </summary>
        /// <param name="enemyBattleFighterId"></param>
        /// <returns></returns>
        // public BattleSceneModel Find(int enemyBattleFighterId)
        // {
        //     return new BattleSceneModel
        //     {
        //         // id
        //         // master
        //         // battleWave
        //     };
        // }
    }
}
