using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using PD;

namespace Moru.UI
{
    public class StoryBoardUI : MonoBehaviour
    {
        private delegate void OnClick_NoRet_NoParam(StoryBoardUI btn);
        private static event OnClick_NoRet_NoParam onClick;
        [SerializeField] private bool isOpen = false;
        [SerializeField] private GAME_INDEX myIndex;

        [BoxGroup("앞면"), LabelText(""), SerializeField] Text chapterName;
        [BoxGroup("뒷면"), LabelText(""), SerializeField] Text chapterDesc;
        [BoxGroup("뒷면"), LabelText(""), SerializeField] Image chapterImg;


        private ChapterStroy myChapterStory;

        private void Awake()
        {
            onClick += BroadCast_EventClick;
        }

        private void Start()
        {
            myChapterStory = PlayerDataXref.pl.ChapterStorySO._ChapterStroy[(int)myIndex];
            chapterName.text = myChapterStory.ChapterName;
            chapterDesc.text = myChapterStory.ChapterDesc;
            chapterImg.sprite = myChapterStory.BackGround;



            var btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
            if (PlayerDataXref.pl.IsClearChapter(myIndex))
            {
                btn.interactable = true;
                chapterDesc.enabled = false;
                chapterImg.enabled = false;
            }
            else
            {
                btn.interactable = false;
                chapterDesc.enabled = false;
                chapterImg.enabled = false;
            }
        }


        void OnClick()
        {
            if (!isOpen)
            {
                isOpen = true;
                chapterName.enabled = false;
                chapterDesc.enabled = true;
                chapterImg.enabled = true;
                onClick?.Invoke(this);
            }
            else
            {
                Hide();

            }
        }

        void Hide()
        {
            isOpen = false;
            chapterName.enabled = true;
            chapterDesc.enabled = false;
            chapterImg.enabled = false;
        }

        private void BroadCast_EventClick(StoryBoardUI btn)
        {
            if (btn != this)
            {
                if (isOpen)
                {
                    Hide();
                }
            }
        }
    }
}