using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Moru.UI
{
    public class GameExplainPopUp : MonoBehaviour
    {
        [SerializeField] Image image;

        public void Init(Sprite sprite)
        {
            image.sprite = sprite;
        }
    }
}

