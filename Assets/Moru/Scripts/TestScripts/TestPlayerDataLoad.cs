using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerDataLoad : MonoBehaviour
{
    public PlayerData data;
    // Start is called before the first frame update
    void Start()
    {
        data = PlayerData.instance;
        data.Load_GameData();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            PlayerData.onClearGame?.Invoke(GAME_INDEX.Snow_White);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerData.onClearGame?.Invoke(GAME_INDEX.Cinderella);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerData.onClearGame?.Invoke(GAME_INDEX.Pinocchio);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayerData.onClearGame?.Invoke(GAME_INDEX.Little_Mermaid);
        }
    }
}
