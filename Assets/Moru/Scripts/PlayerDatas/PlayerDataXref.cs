using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PD;

/// <summary>
/// 플레이어 데이터 -> 외부접근용 리팩토링
/// </summary>
public class PlayerDataXref : MonoBehaviour
{
    #region instance
    private static PlayerDataXref m_instance;
    public static PlayerDataXref instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<PlayerDataXref>(true);
            }
            if( m_instance == null)
            {
                var obj = new GameObject(typeof(PlayerDataXref).Name);
                var comp = obj.AddComponent<PlayerDataXref>();
                comp.Awake();
                m_instance = comp;
                
            }

            return m_instance;
        }
    }
    private PlayerData _pl;
    public static PlayerData pl
    {
        get
        {
            return PlayerDataXref.instance._pl;

        }
    }


    #endregion

    #region Methods

    /// <summary>
    /// 참조와 동시에 PlayerData를 인스턴스화 및 초기화하도록 설정
    /// </summary>
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _pl = new PlayerData();
        _pl.OnInitialized();
        OpenChapter(0);
        Debug.Log($"챕터열기 실행");
    }

    /// <summary>
    /// 해당 게임의 스테이지을 클리어합니다.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="stageNum"></param>
    public void ClearGame(GAME_INDEX index, int stageNum)
    {
        _pl.SetGameStageClear(index, stageNum, 1);
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
    /// 플레이어의 스테이지정보를 매개변수값으로 변경합니다.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="stageNum"></param>
    public void SetCurrentStage(GAME_INDEX index, int stageNum)
    {
        PlayerData.instance.del_SetPlayerSelectedStage?.Invoke(index, stageNum);
    }


    /// <summary>
    /// 해당 챕터를 엽니다. (해금합니다. 클리어가 아닙니다.)
    /// </summary>
    /// <param name="index"></param>
    public void OpenChapter(GAME_INDEX index)
    {
        pl.OpenChapter(index);
        if (index == GAME_INDEX.None)
        {   
            //업적
            SetAchieveSuccess(ACHEIVE_INDEX.END_MAKER);
            return;
        }
    }

    /// <summary>
    /// 해당 챕터를 클리어합니다.
    /// </summary>
    /// <param name="index"></param>
    public void ClearChapter(GAME_INDEX index)
    {
        _pl.ClearChapter(index);
    }


    /// <summary>
    /// 해당 업적을 클리어시켜버립니다..
    /// </summary>
    /// <param name="index"></param>
    public void SetAchieveSuccess(ACHEIVE_INDEX index)
    {
    }

    /// <summary>
    /// 해당업적의 현재값에 addValue만큼 더합니다.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="addValue"></param>
    public void AddAchievementValue(ACHEIVE_INDEX index, int addValue)
    {
    }

    /// <summary>
    /// 컷씬을 읽었는지 여부를 받아옵니다.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool IsGetCutScene(CUTSCENE_INDEX index)
    {
        return _pl.GetCutScene(index);
    }

    /// <summary>
    /// 컷씬을 읽은 상태로 저장합니다.
    /// </summary>
    /// <param name="index"></param>
    public void SetToCutSceneRead(CUTSCENE_INDEX index)
    {
        _pl.SetCutScene(index);
    }

    /// <summary>
    /// 해당 매개변수의 게임의 다음챕터 오픈까지의 스테이지 넘버 번호를 받아옵니다.
    /// 없어질 메서드
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int GetTargetState_ToOpenNextChapter(GAME_INDEX index)
    {
        return 1;//(PlayerData.instance.TargetStage[(int)index]) - 1;
    }

    /// <summary>
    /// 해당 게임의 최대스테이지값을 받아옵니다.
    /// 마찬가지로 사용되지 않을 예정입니다.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int GetMaxStageNumber(GAME_INDEX index)
    {
        return 1;//PlayerData.instance.StageCountPerGames[(int)index];
    }
    #endregion
}
