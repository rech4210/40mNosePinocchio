using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class PlayerTitleUI : MonoBehaviour
{
    private delegate void OnClick_NoRet_NoParam(AchieveContentUI btn);
    private static event OnClick_NoRet_NoParam onClick;
    bool isOpen = false;

    private Button originBtn;
    [BoxGroup("앞면"), LabelText("업적 이름"), SerializeField] private Text playerTitleText;
    //착용중 이미지 효과
    private AchieveResult myResult;
    public void Init(AchieveResult result, StackUIComponent targetComp)
    {
        originBtn = GetComponent<Button>();

        myResult = result;
        
        PlayerData.onGetReward += GetReward;

        //업적달성함 여부 파악
        if (PlayerData.instance.IsGetReward[result.MyIndex] > 0)
        {
            originBtn.interactable = true;
            playerTitleText.text = result.Title;
            //업적을 먹었냐 안먹었냐 체크
            //팝업창 열기
            originBtn.onClick.AddListener(
                () =>
                {
                    targetComp.Show();
                    targetComp.GetComponent<ChangeTitlePopUp>().Init(result);
                    PlayerData.instance.OnGetReward(result.MyIndex);
                }
                );
        }
        //업적 못먹음
        else
        {
            originBtn.interactable = false;
            playerTitleText.text = "????";
        }
    }
    private void GetReward(ACHEIVE_INDEX index)
    {
        if (index == myResult.MyIndex)
        {
            originBtn.interactable = true;
            playerTitleText.text = myResult.Title;
        }
    }
}
