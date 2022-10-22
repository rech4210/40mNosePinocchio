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
        #endregion

        #region Field
        [ShowInInspector] private Dictionary<CUTSCENE_INDEX, int> dic_CutSceneOpen;
        #endregion

        #region Properties
        public Dictionary<CUTSCENE_INDEX, int> Dic_CutSceneOpen => dic_CutSceneOpen;
        #endregion

        public static void Load_CutSceneData()
        {
            var instance = PlayerData.instance;
            for (int i = 0; i < (int)CUTSCENE_INDEX.NONE; i++)
            {
                var value = PlayerPrefs.GetInt(((CUTSCENE_INDEX)i).ToString() + isCutSceneOpen, 0);
                if(instance.dic_CutSceneOpen.ContainsKey((CUTSCENE_INDEX)i))
                {
                    instance.dic_CutSceneOpen[(CUTSCENE_INDEX)i] = value;
                }
                else
                {
                    instance.dic_CutSceneOpen.Add((CUTSCENE_INDEX)i, value);
                }
            }
        }

        public static bool GetCutScene(CUTSCENE_INDEX index)
        {
            bool retVal = false;
            int value = PlayerData.instance.dic_CutSceneOpen[index];
            if (value > 0)
            {
                retVal = true;
            }
            else retVal = false;
            return retVal;
        }
        public static void SetCutScene(CUTSCENE_INDEX index)
        {
            var instance = PlayerData.instance;
            if (instance.dic_CutSceneOpen.ContainsKey(index))
            {
                instance.dic_CutSceneOpen[index] = 1;
            }
            else
            {
                instance.dic_CutSceneOpen.Add(index, 1);
            }
            PlayerPrefs.SetInt(index.ToString() + isCutSceneOpen, 1);
        }
    }
}
