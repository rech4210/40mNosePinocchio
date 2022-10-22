using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Moru.UI;
using PD;

public enum SceneIndex
{
    MainPage = 0,
    Snow_White,         //백설공주
    Cinderella,         //신데렐라
    Pinocchio,          //피노키오
    Little_Mermaid,     //인어공주
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
                DontDestroyOnLoad(comp.gameObject);
                m_Instacne = comp;
                
            }
            return m_Instacne;
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
                var boolsArr = PlayerData.GetStageClearDataPerGame(PlayerData.instance.Cur_Game_Index);
                Debug.Log($"실행여부 확인");
                //StackUIManager.LobbyPage.GetComponent<LevelSelectUI>().SetUp(boolsArr, PlayerData.instance.Cur_Game_Index);
            }
            );
    }
}
