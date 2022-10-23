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


        private void Awake()
        {
            PlayerData.instance.onOpenChapter += UpdateChapterButton;
            var bt = GetComponent<Button>();
            bt.onClick.AddListener(
                () => PlayerData.instance.Cur_Game_Index = myindex
                );
        }


        public void OnClickStage()
        {
            PlayerData.instance.onSelectStage?.Invoke
                (
                    PlayerData.GetStageClearDataPerGame(myindex), myindex
                    );
        }

        public void UpdateChapterButton(GAME_INDEX myIndex)
        {
            chapterName.text =
                    // myIndex.ToString();
                    "";
            if (!PlayerData.IsOpenChapter(myIndex))
            {
                if (btn_Image != null && chapterName != null)
                {
                    btn_Image.enabled = false;
                    chapterName.enabled = false;
                }
            }
            else
            {
                if (btn_Image != null && chapterName != null)
                {
                    btn_Image.enabled = true;
                    chapterName.enabled = true;
                }
            }
        }
    }
}