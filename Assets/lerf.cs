using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lerf : MonoBehaviour
{

    Image lerfImage;
    Vector4 lerfvector;
    void Start()
    {
        lerfImage = gameObject.GetComponent<Image>();
        StartCoroutine(alpha_lerf());
    }


    IEnumerator alpha_lerf()
    {
        while(lerfImage.color.a != 1)
        {
            lerfvector += new Vector4(0, 0, 0, 0.01f);
            lerfImage.color = lerfvector;
            yield return new WaitForSeconds(0.03f);
        }
    }
}
