using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearUI : MonoBehaviour
{
    [SerializeField] Button Btn_Main;
    [SerializeField] Button Btn_Next;

    private void Awake()
    {
        Btn_Main.onClick.AddListener(
            () => WSceneManager.instance.MoveStageSeletedPage()
            );
        Btn_Next.onClick.AddListener(
            () => WSceneManager.instance.ReLoad_ThisScene_WtihStageUp()
            );
    }
}
