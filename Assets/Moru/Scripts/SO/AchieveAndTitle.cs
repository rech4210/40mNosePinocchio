using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "ArchiveTitle", menuName = "MORU/AchieveTItleSO")]
public class AchieveAndTitle : ScriptableObject
{
    [SerializeField, OnInspectorInit(@"OnInit")]
    private List<AchieveResult> achieveResults;
    public List<AchieveResult> AchieveResults => achieveResults;

    private void OnInit()
    {
        if (achieveResults == null || achieveResults.Count != (int)ACHEIVE_INDEX.NONE)
        {
            List<AchieveResult> dummy = new List<AchieveResult>();
            if (achieveResults != null)
            {
                dummy = achieveResults;
            }
            achieveResults = new List<AchieveResult>();
            for (int i = 0; i < (int)ACHEIVE_INDEX.NONE; i++)
            {
                achieveResults.Add(new AchieveResult((ACHEIVE_INDEX)i));
            }
            if (dummy != null)
            {
                for (int i = 0; i < dummy.Count; i++)
                {
                    achieveResults[i].RecoverFieldValue(dummy[i].Target_AchievementCondition, dummy[i].AchieveName, dummy[i].Title, dummy[i].Icon);
                }
            }
        }
    }
}

[System.Serializable]
public struct AchieveResult
{

    [SerializeField, ReadOnly, LabelText("업적 인덱스")] private ACHEIVE_INDEX myIndex;
    [SerializeField, LabelText("현재 업적수치")] private int cur_achievementConditon;
    [SerializeField, LabelText("목표 업적수치")] private int target_achievementConditon;
    [SerializeField, LabelText("업적 이름")] private string achieveName;
    [SerializeField, LabelText("업적 내용")] private string achieveDesc;
    [SerializeField, LabelText("보상 칭호")] private string title;
    [SerializeField, LabelText("아이콘")] private Sprite icon;
    public AchieveResult(ACHEIVE_INDEX index)
    {
        myIndex = index;
        cur_achievementConditon = 0;
        target_achievementConditon = 0;
        achieveName = "";
        achieveDesc = "";
        title = "";
        icon = null;
    }

    public void RecoverFieldValue(int target_achieve, string achieveName, string title, Sprite icon)
    {
        target_achievementConditon = target_achieve;
        this.achieveName = achieveName;
        this.icon = icon;
    }

    public ACHEIVE_INDEX MyIndex => myIndex;
    public int Cur_AchievementCondition { get => cur_achievementConditon; set => cur_achievementConditon = value; }
    public int Target_AchievementCondition { get => target_achievementConditon; set => target_achievementConditon = value; }
    public string AchieveName { get => achieveName; }
    public string AchieveDesc { get => achieveDesc; }
    public string Title { get => title; }
    public Sprite Icon { get => icon; }
}
