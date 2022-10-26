using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Moru.UI
{
    public class SelectedStageBtn : MonoBehaviour
    {
        [SerializeField] int count;
        [SerializeField] GAME_INDEX index;
        [SerializeField] Button myButton;

        [SerializeField] Image lock_Img;
        [SerializeField] Text stageNum_Text;
        [SerializeField] GameObject clearText;

        public void SetUp(int count, GAME_INDEX index, bool isClear, bool isOpen)
        {
            this.count = count;
            this.index = index;
            myButton = GetComponent<Button>();
            myButton.onClick.AddListener(
                () =>
                {
                    PlayerDataXref.pl.CurStageSelectedNum = count;
                    PlayerDataXref.pl.Cur_Game_Index = index;
                    WSceneManager.instance.MoveScene((SceneIndex)index + 1);
                }
                );
            myButton.interactable = isOpen;
            stageNum_Text.text = $"스테이지 {count+1}";
            clearText.SetActive(isClear);
            lock_Img.enabled = isOpen ? false: true;
        }
    }
}