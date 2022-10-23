using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class lerfsmooth : MonoBehaviour
{

    RectTransform rectTransform;

    float anchorPos_y = 0f;
    [Range(0f, 1f)]
    public float duration = 0.25f;
    private float step;
    WaitForSeconds waitTime = new WaitForSeconds(0.5f);


    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(waitUIvisible());
    }
    private void Update()
    {
        Debug.Log(anchorPos_y);
    }

    IEnumerator waitUIvisible()
    {
        float t = 0;
        anchorPos_y = rectTransform.anchoredPosition.y;

        while (t < 2f)
        {

            t += Time.deltaTime;
            //anchorPos_y = Mathf.Lerp(min_PosX, max_PosX, t / duration);
            anchorPos_y = Mathf.SmoothDamp(anchorPos_y, 0, ref step, duration);

            rectTransform.anchoredPosition = new Vector2(0, anchorPos_y);
            yield return null;
        }

        StartCoroutine(waitUIvisible());
    }

}
