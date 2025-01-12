using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.AddressableAssets;

// ブラーシェーダー
// https://assetstore.unity.com/packages/vfx/shaders/ui-blur-173331?locale=ja-JP

namespace Siasm
{
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

        // 頻繁に使用するものだけ参照している
        [Space]
        [SerializeField]
        private MessageDialog messageDialogPrefab;

        [SerializeField]
        private ConfirmDialog ConfirmDialog;

        private float backgroundFadeDuration = 0.1f;
        private Stack<BaseDialog> dialogStack = new Stack<BaseDialog>();
        private Coroutine fadingBackclothCoroutine;

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
        /// メッセージ用のダイアログを表示する
        /// </summary>
        /// <param name="parameter"></param>
        public void OpenMessageDialog(BaseDialog.Parameter parameter)
        {
            var dialog = OpenDialog(messageDialogPrefab);
            dialog.ApplyParameter(parameter);
            dialog.OnCloseAction = CloseDialog;
        }

        /// <summary>
        /// 確認用のダイアログを表示する
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="decideAction"></param>
        /// <param name="cancelAction"></param>
        public void OpenConfirmDialog(BaseDialog.Parameter parameter, Action decideAction, Action cancelAction = null)
        {
            var dialog = OpenDialog(ConfirmDialog);
            dialog.ApplyParameter(parameter);
            dialog.OnCancelAction = () =>
            {
                cancelAction?.Invoke();
                CloseDialog();
            };
            dialog.OnDecideAction = () =>
            {
                decideAction?.Invoke();
                CloseDialog();
            };
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
            dialog = null;

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







        // private async UniTask<GameObject> InstantiatePageAsync(object key)
        // {
        //     if (AssetCacheManager.Instance.Exist(key))
        //     {
        //         var cachedPrefab = AssetCacheManager.Instance.GetAsset<GameObject>(key);
        //         var instance = Instantiate(cachedPrefab);
        //         // AssetCacheManager.Instance.cachePageNames.Add(instance.name);
        //         return instance;
        //     }
        //     // NOTE: ここでは生成したGameObjectをキャッシュはしていないみたい
        //     // NOTE: ロードとインスタンス化の両方を行っている
        //     return await Addressables.InstantiateAsync(key, transform);
        // }






        /// <summary>
        /// 現在のダイアログを閉じて新しいMessageDialogを開く
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public MessageDialog ReplaceMessageDialog(MessageDialog.Parameter parameters)
        {
            var dialog = Replace(messageDialogPrefab);
            // dialog.ApplyParams(dialogParams);
            return dialog;
        }

        /// <summary>
        /// 現在のダイアログを閉じて新しいYesNoDialogを開く
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ConfirmDialog ReplaceConfirmationDialog(ConfirmDialog.Parameter parameters)
        {
            var dialog = Replace(ConfirmDialog);
            // dialog.ApplyParams(dialogParams);
            return dialog;
        }

        private void StopFadingBackclothCorutione()
        {
            if (fadingBackclothCoroutine == null)
            {
                return;
            }

            StopCoroutine(fadingBackclothCoroutine);
            fadingBackclothCoroutine = null;
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

                // await prevDialog.PlayCloseAsync();
                Destroy(prevDialog.gameObject);
                dialog.gameObject.SetActive(true);
                // dialog.PlayOpenAsync().Forget();
            });

            // dialog.SetCloseAction(Close, CloseImmediately);
            dialogStack.Push(dialog);

            return dialog;
        }

        private void CloseImmediately()
        {
            var dialog = dialogStack.Pop();

            // dialog.CloseImmediately();
            Destroy(dialog.gameObject);

            if (dialogStack.Count == 0)
            {
                HideBackclothImmediately(backClothImageOfCanvasGroup);
            }
        }

        private IEnumerator ShowBackclothAsync(CanvasGroup backcloth)
        {
            backcloth.gameObject.SetActive(true);
            yield return backcloth.DOFade(1, backgroundFadeDuration)
                .WaitForCompletion();
            fadingBackclothCoroutine = null;
        }

        private IEnumerator HideBackclothAsync(CanvasGroup backcloth)
        {
            yield return backcloth.DOFade(0, backgroundFadeDuration)
                .WaitForCompletion();
            backcloth.gameObject.SetActive(false);
            fadingBackclothCoroutine = null;
        }

        private void HideBackclothImmediately(CanvasGroup backcloth)
        {
            backcloth.alpha = 0;
            backcloth.gameObject.SetActive(false);
        }
    }
}
