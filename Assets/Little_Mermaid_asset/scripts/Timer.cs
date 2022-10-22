using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
    public float limite_time;
    public TMP_Text text_timer;

    private void FixedUpdate()
    {
        limite_time -= Time.deltaTime;
        text_timer.text = Mathf.Round(limite_time).ToString();
    }
}
