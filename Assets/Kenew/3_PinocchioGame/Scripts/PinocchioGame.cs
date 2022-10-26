using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Pinocchio
{
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

        public bool isGamePlay;

        public AudioClip BGM;
        private void Update()
        {
            if (!isGamePlay) return;

            limite_time -= Time.deltaTime;
            if (limite_time <= 0f)
            {
                StageFailed();
            }
        }
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

            remainText.text = Mathf.Round(limite_time).ToString();
            remainText.color = limite_time <= 20 ? EmergencyColor : NormalColor;


        }

        private void Start()
        {
            stageDifficultyDesign = new List<StageDifficulty>();

            stageDifficultyDesign.Add(new StageDifficulty(3, 40));
            stageDifficultyDesign.Add(new StageDifficulty(4, 55));
            stageDifficultyDesign.Add(new StageDifficulty(5, 70));
            stageDifficultyDesign.Add(new StageDifficulty(6, 85));
            stageDifficultyDesign.Add(new StageDifficulty(7, 100));
            stageDifficultyDesign.Add(new StageDifficulty(8, 115));
            stageDifficultyDesign.Add(new StageDifficulty(9, 130));
            stageDifficultyDesign.Add(new StageDifficulty(10, 145));
            stageDifficultyDesign.Add(new StageDifficulty(11, 160));
            stageDifficultyDesign.Add(new StageDifficulty(12, 170));

            computerInfo = new PinocchioComputerInfo(1, 0.1f);
            currentStage = PlayerDataXref.instance.GetCurrentStage().StageNum;
            stageText.text = $"{currentStage + 1} Stage";
            enemy = FindObjectOfType<PinocchioEnemy>();
            player = FindObjectOfType<PinocchioPlayer>();

            // StartCoroutine(startDelayUI.StartDelay(3));
            player.GetPower = 30;
            enemy.OnMove(stageDifficultyDesign[currentStage].tickHitValue);
            SoundManager.PlayBGM(BGM);
            StageInit(currentStage);
            isGamePlay = false;
            StartCoroutine(StartTextAnim());
        }

        public Text startDelayUI;

        private void StageInit(int stageLevel)
        {
            enemy.StageInit(stageDifficultyDesign[stageLevel]);
            limite_time = 60;
        }

        public Text stageText;
        public void StageClear()
        {
            if (!isGamePlay) return;
            PlayerDataXref.instance.ClearGame(GAME_INDEX.Pinocchio, currentStage);
            WSceneManager.instance.OpenGameClearUI();

            //if(currentStage == PlayerDataXref.instance.GetTargetState_ToOpenNextChapter(GAME_INDEX.Pinocchio))
            //{
            //    PlayerDataXref.instance.OpenChapter(GAME_INDEX.Pinocchio + 1);
            //}
            //if (currentStage == PlayerDataXref.instance.GetMaxStageNumber(GAME_INDEX.Pinocchio)-1)
            //{
            //    PlayerDataXref.instance.ClearChapter(GAME_INDEX.Pinocchio);
            //    PlayerDataXref.instance.SetAchieveSuccess(ACHEIVE_INDEX.PINOCCHIO_ALL_CLEAR);
            //    if(failCount == 0)
            //    {
            //        PlayerDataXref.instance.SetAchieveSuccess(ACHEIVE_INDEX.SAWING_MASTER);
            //    }
            //}
            //else
            //{
            //    //currentStage++;
            //    //stageText.text = currentStage + " STAGE";
            //    //limite_time = 60;
            //    //PlayerDataXref.instance.OpenChapter(GAME_INDEX.Pinocchio + 1);
            //    //StageInit(currentStage);
            //    //SoundManagers.Instance.PlaySFX("StageClear");
            //}
            isGamePlay = false;
        }

        public void StageFailed()
        {
            failCount++;
            WSceneManager.instance.OpenGameFailUI();
            isGamePlay = false;
        }

        private IEnumerator StartTextAnim()
        {
            float waitTime = 2;
            float cur_T = 0;
            var startTextComponent = startDelayUI.GetComponent<Text>();
            startTextComponent.fontSize = 120;
            startDelayUI.text = "준비..";

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
            startDelayUI.text = "시작!";
            var cur_Color = startDelayUI.color;
            isGamePlay = true;
            while (cur_T < 2)
            {
                cur_Color.a = Mathf.Lerp(1, 0, cur_T*0.5f);
                startDelayUI.color = cur_Color;
                startTextComponent.fontSize = (int)Mathf.Lerp(300, 0, cur_T*0.5f);
                cur_T += Time.deltaTime;
                yield return null;
            }
            startDelayUI.gameObject.SetActive(false);
        }
    }
}