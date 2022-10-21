using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChangeAnimation : MonoBehaviour
{
    public Image[] image;
    public Sprite[] sprite;

    public void changeAnimation(InputManager.State state) //마녀와 인어공주 스프라이트 교체
    {
        switch (state)
        {
            case InputManager.State.normal:
                image[0].sprite = sprite[0];
                image[1].sprite = sprite[0];
                break;

            case InputManager.State.success:
                image[0].sprite = sprite[1];
                image[1].sprite = sprite[1];
                break;

            case InputManager.State.fail:
                image[0].sprite = sprite[2];
                image[1].sprite = sprite[2];
                break;

            case InputManager.State.clear:
                image[0].sprite = sprite[3];
                image[1].sprite = sprite[3];
                break;

            default:
                break;
        }

    }
}
