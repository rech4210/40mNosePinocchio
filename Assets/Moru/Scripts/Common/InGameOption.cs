using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace Moru.UI
{
    public class InGameOption : MonoBehaviour
    {
        [Header("볼륨조절")]
        [SerializeField] private Toggle MuteBGM;
        [SerializeField] private Toggle MuteSFX;
        [SerializeField] private Slider BGMValue;
        [SerializeField] private Slider SFXValue;
        [Header("버튼")]
        [SerializeField] private Button Btn_Title;
        [SerializeField] private Button Btn_ToLobby;
        [SerializeField] private Button Btn_ToStage;
        

        bool isInit = false;
        private void OnEnable()
        {
            var instance = SoundManager.Instance;
            MuteBGM.isOn = instance.BGMs.mute;
            MuteSFX.isOn = instance.SFXs.mute;

            BGMValue.value = instance.BGMs.volume;
            SFXValue.value = instance.SFXs.volume;
        }


        // Start is called before the first frame update
        void Start()
        {
            var instance = SoundManager.Instance;
            MuteBGM.onValueChanged.AddListener((value) => instance.BGMs.mute = value);
            MuteSFX.onValueChanged.AddListener((value) => instance.SFXs.mute = value);
            BGMValue.onValueChanged.AddListener((value) => instance.BGMs.volume = value);
            SFXValue.onValueChanged.AddListener((value) => instance.SFXs.volume = value);

            var scene = WSceneManager.instance;
            Btn_Title.onClick.AddListener(() => scene.MoveScene(SceneIndex.MainPage));
            Btn_ToLobby.onClick.AddListener(() => scene.MoveChapterSelectPage());
            Btn_ToStage.onClick.AddListener(() => scene.MoveStageSeletedPage());
            isInit = true;
        }

    }
}