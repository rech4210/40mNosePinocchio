using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using PD;

namespace Moru.UI
{
    public class AchieveContentUI : MonoBehaviour
    {
        private delegate void OnClick_NoRet_NoParam(AchieveContentUI btn);
        private static event OnClick_NoRet_NoParam onClick;
        bool isOpen = false;

        private Button originBtn;
        [BoxGroup("앞면"), LabelText("업적 이름"), SerializeField] private Text achieveName;
        [BoxGroup("뒷면"), LabelText("업적 이미지"), SerializeField] private Image achieveImg;
        [BoxGroup("뒷면"), LabelText("업적 내용"), SerializeField] private Text achieveDesc;
        [BoxGroup("뒷면"), LabelText("업적 받기버튼"), SerializeField] private Button btn_GetReward;


        [BoxGroup("뒷면"), LabelText("업적 현재정보 프로그레스"), SerializeField] private Image clearRate;
        [BoxGroup("뒷면"), LabelText("업적 현재정보 프로그레스 배경"), SerializeField] private Image progressbar_bg;
        [BoxGroup("뒷면"), LabelText("업적 현재정보 텍스트"), SerializeField] private Text clearRate_Text;


        private AchieveResult myResult;

        private void Start()
        {
            originBtn = GetComponent<Button>();
            onClick += AchieveContentUI_onClick;
            originBtn.onClick.AddListener(OnClick);
        }

        public void Init(AchieveResult result, StackUIComponent targetComp)
        {
            isOpen = false;
            myResult = result;
            achieveName.text = result.AchieveName;
            achieveImg.sprite = result.Icon;
            achieveDesc.text = result.AchieveDesc;
            achieveImg.gameObject.SetActive(false);

            bool rewardbtn_Interact =
                PlayerDataXref.pl.GetIsSuccess_Achieve(result.MyIndex) &&       //업적은 달성했는지
                !PlayerDataXref.pl.GetIsReward_Achieve(result.MyIndex);         //보상은 안받았는지
            btn_GetReward.interactable = rewardbtn_Interact;
            btn_GetReward.onClick.AddListener(
                () =>
                {
                    targetComp.Show();
                    PlayerDataXref.pl.SetReward_Achieve(result.MyIndex);
                }
                );


            var perpose = PlayerDataXref.pl.GetAchievePerpose(result.MyIndex);
            switch (result.Display_type)
            {
                case AchieveResult.DISPLAY_TYPE.INT:
                    clearRate.enabled = true;
                    progressbar_bg.enabled = true;
                    clearRate_Text.enabled = true;
                    clearRate.fillAmount = (float)perpose.curValue / (float)perpose.maxValue;
                    clearRate_Text.text = $"{perpose.curValue } / {perpose.maxValue}";
                    break;
                case AchieveResult.DISPLAY_TYPE.NORMAL:
                    clearRate.enabled = false; progressbar_bg.enabled = false;
                    clearRate_Text.enabled = true;
                    string text = PlayerDataXref.pl.GetIsSuccess_Achieve(result.MyIndex) ? "달성" : "미달성";
                    clearRate_Text.text = $"{text}";
                    break;
                case AchieveResult.DISPLAY_TYPE.PERCENT:
                    clearRate.enabled = true;
                    progressbar_bg.enabled = true;
                    clearRate_Text.enabled = true;
                    float rate = (float)perpose.curValue / (float)perpose.maxValue;
                    clearRate.fillAmount = rate;
                    clearRate_Text.text = $"{(rate * 100).ToString("F0")} %";
                    break;
                case AchieveResult.DISPLAY_TYPE.NONE:
                    clearRate.enabled = false;
                    progressbar_bg.enabled = false;
                    clearRate_Text.enabled = false;
                    break;
                default:
                    clearRate.enabled = false;
                    progressbar_bg.enabled = false;
                    clearRate_Text.enabled = false;
                    break;
            }

            clearRate_Text.text = !rewardbtn_Interact ? clearRate_Text.text : "이미 받은 보상";
            clearRate.enabled = !rewardbtn_Interact ? clearRate.enabled : false;
            progressbar_bg.enabled = !rewardbtn_Interact ? progressbar_bg.enabled : false;
        }

        private void AchieveContentUI_onClick(AchieveContentUI btn)
        {
            if (btn != this)
            {
                if (isOpen)
                {
                    Hide();
                }
            }
        }

        private void OnClick()
        {
            if (!isOpen)
            {
                isOpen = true;
                achieveName.enabled = false;
                achieveImg.gameObject.SetActive(true);
                onClick?.Invoke(this);
            }
            else
            {
                Hide();
            }
        }

        void Hide()
        {
            isOpen = false;
            achieveName.enabled = true;
            achieveImg.gameObject.SetActive(false);
        }

        private void GetReward()
        {

        }
    }
}