using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Siasm
{
    /// <summary>
    /// メニューアームのダイアログ表示とは別物でこちらは旧処理が多いため注意
    /// TODO: 不要な処理をリファクタ予定
    /// </summary>
    public class DialogManager : SingletonMonoBehaviour<DialogManager>
    {
        [SerializeField]
        private Canvas mainDialogCanvas;

        [SerializeField]
        private Canvas subDialogCanvas;

        [SerializeField]
        private CanvasGroup backClothImageOfCanvasGroup;

        [SerializeField]
        private Transform mainContainerTransform;

        [SerializeField]
        private Transform subContainerTransform;

        private Stack<BaseDialog> dialogStack = new Stack<BaseDialog>();

        public void Initialize()
        {
            backClothImageOfCanvasGroup.gameObject.SetActive(false);
            backClothImageOfCanvasGroup.alpha = 1.0f;
        }

        public void Setup(Camera mainCamera)
        {
            mainDialogCanvas.worldCamera = mainCamera;
            subDialogCanvas.worldCamera = mainCamera;
        }

        /// <summary>
        /// プレハブを指定してダイアログを開く
        /// </summary>
        /// <param name="dialogPrefab"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T OpenDialog<T>(T dialogPrefab) where T : BaseDialog
        {
            backClothImageOfCanvasGroup.gameObject.SetActive(true);
            backClothImageOfCanvasGroup.alpha = 1.0f;

            MoveDialogToSubContainer();

            var dialog = Instantiate(dialogPrefab, mainContainerTransform, false);
            dialog.Initialize();
            dialogStack.Push(dialog);
            return dialog;
        }

        /// <summary>
        /// アドレスを指定して開くが最初だけ若干遅延があるような挙動になっているので注意
        /// Pefabを渡す形がいいかも
        /// 初期化は行っているが各ボタンのアクションは呼び出し基で行う
        /// </summary>
        /// <param name="dialogPrefab"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async UniTask<T> OpenDialogAsync<T>(string prefabAddress) where T : BaseDialog
        {
            backClothImageOfCanvasGroup.gameObject.SetActive(true);
            backClothImageOfCanvasGroup.alpha = 1.0f;

            MoveDialogToSubContainer();

            T dialog = null;
            if (AssetCacheManager.Instance.Exist(prefabAddress))
            {
                var dialogPrefab = AssetCacheManager.Instance.GetAsset<GameObject>(prefabAddress);
                dialog = Instantiate(dialogPrefab, mainContainerTransform, false).GetComponent<T>();
            }
            else
            {
                // NOTE: 暫定でコメントアウト
                // キャッシュしていない場合でキャッシュする場合
                // var dialogPrefab = await AssetCacheManager.Instance.LoadAssetAsync<GameObject>(prefabAddress);
                // dialog = Instantiate(dialogPrefab, containerTransform, false).GetComponent<T>();

                // キャッシュしていない場合でキャッシュしない場合
                var dialogPrefab = await Addressables.InstantiateAsync(prefabAddress, mainContainerTransform);
                dialog = dialogPrefab.GetComponent<T>();
            }

            dialog.Initialize();
            dialogStack.Push(dialog);
            return dialog;
        }

        /// <summary>
        /// 最前面にダイアログがあればサブコンテナー（後面）に移動させる
        /// 主に新しいダイアログを開く際に使用する
        /// </summary>
        private void MoveDialogToSubContainer()
        {
            if (dialogStack.Count == 0)
            {
                return;
            }

            var foremostDialog = dialogStack.Peek();
            foremostDialog.transform.SetParent(subContainerTransform, false);

            if (dialogStack.Count >= 1)
            {
                subDialogCanvas.gameObject.SetActive(true);
            }
            else
            {
                subDialogCanvas.gameObject.SetActive(false);
            }
        }

        public void CloseDialog()
        {
            backClothImageOfCanvasGroup.gameObject.SetActive(false);
            backClothImageOfCanvasGroup.alpha = 0.0f;

            var dialog = dialogStack.Pop();
            Destroy(dialog.gameObject);

            MoveDialogToMainContainer();
        }

        /// <summary>
        /// ダイアログが他にもあればメインコンテナー（最前面）に移動させる
        /// 主にダイアログを閉じる際に使用する
        /// </summary>
        private void MoveDialogToMainContainer()
        {
            if (dialogStack.Count == 0)
            {
                return;
            }

            var foremostDialog = dialogStack.Peek();
            foremostDialog.transform.SetParent(mainContainerTransform, false);

            if (dialogStack.Count >= 1)
            {
                subDialogCanvas.gameObject.SetActive(true);
            }
            else
            {
                subDialogCanvas.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 現在のダイアログを閉じて新しいダイアログを開く
        /// </summary>
        /// <param name="dialogPrefab"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Replace<T>(T dialogPrefab) where T : BaseDialog
        {
            var prevDialog = dialogStack.Pop();

            var dialog = Instantiate(dialogPrefab, mainContainerTransform, false);
            dialog.gameObject.SetActive(false);

            UniTask.Create(async () => 
            {
                await UniTask.CompletedTask;

                Destroy(prevDialog.gameObject);
                dialog.gameObject.SetActive(true);
            });

            dialogStack.Push(dialog);

            return dialog;
        }

        private void CloseImmediately()
        {
            var dialog = dialogStack.Pop();

            Destroy(dialog.gameObject);

            if (dialogStack.Count == 0)
            {
                HideBackclothImmediately(backClothImageOfCanvasGroup);
            }
        }

        private void HideBackclothImmediately(CanvasGroup backcloth)
        {
            backcloth.alpha = 0;
            backcloth.gameObject.SetActive(false);
        }
    }
}
