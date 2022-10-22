using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PD;

namespace Moru.UI
{
    public class TitleSelectUI : MonoBehaviour
    {
        [SerializeField] Transform contents;
        [SerializeField] GameObject prefaps;
        [SerializeField] StackUIComponent popUpUI;

        void Start()
        {
            for (int i = 0; i < (int)ACHEIVE_INDEX.NONE; i++)
            {
                var obj = Instantiate(prefaps, contents);
                obj.GetComponent<PlayerTitleUI>().Init(PlayerData.instance.Cur_AchievementValue[(ACHEIVE_INDEX)i], popUpUI);
            }
        }


    }
}