using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;

public enum NowGameState
{
    GameReady,
    Gaming,
    GameEnd,
    GameStop
}

public class SnowWhiteGameManager : MonoBehaviour
{
    private static int FailCount = 0;

    [SerializeField]
    [Tooltip("���� �ؽ�Ʈ")]
    private Text startText;

    [SerializeField]
    [Tooltip("���� ĵ���� ������")]
    private Canvas startCanvas;

    [SerializeField]
    [Tooltip("���� �ð� �ؽ�Ʈ")]
    private Text timerText;

    [SerializeField]
    [Tooltip("���� ������Ʈ")]
    private GameObject correctObj;

    [SerializeField]
    [Tooltip("���� ��� ǥ�� �̹���")]
    private Image correctAppleImage;

    [SerializeField] Transform wrongApplesParent;

    [SerializeField, LabelText("��� ��������Ʈ")] List<Sprite> apples;

    [SerializeField, BoxGroup("���̵� ������"), LabelText("���ѽð� ����Ʈ")] 
    private float[] limits_List = new float[15];
    [SerializeField, BoxGroup("���̵� ������"), LabelText("�߸��� ��������Ʈ ��")]
    private int[] wrong_ObjectsCount = new int[15];

    [SerializeField]
    private float cur_limitTime;

    
    private int CurrentStage;

    private NowGameState nowGameState;

    public AudioClip BGMs;


    // Start is called before the first frame update
    void Start()
    {
        CurrentStage = PlayerDataXref.instance.GetCurrentStage().StageNum;
        nowGameState = NowGameState.GameReady;
        SoundManager.PlayBGM(BGMs);
        //���ѽð� ������Ʈ
        cur_limitTime = limits_List[CurrentStage];
        timerText.text = $"���� �ð� {((int)cur_limitTime / 60):D2} : {((int)cur_limitTime % 60):D2}";
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
            cur_limitTime -= Time.deltaTime;
            timerText.text = $"���� �ð� {((int)cur_limitTime / 60):D2} : {((int)cur_limitTime % 60):D2}";
            if (cur_limitTime <= 0)
            {

                cur_limitTime = 0;

                nowGameState = NowGameState.GameEnd;


                timerText.text = $"���� �ð� 00 : 00";
                FailCount++;
                WSceneManager.instance.OpenGameFailUI();
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
                nowGameState = NowGameState.GameEnd;

                //Moru
                PlayerDataXref.instance.ClearGame(GAME_INDEX.Snow_White, CurrentStage);
                WSceneManager.instance.OpenGameClearUI();


                //���̻� �ʿ����� �ʽ��ϴ�.
                ////����é���� ���ϴ�. �ٸ��е鵵 �̷��� ���ֽø� �ǿ�
                //if (CurrentStage == PlayerDataXref.instance.GetTargetState_ToOpenNextChapter(GAME_INDEX.Snow_White))
                //{
                //    //targetClearUI.gameObject.SetActive(true);
                //    PlayerDataXref.instance.OpenChapter(GAME_INDEX.Snow_White + 1);
                //}
                //else
                //{
                //    //targetClearUI.gameObject.SetActive(false);
                //}

                ////��Ŭ����  & 1ȸ�� �������� �ʰ� Ŭ����� ���� �̺�Ʈ ����
                //if (CurrentStage == PlayerDataXref.instance.GetMaxStageNumber(GAME_INDEX.Snow_White) - 1)
                //{
                //    PlayerDataXref.instance.SetAchieveSuccess(ACHEIVE_INDEX.SNOW_WHITE_ALL_CLEAR);
                //    PlayerDataXref.instance.ClearChapter(GAME_INDEX.Snow_White);
                //    if (FailCount == 0)
                //    {
                //        PlayerDataXref.instance.SetAchieveSuccess(ACHEIVE_INDEX.APPLE_SOMMELIER);
                //    }
                //}



            }
        }
    }

    private void StartSetting()
    {
        int randSpritsIndex = Random.Range(0, apples.Count);
        var targetSprite = apples[randSpritsIndex];

        correctAppleImage.sprite = targetSprite;
        apples.Remove(targetSprite);

        int target_intanceCount = wrong_ObjectsCount[CurrentStage];
        target_intanceCount++; //���� �������ڴ� �� ���� ����
        int correct_Obj_Index = Random.Range(0, target_intanceCount);

        float minX = -7.5f; //X �ּ���ǥ
        float maxX = 7.5f;  //x �ִ���ǥ

        float minY = -3.5f; //y�ּ���ǥ
        float maxY = 3.5f;  //y�ִ���ǥ

        float Xlength = maxX - minX;        //x�� ����
        float Ylength = maxY - minY;        //y�� ����

        float XYArea = Xlength * Ylength;   //�ش籸���� ����
        float elementXYArea = XYArea / target_intanceCount; //�� ��ü�� ����

        float xElementPosSqr = (Xlength / Ylength) * elementXYArea;
        float xElementLength = Mathf.Sqrt(xElementPosSqr);

        float yElementLength = elementXYArea / xElementLength;

        Debug.Log($"X:Y�� ���� : {Xlength}:{Ylength} \n" +
            $"���� : ����� ���� : {XYArea} : {elementXYArea}\n" +
            $"X���� : {xElementPosSqr}\n" +
            $"X':Y'�� ���� : {xElementLength} : {yElementLength}");

        float curXPos = minX;
        float curYPos = minY;

        List<GameObject> Apples_GameObject = new List<GameObject>();
        for(float x = minX; x < maxX; x+= xElementLength)
        {
            for(float y= minY; y < maxY; y+= yElementLength)
            {
                float randomXoffset = Random.Range(-xElementLength*0.3f, xElementLength*0.3f);
                float randomYoffset = Random.Range(-yElementLength * 0.3f, yElementLength * 0.3f);

                float targetxPos = x + (xElementLength / 2)+randomXoffset;
                float targetyPos = y + (yElementLength / 2)+randomYoffset;

                float randomScaleOffset = Random.Range(0.7f, 1.05f);
                if(targetxPos < maxX && targetyPos < maxY)
                {
                    if((targetxPos < -3.5f) && (targetyPos > 2.5f))
                    {

                    }
                    else
                    {
                        var obj = new GameObject($"����� ��ġ�׽�Ʈ {x} {y}");
                        obj.transform.SetParent(wrongApplesParent);
                        int spriteIndex = Random.Range(0, apples.Count);
                        obj.AddComponent<SpriteRenderer>().sprite = apples[spriteIndex];
                        obj.transform.localScale = new Vector3(0.5f, 0.5f)* randomScaleOffset;
                        obj.transform.position = new Vector3(targetxPos, targetyPos, 0);
                        Apples_GameObject.Add(obj);
                    }
                }
            }
        }

        //Old
        //correctObj.transform.position = new Vector3(Random.Range(-8, 9), Random.Range(-3, 4));

        //if (correctObj.transform.position.x > 4 || correctObj.transform.position.x < -2)
        //{
        //    correctObj.transform.position = new Vector3(Random.Range(-2, 4), correctObj.transform.position.y);
        //}

        //new
        int correctIndex = Random.Range(0,Apples_GameObject.Count);
        var correct_obj = Apples_GameObject[correctIndex];
        correct_obj.GetComponent<SpriteRenderer>().sprite = targetSprite;
        correct_obj.GetComponent<SpriteRenderer>().sortingOrder = 10;
        correct_obj.tag = "CorrectApple";
        var collider = correct_obj.AddComponent<CircleCollider2D>();
        


        StartCoroutine(StartTextAnim());
    }

    private IEnumerator StartTextAnim()
    {
        float waitTime = 2;
        float cur_T = 0;
        var startTextComponent = startText.GetComponent<Text>();
        startTextComponent.fontSize = 120;
        startText.text = "�غ�..";

        WaitForSecondsRealtime textAnimDelay = new WaitForSecondsRealtime(0.5f);
        while (cur_T < waitTime)
        {
            cur_T += Time.deltaTime;
            yield return null;
        }
        cur_T = 0;
        for (int i = 3; i > 0; i--)
        {
            startTextComponent.fontSize = 300;
            startTextComponent.text = i.ToString();
            while (cur_T < 1)
            {
                startTextComponent.fontSize = (int)Mathf.Lerp(300, 0, cur_T);
                cur_T += Time.deltaTime;
                yield return null;
            }
            cur_T = 0;
        }
        startTextComponent.fontSize = 300;
        startText.text = "����!";
        var cur_Color = startText.color;
        nowGameState = NowGameState.Gaming;
        startCanvas.sortingOrder = -100;
        while (cur_T < 2)
        {
            cur_Color.a = Mathf.Lerp(1, 0, cur_T * 0.5f);
            startText.color = cur_Color;
            startTextComponent.fontSize = (int)Mathf.Lerp(300, 0, cur_T * 0.5f);
            cur_T += Time.deltaTime;
            yield return null;
        }
        startText.gameObject.SetActive(false);
    }
}
