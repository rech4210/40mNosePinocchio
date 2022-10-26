using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Moru.UI;

namespace PD
{
    /// <summary>
    /// 플레이어의 챕터와 스테이지 데이터정보를 관리합니다.
    /// </summary>
    public partial class PlayerData
    {
        #region Const
        private const string isChapterOpen = "isChapterOpen";
        private const string isChapterClear = "isChapterClear";
        #endregion

        #region Field

        #endregion

        #region Properties
        public ChapterStorySO ChapterStorySO => chapterStorySO;
        #endregion

        #region Events
        public delegate void OnChapter(GAME_INDEX index);
        /// <summary>
        /// 매개변수값의 게임이 해금될 때 호출됩니다.
        /// </summary>
        public event OnChapter del_onOpenChapter;
        /// <summary>
        /// 메개변수값의 게임이 클리어될 때 호출됩니다.
        /// </summary>
        public event OnChapter del_onClearChapter;

        public delegate void Delegate_StageSetUp(GAME_INDEX index);
        public Delegate_StageSetUp del_StageSetUp;
        #endregion

        /// <summary>
        /// 해당 챕터가 해금되어있는지 여부를 반환합니다.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsChapterOpen(GAME_INDEX index)
        {
            string key = (index).ToString() + isChapterOpen;
            int value = GetSaveDataValue(key);
            bool retVal = value > 0 ? true : false;
            return retVal;
        }

        /// <summary>
        /// 해당 챕터가 클리어되어있는지 여부를 반환합니다.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsClearChapter(GAME_INDEX index)
        {
            var instance = PlayerData.instance;
            string key = (index).ToString() + isChapterClear;
            int value = instance.GetSaveDataValue(key);
            bool retVal = value > 0 ? true : false;
            return retVal;
        }

        
        /// <summary>
        /// 해당 챕터를 해금합니다. (해금과 동시에 해당게임의 첫번째 스테이지도 함께 해금됩니다.)
        /// </summary>
        /// <param name="index"></param>
        public void OpenChapter(GAME_INDEX index)
        {
            Debug.Log($"오픈챕터 디버깅 {index.ToString()}");
            string key = index.ToString() + isChapterOpen;
            int curValue = GetSaveDataValue(key);
            SetSaveDataValue(key, 1);
            //cur과 new의 밸류를 비교해서 변화가 있으면 새로 해금된 것으로 팝업 띄우기

            del_onOpenChapter?.Invoke(index);

            //챕터를 오픈함과 동시에 첫번째 스테이지를 해금합니다.
            var _stringSaveKey2 = index.ToString() + 0.ToString() + isOpenStageString;
            SetSaveDataValue(_stringSaveKey2, 1);
            //데이터 정상저장 여부를 확인하는 겸 세이브 밸류를 받아옵니다.
            int cur_SaveData = GetSaveDataValue(_stringSaveKey2);
            del_OpenGameStage?.Invoke(index, 0, cur_SaveData > 0 ? true : false);
        }

        /// <summary>
        /// 해당 챕터를 클리어합니다. (디버깅을 위해 해당메서드를 호출하면 모든 스테이지도 함께 클리어처리됩니다.)
        /// </summary>
        /// <param name="index"></param>
        public void ClearChapter(GAME_INDEX index)
        {
            string key = index.ToString() + isChapterClear;
            SetSaveDataValue(key, 1);

            for (int i = 0; i < stageEventSO[index].StageElements.Count; i++)
            {
                SetGameStageClear(index, i, 1);
            }
            del_onClearChapter?.Invoke(index);
        }
    }
}
