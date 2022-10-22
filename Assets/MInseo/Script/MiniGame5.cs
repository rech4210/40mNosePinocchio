using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame5 : MonoBehaviour
{
    [SerializeField]
    private Text startText;
    [SerializeField]
    private AudioClip click_Clip, mix_Clip, success_Clip, fail_Clip, bgm_Clip;
    [SerializeField]
    private GameObject paper_Img;
    [SerializeField]
    private Text timerText;
    private static int failcnt;
    public Button btn1, btn2;
    public Text NumberText;
    enum State
    {
        ING,
        LOSE,
        WIN
    }
    public float speed;
    public int currentStage;
    [SerializeField]
    private GameObject pf_CloseCup, pf_OpenCup, pf_Bean, pf_bean_Sprouts;
    [SerializeField]
    private GameObject pf_Dealer_ING, pf_Dealer_LOSE, pf_Dealer_WIN;
    [SerializeField]
    private GameObject pf_Audience1_ING, pf_Audience1_LOSE, pf_Audience1_WIN;
    [SerializeField]
    private GameObject pf_Audience2_ING, pf_Audience2_LOSE, pf_Audience2_WIN;
    [SerializeField]
    private GameObject ui_Clear, ui_Fail;
    GameObject bean, bean_Sprouts, open_cup;
    List<GameObject> dealer, audience1, audience2;
    List<GameObject> cup;
    List<int> ans;
    Vector2 first_V, second_V;
    Vector2 mousePtr;
    RaycastHit2D choice_Cup;
    int startball;
    int first, second, swap, swapCount, numberofCup;
    float delaySwap, swapTime;
    float interval_X;
    bool isMix, choice, input_bean, nextGame, gameState, overTime, gameClear, ready;
    float timeCount, delayTime = 5f, time;
    public bool GetChoice() => choice;

    int[] arr = new int[10] { 5, 6, 7, 6, 7, 8, 7, 8, 9, 10 };
    void Start()
    {
        SoundManager.PlayBGM(bgm_Clip);
        // List 초기화
        cup = new List<GameObject>();
        ans = new List<int>();

        dealer = new List<GameObject>();
        audience1 = new List<GameObject>();
        audience2 = new List<GameObject>();

        numberofCup = currentStage / 4 + 3;

        startball = Random.Range(0, numberofCup);
        input_bean = true;
        choice = false;
        overTime = false;

        ready = true;
        Stage_Update();
        StartCoroutine(StartTextAnim());
    }

    void Stage_Update() // 수정 필요
    {
        bean = Instantiate(pf_Bean);
        bean_Sprouts = Instantiate(pf_bean_Sprouts);
        open_cup = Instantiate(pf_OpenCup);

        bean.SetActive(false);
        bean_Sprouts.SetActive(false);
        open_cup.SetActive(false);
        //ui_Clear.SetActive(false);
        //ui_Fail.SetActive(false);

        dealer.Add(Instantiate(pf_Dealer_ING));
        dealer.Add(Instantiate(pf_Dealer_LOSE));
        dealer.Add(Instantiate(pf_Dealer_WIN));

        audience1.Add(Instantiate(pf_Audience1_ING));
        audience1.Add(Instantiate(pf_Audience1_LOSE));
        audience1.Add(Instantiate(pf_Audience1_WIN));

        audience2.Add(Instantiate(pf_Audience2_ING));
        audience2.Add(Instantiate(pf_Audience2_LOSE));
        audience2.Add(Instantiate(pf_Audience2_WIN));

        for (int i = 0; i < 5; i++) // 다섯 개의 컵 생성
        {
            ans.Add(0);
            cup.Add(Instantiate(pf_CloseCup));
        }
        CupActive();
        bean.transform.position = new Vector2(startball * interval_X, 0f);
        Init_Ready();
    }

    void Init_Game()
    {
        for (int i = 0; i < 5; i++)
        {
            ans[i] = 0;
        }
        numberofCup = currentStage / 4 + 3;

        swapTime = 0f; // 시간 계산
        delaySwap = 3f / arr[currentStage - 1]; // 딜레이 시간

        ans[startball] = 1; // 콩이 있는 컵 활성화
        swapCount = 0; // 섞는 횟수 초기화
        first = 0;

        input_bean = false;
        choice = false;
        isMix = true;
        gameState = true;
        speed = 30 + 15 * (currentStage / 4);

        bean.SetActive(false);
        bean_Sprouts.SetActive(false);

        CupActive();
        Init_Ready();
    }

    void Init_Ready()
    {
        // win, lose 프리팹 비활성화
        for (int i = 1; i < 3; i++)
        {
            Character_Active(i, false);
        }

        // 컵 갯수에 따른 위치
        for (int i = 0; i < numberofCup; i++)
        {
            cup[i].transform.position = new Vector2(interval_X * i, -2.2f);
        }
    }

    void CupActive()
    {
        if (numberofCup == 3)
        {
            interval_X = 7f;
            for (int i = 0; i < 3; i++)
            {
                cup[i].SetActive(true);
            }
            cup[3].SetActive(false);
            cup[4].SetActive(false);
        }
        else if (numberofCup == 4)
        {
            interval_X = 4.6f;
            for (int i = 0; i < 4; i++)
            {
                cup[i].SetActive(true);
            }
            cup[4].SetActive(false);
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                cup[i].SetActive(true);
            }
            interval_X = 3.5f;
        }
    }

    void Update()
    {
        if (!ready)
        {
            if (input_bean)
            {
                bean.SetActive(true);
                bean.transform.position = Vector2.MoveTowards(bean.transform.position, new Vector2(startball * interval_X, -3.0f), 3 * Time.deltaTime);
                if (Vector2.Distance(bean.transform.position, new Vector2(startball * interval_X, -3.0f)) < 0.001f)
                {
                    Init_Game();
                }
            }
            else // start
            {
                if (gameState)
                {
                    if (choice)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            mousePtr = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            choice_Cup = Physics2D.Raycast(mousePtr, transform.forward);
                            if (choice_Cup)
                            {
                                SoundManager.PlaySFX(click_Clip);
                                //sd.Click(true);
                                int index = (int)choice_Cup.transform.position.x / (int)interval_X;

                                // 프리팹 비활성화
                                cup[index].SetActive(false);
                                Character_Active((int)State.ING, false);

                                open_cup.SetActive(true);
                                open_cup.transform.position = new Vector2(choice_Cup.transform.position.x, -2.2f);

                                if (ans[index] == 1) // 수정 필요
                                {
                                    bean.transform.position = new Vector2(choice_Cup.transform.position.x, -3.0f);

                                    // 프리팹 활성화
                                    bean.SetActive(true);
                                    Character_Active((int)State.WIN, true);

                                    gameClear = true;
                                    SoundManager.PlaySFX(success_Clip);
                                }
                                else
                                {
                                    bean_Sprouts.transform.position = new Vector2(choice_Cup.transform.position.x, -3.0f);

                                    // 프리팹 활성화
                                    bean_Sprouts.SetActive(true);
                                    Character_Active((int)State.LOSE, true);

                                    gameClear = false;
                                    SoundManager.PlaySFX(fail_Clip);
                                }

                                time = 0f;
                                choice = false;
                                gameState = false;
                            }
                        }
                        if (overTime)
                        {
                            for (int i = 0; i < numberofCup; i++)
                            {
                                if (ans[i] == 1)
                                {
                                    cup[i].SetActive(false);
                                    open_cup.SetActive(true);
                                    open_cup.transform.position = new Vector2(cup[i].transform.position.x, -2.2f);
                                    Debug.Log(cup[i].transform.position.x);
                                    bean.transform.position = new Vector2(cup[i].transform.position.x, -3.0f);
                                }
                            }

                            bean.SetActive(true);
                            Character_Active((int)State.ING, false);
                            Character_Active((int)State.LOSE, true);

                            overTime = false;
                            choice = false;
                            gameState = false;
                            gameClear = false;

                            time = 0f;
                            SoundManager.PlaySFX(fail_Clip);
                        }
                        time += Time.deltaTime;
                        timerText.text = $"남은 시간 : {(delayTime - time).ToString("F0")}";
                        if (delayTime - time < 0f)
                        {
                            overTime = true;
                        }
                    }
                    else
                    {
                        if (isMix)
                        {
                            if (swapCount == arr[currentStage - 1])
                            {
                                choice = true;
                            }
                            else
                            {
                                if (swapTime > delaySwap)
                                {
                                    SoundManager.PlaySFX(mix_Clip);
                                    Swap_Numbers();
                                    first_V = cup[first].transform.position;
                                    second_V = cup[second].transform.position;
                                    swapCount++;
                                }
                            }
                        }
                        else
                        {
                            Mix_Cup();
                            swapTime = 0f;
                        }
                    }
                }
                else
                {
                    // 성공과 실패 여부
                    if (gameClear) // 게임 클리어
                    {
                        SetGameWin();
                        // 다음 스테이지 버튼 활성화
                        if (Input.GetMouseButtonDown(0))
                        {
                            nextGame = true;
                            currentStage++;

                        }
                        // 다음 스테이지가 없을 시 비활성화
                        // 특정 스테이지에서 도감 얻기
                        // 업적 달성
                    }
                    else // 게임 오버
                    {
                        // 기존 생각한 방식은 게임 오버가 되었을 때 활성화 된 버튼(재시작, 나가기)에 따라 게임이 진행 or 종료됨
                        SetGameOver();
                        // 재시도 버튼 활성화
                        if (Input.GetMouseButtonDown(0))
                        {
                            nextGame = true;
                        }
                    }

                    if (nextGame)
                    {
                        nextGame = false;
                        gameState = true;
                        input_bean = true;

                        numberofCup = currentStage / 4 + 3;
                        startball = Random.Range(0, numberofCup);

                        for (int i = 0; i < 3; i++)
                        {
                            Character_Active(i, false);
                        }

                        Character_Active(0, true);
                        bean_Sprouts.SetActive(false);
                        bean.SetActive(true);
                        open_cup.SetActive(false);

                        CupActive();
                        bean.transform.position = new Vector3(startball * interval_X, 0f);
                        Init_Ready();
                    }
                }
            }
            swapTime += Time.deltaTime;
        }
    }

    void Character_Active(int st, bool sw)
    {
        dealer[st].SetActive(sw);
        audience1[st].SetActive(sw);
        audience2[st].SetActive(sw);
    }

    void Swap_Numbers()
    {
        first = second;
        while (first == second)
        {
            first = Random.Range(0, numberofCup);
            second = Random.Range(0, numberofCup);
        }
        swap = ans[first];
        ans[first] = ans[second];
        ans[second] = swap;
        isMix = false;
    }

    void Mix_Cup()
    {
        cup[first].transform.position = Vector2.MoveTowards(cup[first].transform.position, second_V, speed * Time.deltaTime);
        cup[second].transform.position = Vector2.MoveTowards(cup[second].transform.position, first_V, speed * Time.deltaTime);

        if (Vector2.Distance(cup[first].transform.position, second_V) < 0.001f)
        {
            isMix = true;
            cup[first].transform.position = first_V;
            cup[second].transform.position = second_V;
        }
    }

    private void SetGameWin()
    {
        //PlayerDataXref.instance.ClearGame(GAME_INDEX.Jack_And_Beanstalk, currentStage);
        //ui_Clear.SetActive(true);

        if (currentStage == PlayerDataXref.instance.GetTargetState_ToOpenNextChapter(GAME_INDEX.Jack_And_Beanstalk))
        {
            //paper_Img.SetActive(true);
            PlayerDataXref.instance.OpenChapter(GAME_INDEX.Jack_And_Beanstalk + 1);
        }
        else
        {
            //
        }
        if (arr.Length == currentStage)
        {
            PlayerDataXref.instance.SetAchieveSuccess(ACHEIVE_INDEX.JACK_AND_BEANSTALK_ALL_CLEAR);
            if (failcnt == 0)
            {
                PlayerDataXref.instance.SetAchieveSuccess(ACHEIVE_INDEX.GAINT_BEANSTALK);
            }
        }
    }
    private void SetGameOver()
    {
        //ui_Fail.SetActive(true);
        failcnt++;
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
        ready = false;

        yield return null;
    }
}