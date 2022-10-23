using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

public class Clear : MonoBehaviour
{

    public Image ending_Image;
    public Image fail_Image;

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
            ending_Image.gameObject.SetActive(true);
            changeAnimation.happyEnding();
        }
        else
        {
            fail_Image.gameObject.SetActive(true);
            changeAnimation.sadEnding();
        }
    }
}
