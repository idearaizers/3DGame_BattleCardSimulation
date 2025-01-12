// using UnityEngine;
// using DG.Tweening;

// namespace Siasm
// {
//     public class DamageNumberView : MonoBehaviour
//     {
//         [SerializeField]
//         private GameObject damageNumberPrefab;

//         private ObjectPoolContainer objectPoolContainer;

//         public void Initialize(ObjectPoolContainer objectPoolContainer)
//         {
//             this.objectPoolContainer = objectPoolContainer;
//         }

//         /// <summary>
//         /// プレイヤーとエネミーで出す場所を調整する方がいいかも
//         /// </summary>
//         /// <param name="transform"></param>
//         /// <param name="damageNumber"></param>
//         public void Create(Transform transform, int damageNumber)
//         {
//             var damageNumberGameObject = objectPoolContainer.Get(damageNumberPrefab);

//             damageNumberGameObject.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
//             var damage = damageNumberGameObject.GetComponent<NumberSpriteView>();

//             damage.Apply(damageNumber, false);

//             var sequence = DOTween.Sequence();
//             sequence.Append(
//                         damage.transform.DOLocalMove(new Vector3(transform.localPosition.x, transform.localPosition.y + 2.0f, transform.localPosition.z), 0.5f
//                     ))
//                     .AppendInterval(0.5f)
//                     .AppendCallback(() =>
//                     {
//                         damageNumberGameObject.GetComponent<ReturnToPool>().Release();
//                     })
//                     .SetLink(gameObject);
//         }
//     }
// }
