using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;





namespace PD
{

    public partial class PlayerData
    {
        private const string isCutSceneOpen = "isCut";

        #region Event
        public delegate void Del_CutSceneEvent(CUTSCENE_INDEX index);
        public Del_CutSceneEvent del_CutSceneEvent;
        #endregion

        #region Field

        #endregion

        #region Properties

        #endregion
        /// <summary>
        /// 컷씬을 보았는지(받았는지) 여부를 받아옵니다.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool GetCutScene(CUTSCENE_INDEX index)
        {
            string key = index.ToString() + isCutSceneOpen;
            int value = GetSaveDataValue(key);
            bool retVal = value > 0 ? true : false;
            return retVal;
        }

        /// <summary>
        /// 컷씬을 보고, 해금되도록 설정합니다.
        /// </summary>
        /// <param name="index"></param>
        public void SetCutScene(CUTSCENE_INDEX index)
        {
            string key = index.ToString() + isCutSceneOpen;
            SetSaveDataValue(key, 1);
            del_CutSceneEvent?.Invoke(index);
        }
    }
}
