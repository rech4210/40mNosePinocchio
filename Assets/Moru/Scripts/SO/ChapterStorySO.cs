using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "ChapterStorySO", menuName = "MORU/ChapterStorySO")]
public class ChapterStorySO : ScriptableObject
{
    [SerializeField, OnInspectorInit(@"OnInit")]
    private List<ChapterStroy> chapterStroy;
    public List<ChapterStroy> _ChapterStroy => chapterStroy;

    private void OnInit()
    {
        if (chapterStroy == null || chapterStroy.Count != (int)GAME_INDEX.None)
        {
            chapterStroy = new List<ChapterStroy>();
            for (int i = 0; i < (int)GAME_INDEX.None; i++)
            {
                chapterStroy.Add(new ChapterStroy((GAME_INDEX)i));
            }
        }
    }
}

[System.Serializable]
public struct ChapterStroy
{
    [SerializeField, ReadOnly, LabelText("게임 인덱스")] private GAME_INDEX myIndex;
    [SerializeField, LabelText("챕터 이름")] private string chapterName;
    [SerializeField, LabelText("챕터 간단스토리")] private string chapterDesc;
    [SerializeField, LabelText("챕터 이미지")] private Sprite backGround;
    public ChapterStroy(GAME_INDEX index)
    {
        myIndex = index;
        chapterName = "";
        chapterDesc = "";
        backGround = null;
    }


    public GAME_INDEX MyIndex => myIndex;
    public string ChapterName { get => chapterName; }
    public string ChapterDesc { get => chapterDesc; }
    public Sprite BackGround { get => backGround; }
}
