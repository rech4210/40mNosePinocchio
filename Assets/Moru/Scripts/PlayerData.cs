using System.Collections;
using System.Collections.Generic;
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
public class PlayerData
{
    #region instance
    private static PlayerData m_instance;
    public static PlayerData instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new PlayerData();
            }
            return m_instance;
        }
    }
    #endregion

    #region Events
    public delegate void OnClearGame(GAME_INDEX index);
    public static OnClearGame onClearGame;
    #endregion

    #region Field
    [SerializeField] private GameObject PopUpUI;
    [SerializeField] private Dictionary<GAME_INDEX, int> saveData;
    #endregion

    #region Properties
    public Dictionary<GAME_INDEX, int> SaveData => saveData;
    #endregion


    public PlayerData()
    {
        onClearGame += ClearGame;
        saveData = new Dictionary<GAME_INDEX, int>();
    }

    #region Methods
    /// <summary>
    /// 게임을 클리어하면 콜백됩니다.
    /// </summary>
    /// <param name="index"></param>
    private void ClearGame(GAME_INDEX index)
    {
        //게임을 클리어했으므로 UI팝업시키기
        PlayerPrefs.SetInt(index.ToString(), 1);
        saveData[index] = 1;
        Debug.Log($"{PlayerPrefs.GetInt(index.ToString())} // {saveData[index] }");
    }

    /// <summary>
    /// 게임을 초기화합니다.
    /// </summary>
    public void Initialize_GameData()
    {
        for (int i = 0; i < (int)GAME_INDEX.None; i++)
        {
            var key = (GAME_INDEX)i;
            PlayerPrefs.SetInt(key.ToString(), 0);
            saveData.Add((GAME_INDEX)i, 0);
        }
    }

    /// <summary>
    /// 플레이어 게임데이터를 불러옵니다.
    /// </summary>
    public void Load_GameData()
    {
        for (int i = 0; i < (int)GAME_INDEX.None; i++)
        {
            var key = (GAME_INDEX)i;
            if (
                !PlayerPrefs.HasKey(key.ToString())
               )
            {
                PlayerPrefs.SetInt(key.ToString(), 0);
                saveData.Add((GAME_INDEX)i, 0);
            }

            else
            {
                int value = PlayerPrefs.GetInt(key.ToString());
                saveData.Add((GAME_INDEX)i, value);
            }
        }
    }
    #endregion
}
