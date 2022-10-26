using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Moru.UI
{
    public class GameExplainPopUp : StackUIComponent
    {
        [SerializeField] Image image;

        public override void Show()
        {
            base.Show();
            var index = PlayerDataXref.pl.Cur_Game_Index;
            image.sprite = PlayerDataXref.pl.ChapterStorySO._ChapterStroy[(int)index].ExplainSprite;
        }
    }
}

