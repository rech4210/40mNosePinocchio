using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lerf : MonoBehaviour
{

    public Image lerfImage;
    Vector4 lerfvector;

    Button lerfButton;
    void Start()
    {
        lerfImage = gameObject.GetComponent<Image>();
        lerfButton= gameObject.GetComponent<Button>();
    }
    private void Update()
    {
        lerfButton.onClick.AddListener(setlerf);
    }
    void setlerf()
    {
        StartCoroutine(alpha_lerf());
    }


    IEnumerator alpha_lerf()
    {
        while(lerfImage.color.a != 1)
        {
            lerfImage.color = lerfvector;
            yield return new WaitForSeconds(0.03f);
        }
    }
}
