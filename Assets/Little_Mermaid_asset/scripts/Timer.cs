using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
    public float limite_time;
    public TMP_Text text_timer;

    public GameObject score_object;

    public TMP_Text[] countdown_text;

    ScoreManager scoremanager;
    private void Start()
    {
        scoremanager =  score_object.GetComponent<ScoreManager>();
        StartCoroutine(countdown());
        Time.timeScale = 0;
    }

    IEnumerator countdown()
    {
        for (int i = 0; i < countdown_text.Length; i++)
        {
            countdown_text[i].gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(1f);
            countdown_text[i].gameObject.SetActive(false);
        }
        Time.timeScale = 1;
    }

    private void FixedUpdate()
    {
        limite_time -= Time.deltaTime;
        text_timer.text = Mathf.Round(limite_time).ToString();

        if(limite_time <= 0f)
        {
            scoremanager.CheckEnding();
            StartCoroutine(wait());
        }
    }
    

    IEnumerator wait()
    {
        limite_time = 0;
        yield return new WaitForSeconds(0.5f);

        Time.timeScale = 0;
    }
}
