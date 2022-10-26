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
        [SerializeField] ChangeTitlePopUp popUpUI;
        [SerializeField] List<GameObject> content;
        void Start()
        {
            if(popUpUI == null)
            {
                popUpUI = FindObjectOfType<ChangeTitlePopUp>(true);
            }
            for (int i = 0; i < (int)ACHEIVE_INDEX.NONE; i++)
            {
                var obj = Instantiate(prefaps, contents);
                content.Add(obj);
            }
            Init();
        }

        void Init()
        {
            for (int i = 0; i < (int)ACHEIVE_INDEX.NONE; i++)
            {
                content[i].GetComponent<PlayerTitleContentUI>().Init(PlayerDataXref.pl.AchieveSo.AchieveResults[i], popUpUI);
            }
        }

    }
}