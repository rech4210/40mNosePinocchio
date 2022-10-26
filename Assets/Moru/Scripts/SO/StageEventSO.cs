using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
namespace PD
{

    [CreateAssetMenu(fileName = "StageEventSO", menuName = "MORU/StageEventSO")]
    public class StageEventSO : ScriptableObject
    {
        [Searchable, SerializeField, OnInspectorInit(@"OnInit")]
        private List<StageInfoPair> stagesInfo;

        public StageInfoPair this[GAME_INDEX index] => stagesInfo[(int)index];


        private void OnInit()
        {
            if (stagesInfo == null)
            {
                stagesInfo = new List<StageInfoPair>();
            }
            if (stagesInfo.Count < (int)GAME_INDEX.None)
            {
                for (int i = stagesInfo.Count; i < (int)GAME_INDEX.None; i++)
                {
                    stagesInfo.Add(new StageInfoPair((GAME_INDEX)i));
                }
            }
            for (int i = 0; i < (int)GAME_INDEX.None; i++)
            {
                if (stagesInfo[i].Index != (GAME_INDEX)i)
                {
                    stagesInfo[i].Index = (GAME_INDEX)i;
                }
            }
        }
    }

    [System.Serializable]
    public class StageInfoPair
    {
        #region Field
        [SerializeField, LabelText("게임모드")] private GAME_INDEX index;
        [OnValueChanged("AddTo")] [SerializeField, LabelText("최대 스테이지 수")] private uint stageCount;
        [OnValueChanged("SetChapter")] [SerializeField, LabelText("다음챕터 해금 스테이지")] private uint chapterOpenStage;
        [SerializeField, LabelText("스테이지 정보"), ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "LabelName")] 
        public List<StageElementEvent> stageElements;
        #endregion


        #region Properties
        public GAME_INDEX Index { get => index; set => index = value; }
        public int StageCount => (int)stageCount;
        public int ChapterOpenStage => (int)chapterOpenStage;
        public List<StageElementEvent> StageElements { get => stageElements; set => stageElements = value; }
        #endregion


        public StageInfoPair(GAME_INDEX _index)
        {
            index = _index;
        }

        private void AddTo()
        {
            if(stageCount == 0)
            {
                stageElements.Clear();
            }
            if(stageElements.Count < stageCount)
            {
                for(int i = stageElements.Count; i < stageCount; i++)
                {
                    stageElements.Add(new StageElementEvent());
                }
            }
            else if(stageElements.Count > stageCount)
            {
                for (int i = stageElements.Count - 1; i > stageCount - 1; i--)
                {
                    stageElements.RemoveAt(i);
                }
            }
        }

        private void SetChapter()
        {
            if(chapterOpenStage > stageCount)
            {
                chapterOpenStage = stageCount;
            }
        }
    }

    [System.Serializable]
    public class StageElementEvent
    {
        public string LabelName;
        public class StageEvent : UnityEvent<GAME_INDEX>
        { }

        public delegate void Delegate_Event_NoRetVal_NoParams();
        public delegate void Delegate_Event_NoRetVal_GAME_INDEX_Params(GAME_INDEX index);
        public Delegate_Event_NoRetVal_GAME_INDEX_Params testClear;

        public StageEvent ClearEvent = new StageEvent();
        public StageEvent FirstEvent = new StageEvent();

    }

}
