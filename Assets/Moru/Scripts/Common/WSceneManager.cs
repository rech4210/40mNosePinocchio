﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Moru.UI;
using PD;

public enum SceneIndex
{
    MainPage = 0,
    Pinocchio,          //피노키오
    Snow_White,         //백설공주
    Little_Mermaid,     //인어공주
    Cinderella,         //신데렐라
    Jack_And_Beanstalk, //잭과 콩나무
    Tree_Little_Pigs,    //아기 돼지 삼형제
    None
}

public class WSceneManager : MonoBehaviour
{
    private static WSceneManager m_Instacne;
    public static WSceneManager instance
    {
        get
        {
            if (m_Instacne == null)
            {
                m_Instacne = FindObjectOfType<WSceneManager>(true);
            }
            if (m_Instacne == null)
            {
                var obj = new GameObject(typeof(WSceneManager).Name);
                var comp = obj.AddComponent<WSceneManager>();
                obj.AddComponent<SoundManager>();
                DontDestroyOnLoad(comp.gameObject);
                m_Instacne = comp;

            }
            return m_Instacne;
        }
    }

    [SerializeField] private GameObject optionUI;
    public GameObject OptionUI
    {
        get
        {
            if (optionUI == null)
            {
                var obj = Resources.Load<GameObject>("OptionCanvas");
                optionUI = Instantiate(obj, this.transform);
                optionUI.SetActive(false);
            }
            return optionUI;
        }
    }

    [SerializeField] private GameObject clearUI;
    public GameObject ClearUI
    {
        get
        {
            if (clearUI == null)
            {
                var obj = Resources.Load<GameObject>("ClearUI");
                clearUI = Instantiate(obj, this.transform);
                clearUI.SetActive(false);
            }
            return clearUI;
        }
    }
    [SerializeField] private GameObject failUI;
    public GameObject FailUI
    {
        get
        {
            if (failUI == null)
            {
                var obj = Resources.Load<GameObject>("FailUI");
                failUI = Instantiate(obj, this.transform);
                failUI.SetActive(false);
            }
            return failUI;
        }
    }



    public void Awake()
    {
        if (!this.GetComponent<SoundManager>())
        {
            transform.gameObject.AddComponent<SoundManager>();
        }
    }

    /// <summary>
    /// 씬을 재로드합니다.
    /// </summary>
    public void ReLoad_ThisScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        MoveScene(index);
    }
    /// <summary>
    /// 씬을 재로드하며 스테이지를 증가시킵니다.
    /// </summary>
    public void ReLoad_ThisScene_WtihStageUp()
    {
        PD.PlayerData.instance.CurStageSelectedNum++;
        int index = SceneManager.GetActiveScene().buildIndex;
        MoveScene(index);
    }


    public void MoveScene(SceneIndex index)
    {
        SceneManager.LoadScene((int)index);
    }
    public void MoveScene(int index)
    {
        if (index >= (int)SceneIndex.MainPage && index < (int)SceneIndex.None)
        {
            SceneManager.LoadScene(index);
        }
        else
        {
            Debug.Log($"{index} : 유효하지 않은 인덱스, 유효값을 넣어주세요. \n" +
                $"인덱스 범위 : {(int)SceneIndex.MainPage } ~ {(int)SceneIndex.None - 1}");
            return;
        }
    }

    public void MoveChapterSelectPage()
    {
        MoveScene(SceneIndex.MainPage);
        StackUIManager.onLoadEvent = new StackUIManager.OnLoadEvent();
        StackUIManager.onLoadEvent.AddListener(
            () =>
            {
                StackUIManager.GoToTargetUIComponent(StackUIManager.ChapterPage);
            }
            );
    }
    public void MoveStageSeletedPage()
    {
        MoveScene(SceneIndex.MainPage);
        StackUIManager.onLoadEvent = new StackUIManager.OnLoadEvent();
        StackUIManager.onLoadEvent.AddListener(
            () =>
            {
                StackUIManager.GoToTargetUIComponent(StackUIManager.LobbyPage);
                var boolsArr = PlayerDataXref.pl.GetStageClearDataPerGame(PlayerData.instance.Cur_Game_Index);
                Debug.Log($"실행여부 확인");
                //StackUIManager.LobbyPage.GetComponent<LevelSelectUI>().SetUp(boolsArr, PlayerData.instance.Cur_Game_Index);
            }
            );
    }

    /// <summary>
    /// 게임 클리어 UI를 오픈
    /// </summary>
    public void OpenGameClearUI()
    {
        ClearUI.SetActive(true);
    }

    /// <summary>
    /// 게임 실패 UI를 오픈
    /// </summary>
    public void OpenGameFailUI()
    {
        FailUI.SetActive(true);
    }

    private void Update()
    {
        Time.timeScale = OptionUI.activeInHierarchy ? 0 : 1;
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                bool result = OptionUI.activeInHierarchy ? false : true;
                OptionUI.SetActive(result);
            }
        }
        else
        {
            OptionUI.SetActive(false);
        }
    }
}
