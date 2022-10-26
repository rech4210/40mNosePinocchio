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
        endingDelegate = ending; // ���ھ� �Ŵ����� �Ѱ��ֱ�
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
            ////����é���� ���ϴ�. �ٸ��е鵵 �̷��� ���ֽø� �ǿ�
            //if (currentStage == PlayerDataXref.instance.GetTargetState_ToOpenNextChapter(GAME_INDEX.Little_Mermaid))
            //{
            //    PlayerDataXref.instance.OpenChapter(GAME_INDEX.Cinderella + 1);
            //}
            //else
            //{
            //}

            ////��Ŭ����  & 1ȸ�� �������� �ʰ� Ŭ����� ���� �̺�Ʈ ����
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
            // ���� �ִϸ��̼� + ���� �ؽ�Ʈ
        }
    }

    // ���� ����
    // Ŭ���� ��������Ʈ ����
    // Ŭ���� ���� ���Խ� ��������Ʈ Ȱ��ȭ
    // Ÿ�� ��ĳ�� 0
    // ȭ�� ��Ӱ�
    // �б⿡ ���� �ִϸ��̼� ����
    // ������ �߾ӿ� ���� ǥ��
}
