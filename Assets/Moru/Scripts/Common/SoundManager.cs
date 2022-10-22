using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager m_Instacne;
    public static SoundManager instance
    {
        get
        {
            if (m_Instacne == null)
            {
                //WSceneManager에서 인스턴스 초기화하며 SoundManager를 생성합.
                var initComp = WSceneManager.instance;
            }
            m_Instacne = FindObjectOfType<SoundManager>(true);
            return m_Instacne;
        }
    }

    public AudioSource BGMs;
    public AudioSource SFXs;

    protected void Awake()
    {
        var comps = new List<AudioSource>();
        for(int i = 0; i < 2; i++)
        {
            comps.Add(this.gameObject.AddComponent<AudioSource>());
        }
        BGMs = comps[0];
        BGMs.loop = true;
        SFXs = comps[1];
    }

    /// <summary>
    /// 해당 BGM을 재생합니다.
    /// </summary>
    /// <param name="clip"></param>
    public static void PlayBGM(AudioClip clip)
    {
        var instance = SoundManager.instance;
        instance.BGMs.clip = clip;
        instance.BGMs.Play();
    }

    public static void PlaySFX(AudioClip clip)
    {
        var instance = SoundManager.instance;
        instance.SFXs.PlayOneShot(clip);
        
    }


}
