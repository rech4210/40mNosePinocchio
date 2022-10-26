using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Moru.UI
{

    public class AchievePopUp : MonoBehaviour
    {
        public float openTime = 1f;
        public float waitTime = 1f;
        public float closingTime = 2f;

        [SerializeField] private Image icon;
        [SerializeField] private Text achieveNameViewer;
        [SerializeField] private Text titleViewer;
        [SerializeField]
        Sequence sq;
        CanvasGroup canvas;

        // Start is called before the first frame update
        void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            canvas = GetComponent<CanvasGroup>();
            canvas.alpha = 0;
            sq.Append(canvas.DOFade(1, openTime));
            sq.Append(canvas.DOFade(0, closingTime).SetDelay(waitTime + openTime));
            Invoke("Destroy", openTime + waitTime + closingTime + 1);
        }

        public void SetViewer(AchieveResult result)
        {
            if (icon != null && result.Icon != null)
            {
                icon.sprite = result.Icon;
            }
            if (achieveNameViewer != null)
            {
                achieveNameViewer.text = $"'{result.AchieveName}'"
                + " 업적을 달성했다!";
            }
            if (titleViewer != null)
            {
                titleViewer.text = $"'{result.Title}'"
                + " 칭호를 획득하였습니다.";
            }
        }
        private void Destroy()
        {
            Destroy(this.gameObject);
        }
    }
}