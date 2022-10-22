using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    public delegate int scoreDelegate();
    public scoreDelegate scoreDel; // ���� ��������Ʈ

    private int _score = 0;
    private int _combo_bonus = 0;
    private int comboValue = 1;
    private int comboCount = 0;

    public int score{ get{ return _score; } set { _score += value; textChange();} }
    public int comboBonus{ get { return _combo_bonus; } set { _combo_bonus = value; } }

    public TMP_Text score_text; // �ؽ�Ʈ �߰��ϱ�
    public TMP_Text combo_text;
    public TMP_Text combo_bonus_text;

    Clear clear;

    public void CheckEnding()
    {
        clear.endingDelegate(score);
    }

    public void Start()
    {

        clear = gameObject.GetComponent<Clear>();
        
        score_text.text = score + "score";

        scoreDel += countUp;
        scoreDel += comboSystem;
    }

    void textChange()
    {
        score_text.text = score + " score";
        combo_text.text = comboCount + " Combo!";
        combo_bonus_text.text = comboValue+ " x " + "100 "+ "+ " + comboBonus + " Bonus!";
    }
    
    private int countUp()
    {
        return comboCount++;
    }

    int comboSystem()
    {
        Debug.Log("�޺� �ý��� �۵�");
        if(comboCount >= 50)
        {
            comboValue = 7;
            comboBonus = 280; // 0.4 ����
        }
        else if(comboCount >= 30)
        {
            comboValue = 4;
            comboBonus = 120;// 0.3 ����
        }
        else if(comboCount >= 10)
        {
            comboValue = 2;
            comboBonus = 50;// 0.25 ����
        }
        else if (comboCount < 10)
        {
            comboValue = 1;
            comboBonus = 0;
        }
        score = (100 * comboValue) + comboBonus; // ���� = 100* �޺� ���� + �޺� ���ʽ�

        Debug.Log(comboBonus);
        return score;
    }
}