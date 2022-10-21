using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum GAME_INDEX
{
    Snow_White,         //백설공주
    Cinderella,         //신데렐라
    Pinocchio,          //피노키오
    Little_Mermaid,     //인어공주
    Jack_And_Beanstalk, //잭과 콩나무
    Tree_Little_Pigs,    //아기 돼지 삼형제
    None
}

[System.Serializable]
public partial class PlayerData
{
    #region instance
    private static PlayerData m_instance;
    public static PlayerData instance
    {
        get
        {
            if (m_instance == null)
            {
                int[] initValue = new int[6] { 10, 10, 10, 10, 10, 10 };
                m_instance = new PlayerData(initValue);
            }
            return m_instance;
        }
    }
    #endregion

    #region Events
    public delegate void OnClearGame(GAME_INDEX index, int StageNum);
    public static OnClearGame onClearGame;

    public delegate void OnValueChange();
    public OnValueChange onPlayerNameChange;
    public OnValueChange onPlayerTitleChange;

    #endregion

    #region Field
    [SerializeField] private string playerName;
    [SerializeField] private string playerTitle;

    [SerializeField] private GameObject PopUpUI;
    [ShowInInspector] private Dictionary<GAME_INDEX, Dictionary<int, int>> saveData;
    [SerializeField] private int[] stageCountPerGames;
    #endregion

    #region Properties
    public Dictionary<GAME_INDEX, Dictionary<int, int>> SaveData => saveData;
    public string PlayerName { get { return playerName; } set { playerName = value; onPlayerNameChange?.Invoke(); } }
    public string PlayerTitle { get { return playerTitle; } set { playerTitle = value; onPlayerTitleChange?.Invoke(); } }
    #endregion


    public PlayerData(int[] stageCountPerGames)
    {
        onClearGame += ClearGame;
        this.stageCountPerGames = stageCountPerGames;
        saveData = new Dictionary<GAME_INDEX, Dictionary<int, int>>();
    }

    #region Methods
    /// <summary>
    /// 게임을 클리어하면 콜백됩니다.
    /// </summary>
    /// <param name="index"></param>
    private void ClearGame(GAME_INDEX index, int stageNum)
    {
        //게임을 클리어했으므로 UI팝업시키기
        PlayerPrefs.SetInt(index.ToString() + stageNum.ToString(), 1);
        saveData[index][stageNum] = 1;
        Debug.Log($"{PlayerPrefs.GetInt(index.ToString() + stageNum.ToString())} // {saveData[index][stageNum] }");
    }

    /// <summary>
    /// 게임을 초기화합니다.
    /// </summary>
    public static void Initialize_GameData()
    {
        var instacne = PlayerData.instance;
        for (int i = 0; i < (int)GAME_INDEX.None; i++)
        {
            var key = (GAME_INDEX)i;
            Dictionary<int, int> stageSaveDic = new Dictionary<int, int>();
            for (int stageNum = 0; stageNum < instance.stageCountPerGames[i]; stageNum++)
            {
                PlayerPrefs.SetInt(key.ToString() + stageNum.ToString(), 0);
                stageSaveDic.Add(stageNum, 0);
            }
            instacne.saveData[(GAME_INDEX)i] = stageSaveDic;
        }
    }

    /// <summary>
    /// 플레이어 게임데이터를 불러옵니다.
    /// </summary>
    public static void Load_GameData()
    {
        var instacne = PlayerData.instance;
        for (int i = 0; i < (int)GAME_INDEX.None; i++)
        {
            var key = (GAME_INDEX)i;
            Dictionary<int, int> stageSaveDic = new Dictionary<int, int>();
            for (int stageNum = 0; stageNum < instacne.stageCountPerGames[i]; stageNum++)
            {
                if (
                    !PlayerPrefs.HasKey(key.ToString() + stageNum.ToString())
                   )
                {
                    PlayerPrefs.SetInt(key.ToString() + stageNum.ToString(), 0);
                    stageSaveDic.Add(stageNum, 0);
                }
                else
                {
                    int value = PlayerPrefs.GetInt(key.ToString() + stageNum.ToString());
                    stageSaveDic.Add(stageNum, value);
                }
            }
            instacne.saveData.Add((GAME_INDEX)i, stageSaveDic);
        }
    }

    /// <summary>
    /// 해당게임에서 해당 스테이지의 클리어 여부를 받아옵니다.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="stageNum"></param>
    public static bool GetStageClearDataPerGame(GAME_INDEX index, int stageNum)
    {
        var instacne = PlayerData.instance;
        int value = instance.saveData[index][stageNum];
        bool retVal = false;
        if(value > 0)
        {
            retVal = true;
        }
        else
        {
            retVal = false;
        }
        return retVal;
    }

    /// <summary>
    /// 해당 게임의 모든 스테이지의 클리어 여부를 배열로 받아옵니다.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static bool[] GetStageClearDataPerGame(GAME_INDEX index)
    {
        var instacne = PlayerData.instance;
        int count = instacne.saveData[index].Count;
        bool[] retVal = new bool[count];
        for (int i = 0; i < retVal.Length; i++)
        {
            int value = instance.saveData[index][i];
            if (value > 0)
            {
                retVal[i] = true;
            }
            else
            {
                retVal[i] = false;
            }
        }
        return retVal;
    }
    #endregion
}
