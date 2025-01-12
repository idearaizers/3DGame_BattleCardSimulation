using System;
using System.Threading;
using UnityEngine;

namespace Siasm
{
    public sealed class TitleUIManager : MonoBehaviour
    {
        /// <summary>
        /// 設定とセーブだけ使用可能
        /// </summary>
        private readonly bool[] activeMenus = new bool[]
        {
            false,  // アイテム
            false,  // デッキ
            true,   // 設定
            false,  // セーブ
            true,   // ロード
            false,
            false,
            false,
            false,
            false
        };

        [SerializeField]
        private MenuArmController menuArmController;

        [SerializeField]
        private TitleUIContent titleUIContent;

        [SerializeField]
        private TitleDirectionContent titleDirectionContent;

        public MenuArmController MenuArmController => menuArmController;
        public TitleUIContent TitleUIContent => titleUIContent;

        public Action<int> OnTitleMenuButtonAction { get; set; }

        public void Initialize(CancellationToken token, BaseUseCase baseUseCase, TitleCameraController titleCameraController)
        {
            menuArmController.Initialize(token, baseUseCase, titleCameraController);
            menuArmController.OnHidAction = () => titleUIContent.ShowTitleMenu();

            titleUIContent.Initialize();
            titleUIContent.OnTitleMenuButtonAction = (selectedIndex) => OnTitleMenuButtonAction?.Invoke(selectedIndex);

            titleDirectionContent.Initialize();
        }

        public void Setup()
        {
            menuArmController.Setup(activeMenus, selectedIndex: -1);
            titleUIContent.Setup();
            titleDirectionContent.Setup();
        }
    }
}
