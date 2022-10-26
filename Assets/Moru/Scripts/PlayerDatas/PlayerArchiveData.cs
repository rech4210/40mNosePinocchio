using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Moru.UI;


namespace PD
{
    public partial class PlayerData
    {
        #region Const
        private const string isAchieve = "isAchieve";
        private const string isReward = "isReward";
        private const string curValue = "cur_Value";
        private const string maxValue = "max_Value";
        #endregion

        #region Field
        private AchieveAndTitle achieveSo;
        #endregion

        #region Events
        public delegate void OnClearAchieve(ACHEIVE_INDEX index);
        public  OnClearAchieve del_ClearAchieve;
        public  OnClearAchieve del_GetReward;

        public delegate void OnUpdateAchieve(ACHEIVE_INDEX index, int value, int maxValue);
        public OnUpdateAchieve del_UpdateAchieveStage;
        #endregion

        #region Properties
        public AchieveAndTitle AchieveSo => achieveSo;
        #endregion


        #region Methods
        /// <summary>
        /// 업적의 최대밸류값을 플레이어데이터에 초기화합니다.
        /// </summary>
        private void SetInitializeAchieve()
        {
            for (int i = 0; i < achieveSo.AchieveResults.Count; i++)
            {
                string maxkey = ((ACHEIVE_INDEX)i).ToString() + maxValue;
                var results = achieveSo.AchieveResults[i];
                SetSaveDataValue(maxkey, results.Target_AchievementCondition);
            }
        }


        /// <summary>
        /// 해당 업적의 현재 밸류와 타겟밸류를 받아옵니다.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public (int curValue, int maxValue) GetAchievePerpose(ACHEIVE_INDEX index)
        {
            string key_CurVal = index.ToString() + curValue;
            int _curValue = GetSaveDataValue(key_CurVal);

            string key_MaxVal = index.ToString() + maxValue;
            int _maxValue = GetSaveDataValue(key_MaxVal);

            return (_curValue, _maxValue);
        }

        /// <summary>
        /// 현재 업적의 밸류값을 업데이트합니다. isRecover = 덮어띄울지, 더할지를 결정합니다. (기본값 : 더하기)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <param name="isRecover"></param>
        public void SetAchievePerposeValue(ACHEIVE_INDEX index, int value, bool isRecover = false)
        {
            string key = index.ToString() + curValue;
            int _curValue = isRecover ? value : (GetSaveDataValue(key) + value);
            SetSaveDataValue(key, _curValue);
            
            string _maxkey = index.ToString() + maxValue;
            int _maxValue = GetSaveDataValue(_maxkey);
            if(_curValue >= _maxValue)
            {
                SetSuccess_Achieve(index);
            }
            else
            {
                del_UpdateAchieveStage?.Invoke(index, _curValue, _maxValue);
            }
        }

        /// <summary>
        /// 업적을 달성했는지 여부를 불린으로 받아옵니다.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool GetIsSuccess_Achieve(ACHEIVE_INDEX index)
        {
            string key = index.ToString() + isAchieve;
            int value = GetSaveDataValue(key);
            return value > 0 ? true : false;
        }

        /// <summary>
        /// 업적을 달성하도록 처리합니다.
        /// </summary>
        /// <param name="index"></param>
        public void SetSuccess_Achieve(ACHEIVE_INDEX index)
        {
            string key = index.ToString() + isAchieve;
            SetSaveDataValue(key, 1);
            del_ClearAchieve?.Invoke(index);
        }

        /// <summary>
        /// 업적에 대한 보상을 받았는지 여부를 받아옵니다.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool GetIsReward_Achieve(ACHEIVE_INDEX index)
        {
            string key = index.ToString() + isReward;
            int value = GetSaveDataValue(key);
            return value > 0 ? true : false;
        }

        /// <summary>
        /// 업적에 대한 보상을 받도록 처리합니다.
        /// </summary>
        /// <param name="index"></param>
        public void SetReward_Achieve(ACHEIVE_INDEX index)
        {
            string key = index.ToString() + isReward;
            SetSaveDataValue(key, 1);
            del_GetReward?.Invoke(index);
        }
        #endregion
    }
}