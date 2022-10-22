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
        [BoxGroup("뒷면"), LabelText("업적 받기 텍스트"), SerializeField] private Text isGetReward;



        private AchieveResult myResult;

        public void Init(AchieveResult result, StackUIComponent targetComp)
        {
            isOpen = false;
            onClick += AchieveContentUI_onClick;
            originBtn = GetComponent<Button>();
            myResult = result;
            achieveName.text = result.AchieveName;
            achieveImg.sprite = result.Icon;
            achieveDesc.text = result.AchieveDesc;
            achieveImg.gameObject.SetActive(false);


            originBtn.onClick.AddListener(OnClick);
            PlayerData.onGetReward += GetReward;

            //업적달성함 여부 파악
            if (PlayerData.instance.IsAchievement[result.MyIndex] > 0)
            {
                //originBtn.interactable = true;
                //업적을 먹었냐 안먹었냐 체크
                if (PlayerData.instance.IsGetReward[result.MyIndex] > 0)
                {
                    //안먹은건 파티클로 추가표과를 주던가.
                    //팝업창 열기
                    btn_GetReward.onClick.AddListener(
                        () =>
                        {
                            targetComp.Show();
                            PlayerData.instance.OnGetReward(result.MyIndex);
                        }
                        );
                    btn_GetReward.interactable = true;
                    isGetReward.text = "보상 받기";
                }
                //이미 먹은건 먹었다고 표시해야지
                else
                {
                    btn_GetReward.interactable = false;
                    isGetReward.text = "이미 받은 보상입니다.";
                }
            }
            //달성 못함
            else
            {
                //originBtn.interactable = false;
            }
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

        private void GetReward(ACHEIVE_INDEX index)
        {
            if (index == myResult.MyIndex)
            {
                btn_GetReward.interactable = false;
                isGetReward.text = "이미 받은 보상입니다.";
            }
        }
    }
}