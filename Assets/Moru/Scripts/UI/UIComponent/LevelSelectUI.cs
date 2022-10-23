using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using PD;

namespace Moru.UI
{
    public class LevelSelectUI : MonoBehaviour
    {
        [SerializeField, LabelText("왼쪽 버튼")] Button LeftBtn;
        [SerializeField, LabelText("오른쪽 버튼")] Button RightBtn;
        [SerializeField, LabelText("설명 버튼")] Button ExplainBtn;
        [SerializeField, LabelText("콘텐츠")] Transform contents;
        GAME_INDEX cur_Index;
        int maxStageInt;

        //string isFail, isSuccess;
        //Animator anim;

        //void AA()
        //{
        //    anim.SetBool(isFail, true);
        //}


        // Start is called before the first frame update
        void Awake()
        {

            PlayerData.instance.onSelectStage += SetUp;
            LeftBtn.onClick.AddListener
                (
                    () =>
                    {
                        SetUp(PlayerData.GetStageClearDataPerGame(cur_Index - 1), cur_Index - 1);
                        PlayerData.instance.Cur_Game_Index--;
                    }
                );
            RightBtn.onClick.AddListener
                (
                    () =>
                    {
                        SetUp(PlayerData.GetStageClearDataPerGame(cur_Index + 1), cur_Index + 1);
                        PlayerData.instance.Cur_Game_Index++;
                    }
                );


        }
        private void Start()
        {
            SetUp(PlayerData.GetStageClearDataPerGame(PlayerData.instance.Cur_Game_Index), PlayerData.instance.Cur_Game_Index);
        }
        public void SetUp(bool[] stageArr, GAME_INDEX index)
        {
            cur_Index = index;
            bool isinit = false;
            //스테이지 리스트 업데이트
            for (int i = 0; i < contents.childCount; i++)
            {
                //이런식의 참조...극혐하지만 너무 귀찮다.
                var comp = contents.GetChild(i).GetComponent<Button>();
                //comp.onClick.RemoveAllListeners();
                //comp.onClick.AddListener(
                //    () => PlayerData.instance.CurStageSelectedNum = i);
                //Old
                //comp.onClick.AddListener(
                //    () =>
                //    {
                //        PlayerData.instance.Cur_Game_Index = index;
                //        WSceneManager.instance.MoveScene((SceneIndex)index + 1);
                //        Debug.Log($" 플레이어가 선택한 게임 / 스테이지 : {index} / {i}, ");
                //    }

                //    );
                //New
                if(comp.gameObject.TryGetComponent<SeleteStageBtn>(out var addComp))
                {
                    addComp.Init(i, index);
                }
                else
                {
                    comp.gameObject.AddComponent<SeleteStageBtn>().Init(i, index);
                }


                comp.transform.GetChild(1).GetComponent<Text>().text = $"스테이지 {i + 1}";
                if (i < stageArr.Length)
                {

                    contents.GetChild(i).gameObject.SetActive(true);
                    //완전히 클리어한 스테이지
                    if (stageArr[i])
                    {
                        comp.interactable = true;
                        comp.transform.GetChild(0).gameObject.SetActive(false);
                        comp.transform.GetChild(2).gameObject.SetActive(true);
                    }
                    else
                    {
                        if (!isinit)
                        {
                            maxStageInt = i;
                            isinit = true;
                        }
                        //도전할 수 있는 스테이지
                        if (comp == contents.GetChild(maxStageInt).GetComponent<Button>())
                        {
                            comp.interactable = true;
                            comp.transform.GetChild(0).gameObject.SetActive(false);
                            comp.transform.GetChild(2).gameObject.SetActive(false);
                        }
                        //도전불가능한 스테이지
                        else
                        {
                            comp.interactable = false;
                            comp.transform.GetChild(0).gameObject.SetActive(true);
                            comp.transform.GetChild(2).gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    contents.GetChild(i).gameObject.SetActive(false);
                }
            }

            //좌 우 버튼 활성화
            if (index == 0)
            {

                LeftBtn.interactable = false;
                RightBtn.interactable = PlayerData.IsOpenChapter((GAME_INDEX)index + 1) ? true : false;
            }
            else if (index + 1 == GAME_INDEX.None)
            {
                LeftBtn.interactable = PlayerData.IsOpenChapter((GAME_INDEX)index - 1) ? true : false;
                RightBtn.interactable = false;
            }
            else
            {
                LeftBtn.interactable = PlayerData.IsOpenChapter((GAME_INDEX)index - 1) ? true : false;
                RightBtn.interactable = PlayerData.IsOpenChapter((GAME_INDEX)index + 1) ? true : false;
            }

            //게임설명창 팝업 이미지 업데이트
            var ExplainSprite = PlayerData.instance.ChapterStorySO._ChapterStroy[(int)cur_Index].ExplainSprite;
            ExplainBtn.GetComponent<PopNPush>().Push_comp.GetComponent<GameExplainPopUp>().Init(ExplainSprite);
            //ExplainBtn.onClick.AddListener(
            //    () => ExplainBtn.GetComponent<PopNPush>().Push_comp.GetComponent<GameExplainPopUp>().Init(ExplainSprite)
            //    );
        }

        public class SeleteStageBtn : MonoBehaviour
        {
            public int count;
            public GAME_INDEX index;
            public Button myButton;

            public void Init(int count, GAME_INDEX index)
            {
                this.count = count;
                this.index = index;
                myButton = GetComponent<Button>();
                myButton.onClick.AddListener(
                    () =>
                    {
                        PlayerData.instance.CurStageSelectedNum = count;
                        PlayerData.instance.Cur_Game_Index = index;
                        WSceneManager.instance.MoveScene((SceneIndex)index + 1);
                    }
                    );
            }
        }
    }
}