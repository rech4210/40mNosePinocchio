using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailUI : MonoBehaviour
{
    [SerializeField] Button Btn_Main;
    [SerializeField] Button Btn_Retry;

    private void Awake()
    {
        Btn_Main.onClick.AddListener(
            () => WSceneManager.instance.MoveStageSeletedPage()
            );
        Btn_Retry.onClick.AddListener(
            () => WSceneManager.instance.ReLoad_ThisScene()
            );
    }
}
