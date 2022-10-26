using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using PD;

namespace Moru.UI
{
    public class LevelSelectUI : StackUIComponent
    {
        [SerializeField, LabelText("선택한 게임")] Text cur_SelectedGame;
        [SerializeField, LabelText("클리어율")] Image clearRate;
        [SerializeField, LabelText("클리어율")] Text clearRate_Text;

        [SerializeField, LabelText("왼쪽 버튼")] Button LeftBtn;
        [SerializeField, LabelText("오른쪽 버튼")] Button RightBtn;
        [SerializeField, LabelText("설명 버튼")] Button ExplainBtn;
        [SerializeField, LabelText("콘텐츠")] Transform contents;
        [SerializeField]Button[] buttons;
        GAME_INDEX cur_Index => PlayerDataXref.pl.Cur_Game_Index;
        int maxStageInt;


        private void Start()
        {            //버튼리스트 참조
            buttons = new Button[contents.childCount];
            for (int i = 0; i < contents.childCount; i++)
            {
                buttons[i] = contents.GetChild(i).GetComponent<Button>();
            }


            PlayerDataXref.pl.del_StageSetUp += SetUp;
            LeftBtn.onClick.AddListener
                (
                    () =>
                    {
                        PlayerDataXref.pl.Cur_Game_Index--;
                        SetUp(cur_Index);
                    }
                );
            RightBtn.onClick.AddListener
                (
                    () =>
                    {
                        PlayerDataXref.pl.Cur_Game_Index++;
                        SetUp(cur_Index);
                    }
                );
            SetUp(cur_Index);
        }

        private void OnDestroy()
        {
            PlayerDataXref.pl.del_StageSetUp -= SetUp;
        }
        private void SetUp(GAME_INDEX index)
        {
            var rate = PlayerDataXref.pl.GetGameIndexClearRate(index);
            clearRate.fillAmount = rate;
            clearRate_Text.text = (rate * 100).ToString("F0") + "%";
            //게임이름 업데이트
            cur_SelectedGame.text = PlayerDataXref.pl.ChapterStorySO._ChapterStroy[(int)index].ChapterName;
            //현재 선택한 게임의 최대스테이지 받아오기

            //스테이지 리스트 업데이트
            for (int i = 0; i < buttons.Length; i++)
            {
                var comp = buttons[i];
                if(i >= PlayerDataXref.pl.GetStageLength(index))
                    { buttons[i].gameObject.SetActive(false); continue; }
                else { buttons[i].gameObject.SetActive(true); }
                if(comp.gameObject.TryGetComponent<SelectedStageBtn>(out var addComp))
                {
                    bool isOpen = PlayerDataXref.pl.GetGameStageOpen(index, i);
                    bool isClear = PlayerDataXref.pl.GetGameStageClear(index, i);
                    addComp.SetUp(i, index, isClear, isOpen);
                }
            }

            //좌 우 버튼 활성화
            if (index == 0)
            {

                LeftBtn.interactable = false;
                RightBtn.interactable = PlayerDataXref.pl.IsChapterOpen((GAME_INDEX)index + 1) ? true : false;
            }
            else if (index + 1 == GAME_INDEX.None)
            {
                LeftBtn.interactable = PlayerDataXref.pl.IsChapterOpen((GAME_INDEX)index - 1) ? true : false;
                RightBtn.interactable = false;
            }
            else
            {
                LeftBtn.interactable = PlayerDataXref.pl.IsChapterOpen((GAME_INDEX)index - 1) ? true : false;
                RightBtn.interactable = PlayerDataXref.pl.IsChapterOpen((GAME_INDEX)index + 1) ? true : false;
            }
        }
    }
}