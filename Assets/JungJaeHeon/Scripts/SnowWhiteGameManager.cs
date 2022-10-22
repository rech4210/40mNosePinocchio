using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum NowGameState
{
    GameReady,
    Gaming,
    GameEnd,
    GameStop
}

public class SnowWhiteGameManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("시작 텍스트")]
    private Text startText;

    [SerializeField]
    [Tooltip("제한 시간 텍스트")]
    private Text timerText;

    [SerializeField]
    [Tooltip("정답 오브젝트")]
    private GameObject correctObj;

    private float limitTime;

    private NowGameState nowGameState;

    // Start is called before the first frame update
    void Start()
    {
        StartSetting();
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
        CorrectAnswerDistinction();
    }

    private void Timer()
    {
        if (nowGameState == NowGameState.Gaming)
        {
            limitTime -= Time.deltaTime;
            timerText.text = $"남은 시간 {((int)limitTime / 60):D2} : {((int)limitTime % 60):D2}";
            if (limitTime <= 0)
            {
                limitTime = 0;
                timerText.text = $"남은 시간 00 : 00";
            }
        }
    }

    private void CorrectAnswerDistinction()
    {
        if (nowGameState == NowGameState.Gaming && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null && hit.collider.CompareTag("CorrectApple"))
            {
                print("찾음");
            }
        }
    }

    private void StartSetting()
    {
        limitTime = 120;

        correctObj.transform.position = new Vector3(Random.Range(-8, 9), Random.Range(-4, 5));

        if (correctObj.transform.position.x > 4 || correctObj.transform.position.x < -2)
        {
            correctObj.transform.position = new Vector3(Random.Range(-2, 5), correctObj.transform.position.y);
        }

        nowGameState = NowGameState.GameReady;

        StartCoroutine(StartTextAnim());
    }

    private IEnumerator StartTextAnim()
    {
        WaitForSeconds textAnimDelay = new WaitForSeconds(2);
        var startTextComponent = startText.GetComponent<Text>();
        
        startTextComponent.fontSize = 120;

        startText.text = "준비..";
        yield return textAnimDelay;

        for (int nowRepetitionIndex = 3; nowRepetitionIndex >= 1; nowRepetitionIndex--)
        {
            startTextComponent.fontSize = 300;

            startText.text = $"{nowRepetitionIndex}";

            while (startTextComponent.fontSize > 2)
            {
                startTextComponent.fontSize -= 1;
                yield return null;
            }
        }

        startTextComponent.fontSize = 120;
        startText.text = "시작!";
        yield return textAnimDelay;

        startText.text = "";
        nowGameState = NowGameState.Gaming;

        yield return null;
    }
}
