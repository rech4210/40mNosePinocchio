using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementUI : MonoBehaviour
{
    [SerializeField] Transform contents;
    [SerializeField] GameObject prefaps;
    [SerializeField] StackUIComponent popUpUI;

    private void Start()
    {
        for (int i = 0; i < (int)ACHEIVE_INDEX.NONE; i++)
        {
            var obj = Instantiate(prefaps, contents);
            obj.GetComponent<AchieveContentUI>().Init(PlayerData.instance.Cur_AchievementValue[(ACHEIVE_INDEX)i], popUpUI);
        }
    }
}
