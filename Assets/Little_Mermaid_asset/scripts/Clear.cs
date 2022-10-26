using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

public class Clear : MonoBehaviour
{
    private static int failCount = 0;
    public int currentStage;
    public TMP_Text ending_text;
    public TMP_Text fail_text;

    public Image liquor;
    public int require_score;

    public delegate void endingDele(int score);

    public endingDele endingDelegate;

    ChangeAnimation changeAnimation;
    private void Start()
    {
        changeAnimation = gameObject.GetComponent<ChangeAnimation>();
        endingDelegate = ending; // 스코어 매니저로 넘겨주기
    }

    public void ending(int score)
    {
        if(score >= require_score )
        {
            liquor.gameObject.SetActive(true);
            ending_text.gameObject.SetActive(true);
            changeAnimation.happyEnding();

            PlayerDataXref.instance.ClearGame(GAME_INDEX.Little_Mermaid, currentStage);
            WSceneManager.instance.OpenGameClearUI();
            ////다음챕터을 엽니다. 다른분들도 이렇게 해주시면 되요
            //if (currentStage == PlayerDataXref.instance.GetTargetState_ToOpenNextChapter(GAME_INDEX.Little_Mermaid))
            //{
            //    PlayerDataXref.instance.OpenChapter(GAME_INDEX.Cinderella + 1);
            //}
            //else
            //{
            //}

            ////올클리어  & 1회도 실패하지 않고 클리어시 업적 이벤트 예시
            //if (currentStage == PlayerDataXref.instance.GetMaxStageNumber(GAME_INDEX.Little_Mermaid) - 1)
            //{
            //    PlayerDataXref.instance.SetAchieveSuccess(ACHEIVE_INDEX.LITTLE_MERMAID_ALL_CLEAR);
            //    PlayerDataXref.instance.ClearChapter(GAME_INDEX.Little_Mermaid);
            //    if (failCount == 0)
            //    {
            //        PlayerDataXref.instance.SetAchieveSuccess(ACHEIVE_INDEX.DRUG_KING);
            //    }
            //}
        }
        else
        {
            fail_text.gameObject.SetActive(true);
            changeAnimation.sadEnding();
            failCount++;
            WSceneManager.instance.OpenGameFailUI();
            // 실패 애니메이션 + 실패 텍스트
        }
    }

    // 상태 주입
    // 클리어 델리게이트 생성
    // 클리어 상태 돌입시 델리게이트 활성화
    // 타임 스캐일 0
    // 화면 어둡게
    // 분기에 따라 애니메이션 변경
    // 성공시 중앙에 물약 표시
}
