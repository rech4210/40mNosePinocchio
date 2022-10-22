using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingleToneMono<SoundManager>
{
    public AudioSource BGMs;
    public AudioSource SFXs;

    protected override void Awake()
    {
        base.Awake();
        var comps = new List<AudioSource>();
        for(int i = 0; i < 2; i++)
        {
            comps.Add(this.gameObject.AddComponent<AudioSource>());
        }
        BGMs = comps[0];
        SFXs = comps[1];
    }

    /// <summary>
    /// 해당 BGM을 재생합니다.
    /// </summary>
    /// <param name="clip"></param>
    public static void PlayBGM(AudioClip clip)
    {
        var instance = SoundManager.Instance;
        instance.BGMs.clip = clip;
        instance.BGMs.Play();
    }

    public static void PlaySFX(AudioClip clip)
    {
        var instance = SoundManager.Instance;
        instance.SFXs.PlayOneShot(clip);
        
    }


}
