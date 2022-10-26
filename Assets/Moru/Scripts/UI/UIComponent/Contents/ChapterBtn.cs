using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PD;

namespace Moru.UI
{
    public class ChapterBtn : MonoBehaviour
    {
        [SerializeField] GAME_INDEX myindex;
        public GAME_INDEX MyIndex { get => myindex; }
        [SerializeField] Image btn_Image;
        [SerializeField] Text chapterName;
        Button btn;


        private void Start()
        {
            btn = GetComponent<Button>();
            PlayerDataXref.pl.del_onOpenChapter += UpdateChapterButton;
            btn.onClick.AddListener(
                () => { PlayerDataXref.pl.Cur_Game_Index = myindex; OnClickStage(); }
                );
            UpdateChapterButton(myindex);
        }

        private void OnDestroy()
        {
            PlayerDataXref.pl.del_onOpenChapter -= UpdateChapterButton;
        }

        /// <summary>
        /// 챕터버튼을 누르면 발생하는 이벤트
        /// </summary>
        private void OnClickStage()
        {
            PlayerDataXref.pl.del_StageSetUp?.Invoke(myindex);
        }

        /// <summary>
        /// 챕터에 대한 데이터가 변경되었을 때 델리게이트 메서드등록
        /// </summary>
        /// <param name="myIndex"></param>
        private void UpdateChapterButton(GAME_INDEX myIndex)
        {
            chapterName.text =
                    // myIndex.ToString();
                    "";
            if (!PlayerDataXref.pl.IsChapterOpen(myIndex))
            {
                if (btn_Image != null && chapterName != null)
                {
                    btn.interactable = false;
                    chapterName.enabled = false;
                }
            }
            else
            {
                if (btn_Image != null && chapterName != null)
                {
                    btn.interactable = true;
                    chapterName.enabled = true;
                }
            }
        }
    }
}