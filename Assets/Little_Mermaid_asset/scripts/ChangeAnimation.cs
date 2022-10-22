using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChangeAnimation : MonoBehaviour
{
    public Image[] image;
    public Sprite[] sprite;

    public Sprite[] witchAnime;

    WaitForSeconds waitTime = new WaitForSeconds(1.5f);
    public void changeAnimation(InputManager.State state) //마녀와 인어공주 스프라이트 교체
    {
        Debug.Log("애니메이션 진입");
        switch (state)
        {
            case InputManager.State.normal: // idle
                image[0].sprite = sprite[0]; // 인어공주
                //StartCoroutine(witchAnimation(state));
                witchAnimation(state);
                break;

            case InputManager.State.success: // 노트 성공
                image[0].sprite = sprite[0];
                witchAnimation(state);

                //StartCoroutine(witchAnimation(state));

                break;

            case InputManager.State.fail:   // 클리어 실패
                image[0].sprite = sprite[1];
                image[1].sprite = sprite[3];

                break;

            case InputManager.State.clear:   // 클리어 성공
                image[0].sprite = sprite[2];
                image[1].sprite = sprite[4];
                break;
            default:
                break;
        }

    }

    void witchAnimation(InputManager.State state)
    {
        while(state != InputManager.State.fail)
        {
            for (int i = 0; i < witchAnime.Length; i++)
            {
                image[1].sprite = witchAnime[i];
            }
        }

    }
}
