using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Moru.UI
{
    public class StackUIComponent : MonoBehaviour
    {
        [ContextMenu("보여지기")]
        /// <summary>
        /// 해당 컴포넌트를 가진 오브젝트를 SetActive(true)대신 Show()를 실행시켜주어야 합니다.
        /// </summary>
        public void Show()
        {
            //게임오브젝트를 활성화시킵니다.
            this.gameObject.SetActive(true);
            StackUIManager.Instance.Push(this);
        }

        [ContextMenu("숨겨지기")]
        /// <summary>
        /// 해당 컴포넌트를 가진 오브젝트를 SetActive(false)대신 Hide()를 실행시켜주어야 합니다.
        /// </summary>
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
    }
}