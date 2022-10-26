using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PD;

namespace Moru.UI
{
    public class AchievementUI : StackUIComponent
    {
        [SerializeField] Transform contents;
        [SerializeField] GameObject prefaps;
        [SerializeField] StackUIComponent PopUpUI;

        private void Start()
        {
            for (int i = 0; i < (int)ACHEIVE_INDEX.NONE; i++)
            {
                var obj = Instantiate(prefaps, contents);
                obj.GetComponent<AchieveContentUI>().
                    Init(
                    PlayerDataXref.pl.AchieveSo.AchieveResults[i]
                    , PopUpUI
                    );
            }
        }


    }
}