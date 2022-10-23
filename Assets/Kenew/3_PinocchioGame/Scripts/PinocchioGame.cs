using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PinocchioComputerInfo
{
    public bool isFever = false;
    public int tickHitValue;
    public float tickHitDelay;

    public PinocchioComputerInfo(int tickHitValue, float tickHitDelay)
    {
        this.tickHitDelay = tickHitDelay;
        this.tickHitValue = tickHitValue;
    }
}

public class StageDifficulty
{
    public int tickHitValue;
    public int feverHitValue;

    public StageDifficulty(int tickHitValue, int feverHitValue)
    {
        this.feverHitValue = feverHitValue;
        this.tickHitValue = tickHitValue;
    }
}

public class PinocchioGame : MonoSingleton<PinocchioGame>
{
    private PinocchioComputerInfo computerInfo;
    private PinocchioEnemy enemy;
    private PinocchioPlayer player;

    private StageDifficulty stageDifficulty;
    public List<StageDifficulty> stageDifficultyDesign;
    
    private float currentTime;
    private readonly float TICK_DELAY = 1f;

    public float power;

    private bool isTime = false;

    private int currentStage = 1;
    
    public float limite_time = 60;
    
    public Text remainText;
    public Color EmergencyColor = new Color(255, 0, 0, 255);
    public Color NormalColor = new Color(58, 58, 58, 255);

    private bool isStart = false;
    private float startDelay = 3;

    private static int failCount = 0;

    private bool isGamePlay;
    private void FixedUpdate()
    {
        // if (isStart == false)
        // {
        //     startDelay -= Time.deltaTime;
        //     Debug.Log();
        //     if (startDelay <= 0f)
        //     {
        //         isStart = true;
        //         
        //         player.GetPower = 30;
        //         enemy.OnMove(stageDifficultyDesign[currentStage].tickHitValue);
        //         Time.timeScale = 1;
        //     }
        //     return;
        // }
        limite_time -= Time.deltaTime;
        remainText.text = Mathf.Round(limite_time).ToString();
        remainText.color = limite_time <= 20 ? EmergencyColor : NormalColor;
        
        if(limite_time <= 0f)
        {
            StageFailed();
        }
    }
    
    private void Start()
    {
        stageDifficultyDesign = new List<StageDifficulty>();
        
        stageDifficultyDesign.Add(new StageDifficulty(3,  40));
        stageDifficultyDesign.Add(new StageDifficulty(4,  55));
        stageDifficultyDesign.Add(new StageDifficulty(5,  70));
        stageDifficultyDesign.Add(new StageDifficulty(6,  85));
        stageDifficultyDesign.Add(new StageDifficulty(7, 100));
        stageDifficultyDesign.Add(new StageDifficulty(8, 115));
        stageDifficultyDesign.Add(new StageDifficulty(9, 130));
        stageDifficultyDesign.Add(new StageDifficulty(10, 145));
        stageDifficultyDesign.Add(new StageDifficulty(11, 160));
        stageDifficultyDesign.Add(new StageDifficulty(12, 170));

        computerInfo = new PinocchioComputerInfo(1, 0.1f);
        currentStage = PlayerDataXref.instance.GetCurrentStage().StageNum;
        
        enemy = FindObjectOfType<PinocchioEnemy>();
        player = FindObjectOfType<PinocchioPlayer>();
        
        // StartCoroutine(startDelayUI.StartDelay(3));
        player.GetPower = 30;
        enemy.OnMove(stageDifficultyDesign[currentStage].tickHitValue);
        SoundManagers.Instance.PlayBGM("6BGM");
        StageInit(currentStage);

        isGamePlay = true;
    }

    public StartDelayUI startDelayUI;

    private void StageInit(int stageLevel)
    {
        enemy.StageInit(stageDifficultyDesign[stageLevel]);
        limite_time = 60;
    }

    public Text stageText;
    public void StageClear()
    {
        if (!isGamePlay) return;
        WSceneManager.instance.OpenGameClearUI();

        if(currentStage == PlayerDataXref.instance.GetTargetState_ToOpenNextChapter(GAME_INDEX.Pinocchio))
        {
            PlayerDataXref.instance.OpenChapter(GAME_INDEX.Pinocchio + 1);
        }
        if (currentStage == PlayerDataXref.instance.GetMaxStageNumber(GAME_INDEX.Pinocchio))
        {
            PlayerDataXref.instance.SetAchieveSuccess(ACHEIVE_INDEX.PINOCCHIO_ALL_CLEAR);
            if(failCount == 0)
            {
                PlayerDataXref.instance.SetAchieveSuccess(ACHEIVE_INDEX.SAWING_MASTER);
            }
        }
        else
        {
            //currentStage++;
            //stageText.text = currentStage + " STAGE";
            //limite_time = 60;
            //PlayerDataXref.instance.OpenChapter(GAME_INDEX.Pinocchio + 1);
            //StageInit(currentStage);
            //SoundManagers.Instance.PlaySFX("StageClear");
        }
        isGamePlay = false;
    }

    public void StageFailed()
    {
        failCount++;
        WSceneManager.instance.OpenGameClearUI();
    }
}
