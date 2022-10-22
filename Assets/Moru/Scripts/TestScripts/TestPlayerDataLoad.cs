using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PD;
public class TestPlayerDataLoad : MonoBehaviour
{
    public PlayerData data;
    public ACHEIVE_INDEX target;
    // Start is called before the first frame update
    void Start()
    {
        data = PlayerData.instance;
        PlayerData.Load_GameData();
        PlayerData.Load_PlayerAchieve();
    }
    public void TestAchieve()
    {
        //업적 시스템의 밸류 업데이트 델리게이트 메서드
        //(목표치에 도달하면 자동으로 업적이 달성됩니다.
        PlayerData.onUpdateAchieve(target, 5);
    }
    public void TestClear()
    {
        PlayerData.onClearAchieve(target);
    }
    

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //사용방법
            //신데렐라 게임모드에서 0번째 스테이지를 클리어했습니다!
            PlayerData.onClearGame?.Invoke(GAME_INDEX.Cinderella, 0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //사용방법
            //잭과 콩나물 게임모드에서 1번째 스테이지를 클리어했습니다!
            PlayerData.onClearGame?.Invoke(GAME_INDEX.Jack_And_Beanstalk, 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //사용방법
            //인어공주 게임모드에서 2번째 스테이지를 클리어했습니다!
            PlayerData.onClearGame?.Invoke(GAME_INDEX.Little_Mermaid, 2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //사용방법
            //신데렐라 게임모드에서 3번째 스테이지를 클리어했습니다!
            PlayerData.onClearGame?.Invoke(GAME_INDEX.Cinderella, 3);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //신데렐라 게임모드의 모든 스테이지의 클리어 여부를 받아옵니다.
            var value = PlayerData.GetStageClearDataPerGame(GAME_INDEX.Cinderella);
            for (int i = 0; i < value.Length; i++)
            {
                Debug.Log($"{value[i]}");
            }
            //잭과 콩나물 게임모드의 0번째 스테이지의 클리어 여부를 받아옵니다.
            bool result = PlayerData.GetStageClearDataPerGame(GAME_INDEX.Cinderella, 0);
            Debug.Log($"{result}");
        
        }
    }
}
