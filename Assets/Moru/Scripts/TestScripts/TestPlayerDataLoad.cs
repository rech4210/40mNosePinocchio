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
        PlayerData.Load_GameData();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerData.onClearGame?.Invoke(GAME_INDEX.Cinderella, 0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerData.onClearGame?.Invoke(GAME_INDEX.Cinderella, 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayerData.onClearGame?.Invoke(GAME_INDEX.Cinderella, 2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayerData.onClearGame?.Invoke(GAME_INDEX.Cinderella, 3);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var value = PlayerData.GetStageClearDataPerGame(GAME_INDEX.Cinderella);
            for (int i = 0; i < value.Length; i++)
            {
                Debug.Log($"{value[i]}");
            }
        }
    }
}
