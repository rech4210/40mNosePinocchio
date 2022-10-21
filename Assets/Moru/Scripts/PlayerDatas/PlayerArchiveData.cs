using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum ACHEIVE_INDEX
{
    /// <summary>
    /// 모든 게임들의 모든 스테이지를 클리어할 경우 클리어입니다.
    /// </summary>
    ALL_CLEAR,
    /// <summary>
    /// 테스트용으로 하나 생성
    /// </summary>
    TEST_ACHIEVE,
    /// <summary>
    /// 가장 마지막 인덱스 번호입니다.
    /// </summary>
    NONE,

}

public partial class PlayerData
{
    #region Const
    private const string isAchieve = "isAchieve";
    private const string curValue = "cur_Value";
    private const string maxValue = "max_Value";
    #endregion

    #region Field
    [SerializeField] private AchieveAndTitle achieveAndTitle;
    [ShowInInspector] private Dictionary<ACHEIVE_INDEX, int> isAchievement;
    [ShowInInspector] private Dictionary<ACHEIVE_INDEX, AchieveResult> cur_AchievementValue;

    #endregion

    #region Events
    public delegate void OnClearAchieve(ACHEIVE_INDEX index);
    public static OnClearAchieve onClearAchieve;

    public delegate void OnUpdateAchieve(ACHEIVE_INDEX index, int value);
    public static OnUpdateAchieve onUpdateAchieve;

    #endregion

    #region Methods

    public static void Load_PlayerAchieve()
    {
        var instance = PlayerData.instance;
        instance.achieveAndTitle = Resources.Load<AchieveAndTitle>("AchieveTitle");
        var resultList = instance.achieveAndTitle.AchieveResults;
        for (int i = 0; i < resultList.Count; i++)
        {
            //isAchievement 체크
            if(!PlayerPrefs.HasKey(
                resultList[i].MyIndex.ToString()+ isAchieve)
                )
            {
                PlayerPrefs.SetInt(
                    resultList[i].MyIndex.ToString() + isAchieve, 0
                    );
                instance.isAchievement.Add(resultList[i].MyIndex, 0);
            }
            else
            {
                int value = PlayerPrefs.GetInt(
                    resultList[i].MyIndex.ToString() + isAchieve
                    );
                instance.isAchievement.Add(resultList[i].MyIndex, value);
            }

            //현재 밸류 체크
            instance.cur_AchievementValue.Add(resultList[i].MyIndex, resultList[i]);
            var resultList_Value = resultList[i].Cur_AchievementCondition;
            resultList_Value = PlayerPrefs.GetInt(resultList[i].MyIndex.ToString() + curValue, 0);
        }
    }

    /// <summary>
    /// 업적의 밸류를 업데이트시켜주는 콜백메서드
    /// </summary>
    /// <param name="index"></param>
    /// <param name="value"></param>
    private void onUpdateAchieveCallBack(ACHEIVE_INDEX index, int value)
    {
        AchieveResult result = cur_AchievementValue[index];
        result.Cur_AchievementCondition += value;
        cur_AchievementValue[index] = result;
        var comp = cur_AchievementValue[index];
        var curValue = comp.Cur_AchievementCondition;

        PlayerPrefs.SetInt(index.ToString() + curValue, comp.Cur_AchievementCondition);

        if(comp.Cur_AchievementCondition >= comp.Target_AchievementCondition)
        {
            result.Cur_AchievementCondition = result.Target_AchievementCondition;
            cur_AchievementValue[index] = result;
            PlayerPrefs.SetInt(index.ToString() + curValue, result.Cur_AchievementCondition);
            onClearAchieve?.Invoke(index);
        }
        else
        {
            //업적 진행정도 띄우기
        }

        Debug.Log($"업적 진행도 : 딕셔너리 = {result.Cur_AchievementCondition} / {result.Target_AchievementCondition}" +
            $"\n플레이어 프랩스 = {PlayerPrefs.GetInt(index.ToString() + curValue)}");
    }

    private void onClearAchieveCallBack(ACHEIVE_INDEX index)
    {
        isAchievement[index] = 1;
        PlayerPrefs.SetInt(index.ToString() + isAchieve, 1);
        //업적을 달성했습니다 팝업띄우기


        Debug.Log($"업적 달성현황 : {cur_AchievementValue[index].AchieveName} 업적을 클리어해 {cur_AchievementValue[index].Title} 칭호를 획득했다!" +
            $"\n딕셔너리 = {isAchievement[index]}" +
    $"\n플레이어 프랩스 = {PlayerPrefs.GetInt(index.ToString() + isAchieve)}");
    }


    #endregion
}
