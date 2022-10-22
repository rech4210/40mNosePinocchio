using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartDelayUI : MonoBehaviour
{
    public GameObject panel;
    public Text remainText;
    public int remainTime;
    
    public IEnumerator StartDelay(int time)
    {
        remainTime = time;
        while (remainTime > 0)
        {
            remainText.text = remainTime + "";
            remainTime--;
            yield return new WaitForSeconds(1);
        }
        
        panel.SetActive(false);
    }
}
