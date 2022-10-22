using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoSingleton<TimerManager>
{
    public Text remainText;
    public int remainTime = 0;
    
    public void Init(int maxTime)
    {
        remainTime = maxTime;
    }

    public IEnumerator StartTimer(Action act)
    {
        while (remainTime >= 0)
        {
            remainTime--;
            remainText.text = remainText + "";
            
            yield return new WaitForSeconds(1);
        }

        act();
    }
}
