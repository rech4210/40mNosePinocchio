﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PD;

public class PlayerDataXref
{
    #region instance
    private static PlayerDataXref m_instance;
    public static PlayerDataXref instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new PlayerDataXref();
            }
            return m_instance;
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// 해당 게임의 스테이지을 클리어합니다.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="stageNum"></param>
    public void ClearGame(GAME_INDEX index, int stageNum)
    {
        PlayerData.onClearGame(index, stageNum);
    }

    /// <summary>
    /// 플레이어가 현재 선택한 게임과 스테이지 넘버를 받아옵니다.
    /// </summary>
    /// <returns></returns>
    public (GAME_INDEX index, int StageNum) GetCurrentStage()
    {
        var index = PlayerData.instance.Cur_Game_Index;
        var stage = PlayerData.instance.CurStageSelectedNum;
        return (index, stage);
    }

    /// <summary>
    /// 해당 챕터를 엽니다. (해금합니다. 클리어가 아닙니다.)
    /// </summary>
    /// <param name="index"></param>
    public void OpenChapter(GAME_INDEX index)
    {
        PlayerData.instance.onOpenChapter?.Invoke(index);
    }

    /// <summary>
    /// 해당 챕터를 클리어합니다.
    /// </summary>
    /// <param name="index"></param>
    public void ClearChapter(GAME_INDEX index)
    {
        PlayerData.instance.onClearChapter?.Invoke(index);
    }


    /// <summary>
    /// 해당 업적을 클리어시켜버립니다..
    /// </summary>
    /// <param name="index"></param>
    public void SetAchieveSuccess(ACHEIVE_INDEX index)
    {
        PlayerData.onClearAchieve(index);
    }

    /// <summary>
    /// 해당업적의 현재값에 addValue만큼 더합니다.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="addValue"></param>
    public void AddAchievementValue(ACHEIVE_INDEX index, int addValue)
    {
        PlayerData.onUpdateAchieve(index, addValue);
    }



    #endregion
}