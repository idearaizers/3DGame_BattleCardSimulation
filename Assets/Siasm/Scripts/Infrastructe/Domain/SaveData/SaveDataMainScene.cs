using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// メインシーンを始めるのに必要な情報をまとめたクラス
    /// </summary>
    [System.Serializable]
    public class SaveDataMainScene
    {
        public string LastDateAndTime;          // DateTime型だと保存できないのでstringで格納
        public string StartDateAndTime;         // DateTime型だと保存できないのでstringで格納 NOTE: セーブする必要はないが管理の関係で一旦ここに記載
        public string TotalPlayTime;            // DateTime型だと保存できないのでstringで格納
        public int CurrentDate;                 // 現在の日にち
        public int TotalEgidoNumberDelivered;   // 納品したエギドの合計値 所持している値ではないので注意
        public Vector3 SpawnWorldPosition;      // 生成する場所

        // TODO: ステージのエリア解放内容
    }
}
