using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterBtn : MonoBehaviour
{
    [SerializeField] GAME_INDEX myindex;
    public GAME_INDEX MyIndex { get => myindex; }
    [SerializeField] Image btn_Image;
    [SerializeField] Text chapterName;


    private void Awake()
    {
        PlayerData.instance.onOpenChapter += UpdateChapterButton;
    }


    public void OnClickStage()
    {
        PlayerData.instance.onSelectStage?.Invoke
            (
                PlayerData.GetStageClearDataPerGame(myindex), myindex
                );
    }

    public void UpdateChapterButton(GAME_INDEX myIndex)
    {
        if(!PlayerData.IsOpenChapter(myIndex))
        {
            btn_Image.enabled = false;
            chapterName.enabled = false;
        }
        else
        {
            btn_Image.enabled = true;
            chapterName.enabled = true;
        }
    }
}
