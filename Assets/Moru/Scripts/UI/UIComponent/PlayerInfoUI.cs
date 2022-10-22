using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    [SerializeField] private Text titleText;
    [SerializeField] private Text clearRate;
    [SerializeField] private Image fillAmount;
    float fillValue;

    void Start()
    {
        UpdatePlayerInfo();
        PlayerData.instance.onPlayerTitleChange += UpdatePlayerInfo;
    }

    private void UpdatePlayerInfo()
    {
        if(titleText != null)
        {
            titleText.text = PlayerData.instance.PlayerTitle;
        }
        if(clearRate != null)
        {
            clearRate.text = (PlayerData.instance.WholeClearRate * 100).ToString("F0") + " %";
        }
        if(fillAmount != null)
        {
            fillAmount.fillAmount = PlayerData.instance.WholeClearRate;
        }
        fillValue = PlayerData.instance.WholeClearRate;
    }

}
