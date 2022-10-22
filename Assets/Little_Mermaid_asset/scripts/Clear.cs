using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

public class Clear : MonoBehaviour
{

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
        }
        else
        {
            fail_text.gameObject.SetActive(true);
            changeAnimation.sadEnding();
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
