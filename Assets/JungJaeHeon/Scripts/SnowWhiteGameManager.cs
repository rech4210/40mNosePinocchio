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

    [SerializeField]
    [Tooltip("정답 사과 표시 이미지")]
    private Image correctAppleImage;

    [SerializeField]
    [Tooltip("정답 사과 표시 스프라이트들")]
    private Sprite[] displayCorrectAppleSprits;

    [SerializeField]
    [Tooltip("정답 사과 스프라이트들")]
    private Sprite[] correctAppleSprits;

    public float limitTime;

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
                var startTextComponent = startText.GetComponent<Text>();

                limitTime = 0;

                nowGameState = NowGameState.GameEnd;

                startTextComponent.fontSize = 120;

                timerText.text = $"남은 시간 00 : 00";
                startText.text = "실패...";
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
                var startTextComponent = startText.GetComponent<Text>();
                nowGameState = NowGameState.GameEnd;

                startTextComponent.fontSize = 120;

                startText.text = "클리어!";
            }
        }
    }

    private void StartSetting()
    {
        int randSpritsIndex = Random.Range(0, correctAppleSprits.Length);
        correctObj.GetComponent<SpriteRenderer>().sprite = correctAppleSprits[randSpritsIndex];
        correctAppleImage.sprite = displayCorrectAppleSprits[randSpritsIndex];

        limitTime = 120;

        correctObj.transform.position = new Vector3(Random.Range(-8, 9), Random.Range(-3, 4));

        if (correctObj.transform.position.x > 4 || correctObj.transform.position.x < -2)
        {
            correctObj.transform.position = new Vector3(Random.Range(-2, 4), correctObj.transform.position.y);
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
