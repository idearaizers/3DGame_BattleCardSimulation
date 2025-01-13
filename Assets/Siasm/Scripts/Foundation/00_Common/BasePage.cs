using System;
using UnityEngine;

namespace Siasm
{
    public abstract class BasePage : MonoBehaviour
    {
        public PageNavigator PageNavigator { get; set; }
        public Action OnResume { get; set; }

        /// <summary>
        /// NOTE: Initializeの方が分かりやすいかも
        /// </summary>
        /// <param name="pageNavigator"></param>
        public void SetPageNavigator(PageNavigator pageNavigator)
        {
            PageNavigator = pageNavigator;
        }

        /// <summary>
        /// 表示される直前に呼ばれる
        /// </summary>
        public virtual void Enter() { }

        /// <summary>
        /// 別のページがPushされる直前で呼ばれる
        /// </summary>
        public virtual void Suspend() { }

        /// <summary>
        /// 戻ってきたタイミングで呼ばれる
        /// </summary>
        public virtual void Resume()
        {
            OnResume?.Invoke();
        }

        /// <summary>
        /// Pop直後に呼ばれる
        /// </summary>
        public virtual void Exit() { }
    }
}
