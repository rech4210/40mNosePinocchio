using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using PD;


namespace Moru.UI
{
    public class PlayerTitleContentUI : MonoBehaviour
    {
        //private delegate void OnClick_NoRet_NoParam(AchieveContentUI btn);
        //private static event OnClick_NoRet_NoParam onClick;
        //bool isOpen = false;

        private Button originBtn;
        [BoxGroup("앞면"), LabelText("업적 이름"), SerializeField] private Text playerTitleText;
        //착용중 이미지 효과
        private AchieveResult myResult;
        ChangeTitlePopUp targetComp;
        [SerializeField] private Sprite lockSprite;
        [SerializeField] private Sprite openSprite;
        [SerializeField] private bool isEquip => PlayerDataXref.pl.PlayerTitle == myResult.Title ? true : false;

        bool isInit = false;
        private void OnDestroy()
        {
            if (true)
            {
                PlayerDataXref.pl.onPlayerTitleChange -= Refresh;
            }
        }

        public void Init(AchieveResult result, ChangeTitlePopUp targetComp)
        {
            originBtn = GetComponent<Button>();
            myResult = result;
            this.targetComp = targetComp;
            bool isHave = PlayerDataXref.pl.GetIsReward_Achieve(result.MyIndex);
            originBtn.onClick.RemoveAllListeners();
            if (isHave)
            {
                originBtn.interactable = true;
                playerTitleText.text = result.Title;
                originBtn.GetComponent<Image>().sprite = openSprite;
                originBtn.onClick.AddListener(
                    () =>
                    {
                        targetComp.Show();
                        targetComp.Init(result);
                    }
                    );
            }
            else
            {
                originBtn.interactable = false;
                originBtn.GetComponent<Image>().sprite = lockSprite;
                playerTitleText.text = "????";
            }
            if(isEquip)
            {

            }
            if (!isInit)
            {
                PlayerDataXref.pl.onPlayerTitleChange += Refresh;
                isInit = true;
            }
        }

        private void Refresh()
        {
            originBtn = GetComponent<Button>();
            bool isHave = PlayerDataXref.pl.GetIsReward_Achieve(myResult.MyIndex);
            originBtn.onClick.RemoveAllListeners();
            if (isHave)
            {
                originBtn.interactable = true;
                playerTitleText.text = myResult.Title;
                originBtn.GetComponent<Image>().sprite = openSprite;
                originBtn.onClick.AddListener(
                    () =>
                    {
                        targetComp.Show();
                        targetComp.Init(myResult);
                    }
                    );
            }
            else
            {
                originBtn.interactable = false;
                originBtn.GetComponent<Image>().sprite = lockSprite;
                playerTitleText.text = "????";
            }
            if (isEquip)
            {

            }
        }
    }
}