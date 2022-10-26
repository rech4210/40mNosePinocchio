using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PD;

namespace Moru.UI
{
    public class PlayerInfoUI : MonoBehaviour
    {
        [SerializeField] private Text titleText;
        [SerializeField] private Text clearRate;
        [SerializeField] private Image fillAmount;
        float fillValue;

        void Start()
        {
            UpdatePlayerInfo();
            PlayerDataXref.pl.onPlayerTitleChange += UpdatePlayerInfo;
        }

        private void OnDestroy()
        {
            PlayerDataXref.pl.onPlayerTitleChange -= UpdatePlayerInfo;
        }

        private void UpdatePlayerInfo()
        {
            var player_clearRate = PlayerDataXref.pl.WholeClearRate;
            if (titleText != null)
            {
                titleText.text = PlayerDataXref.pl.PlayerTitle;
            }
            if (clearRate != null)
            {
                clearRate.text = (player_clearRate * 100).ToString("F0") + " %";
            }
            if (fillAmount != null)
            {
                fillAmount.fillAmount = player_clearRate;
            }
            fillValue = player_clearRate;
        }

    }
}