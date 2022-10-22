using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PD;

namespace Moru.UI
{
    public class ChangeTitlePopUp : MonoBehaviour
    {
        [SerializeField] Text text;
        [SerializeField] Button AcceptBtn;

        public void Init(AchieveResult result)
        {
            text.text = $"칭호를 \"{result.Title}\"으로 바꾸시겠습니까?";
            AcceptBtn.onClick.RemoveAllListeners();
            AcceptBtn.onClick.AddListener(
                () =>
                PlayerData.instance.PlayerTitle = result.Title
                );

        }

    }
}