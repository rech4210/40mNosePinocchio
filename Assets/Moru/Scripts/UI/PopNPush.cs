using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Moru.UI
{
    public class PopNPush : MonoBehaviour, ISubmitHandler, IPointerClickHandler
    {


        public void OnPointerClick(PointerEventData eventData)
        {
            PopAndPush();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            PopAndPush();
        }
        [SerializeField] private bool isPopAndPush;
        [SerializeField] private StackUIComponent push_comp;
        public StackUIComponent Push_comp => push_comp;

        void Start()
        {

        }

        private void PopAndPush()
        {
            if (GetComponent<Button>().interactable)
            {
                if (push_comp == null) Debug.Log($"push대상이 없습니다. 기능 종료");
                else
                {
                    if (isPopAndPush)
                    {
                        StackUIManager.Instance.Pop();
                    }
                    push_comp.Show();
                        SoundManager.PlaySFX(StackUIManager.Instance.clickSFX);

                }
            }
            else
            {
                SoundManager.PlaySFX(StackUIManager.Instance.clickFailSFX);
            }
        }
    }
}