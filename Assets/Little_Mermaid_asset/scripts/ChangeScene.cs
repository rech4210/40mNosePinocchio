using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScene : MonoBehaviour
{
    public void sceneChange()
    {
        SceneManager.LoadScene("Little_Mermaid");
    }
}
