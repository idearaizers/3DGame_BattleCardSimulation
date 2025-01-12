using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// バトルシーンから起動した際は強制的に下記の設定値でプレイを開始する
    /// デバッグ用で中身はインスペクターで設定する
    /// 初期化やセットアップ処理は不要で、スクリプタブルオブジェクトで管理でもいいかも
    /// これは開始時に必要なデバッグをまとめたクラス
    /// </summary>
    public class BattleSceneDebug : MonoBehaviour
    {
        [Header("エネミー関連")]
        [SerializeField]
        private int enemyBattleFighterId = 2001;

        [SerializeField]
        private int enemyBattleFighterLevel = 1;

        [Header("プレイヤー関連")]
        [SerializeField]
        private int maxHealthPoint = 15;

        [SerializeField]
        private int maxThinkingPoint = 5;

        [SerializeField]
        private int beginBattleBoxNumber = 1;

        [SerializeField]
        private int maxBattleBoxNumber = 5;

        // エネミー関連
        public int EnemyBattleFighterId => enemyBattleFighterId;
        public int EnemyBattleFighterLevel => enemyBattleFighterLevel;

        // プレイヤー関連
        public int MaxHealthPoint => maxHealthPoint;
        public int MaxThinkingPoint => maxThinkingPoint;
        public int BeginBattleBoxNumber => beginBattleBoxNumber;
        public int MaxBattleBoxNumber => maxBattleBoxNumber;
    }
}
