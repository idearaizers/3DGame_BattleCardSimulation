using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    public class BattleFighterAnimation : MonoBehaviour
    {
        [SerializeField]
        private Image image;

        private BattleFighterAnimationTypeSprites battleFighterAnimationTypeSprites;

        public void Initialize(bool isPlayer, BattleFighterAnimationTypeSprites battleFighterAnimationTypeSprites)
        {
            if (isPlayer)
            {
                this.transform.localScale = Vector3.one;
            }
            // NOTE: エネミーの場合はスケールを使用して向きを変えている
            else
            {
                // this.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                this.transform.localScale = Vector3.one;
            }

            this.battleFighterAnimationTypeSprites = battleFighterAnimationTypeSprites;

            // 初期は待機状態に変更
            SetImage(BattleFighterAnimationType.Idle);
        }

        public void Setup() { }

        public void SetImage(BattleFighterAnimationType battleFighterAnimationType)
        {
            if (battleFighterAnimationType == BattleFighterAnimationType.Dead)
            {
                Debug.Log("TODO: 暫定で死亡時にキャラの向きを変更。正しいアセットが用意できた際に削除予定");
                this.gameObject.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }

            image.sprite = battleFighterAnimationTypeSprites.GetSprite(battleFighterAnimationType);
        }
    }
}
