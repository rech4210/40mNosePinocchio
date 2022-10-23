using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Moru.UI;

public class CutSceneContentUI : MonoBehaviour
{
    [SerializeField] Image image;
    private Sprite sprite1;
    [SerializeField] Button btn;

    public void Init(Sprite sprite, StackUIComponent comp)
    {
        image.sprite = sprite;
        sprite1 = sprite;
        GetComponent<PopNPush>().Push_comp = comp;
        btn.onClick.AddListener(
            () => FindObjectOfType<CutSceneViewerUI>().bickIMG.sprite = sprite1
            );
        
    }
}
