using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Moru.UI;


namespace PD
{

    /// <summary>
    /// 플레이어의 매우 기초되는 데이터값을 받아옵니다. (플레이어 이름, 선택한 타이틀 등)
    /// </summary>
    [System.Serializable]
    public partial class PlayerData
    {
        #region Const
        private const string isClearStageString = "isClear";
        private const string isOpenStageString = "isOen";
        #endregion


        #region instance
        private static PlayerData m_instance;
        public static PlayerData instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new PlayerData();
                }
                return m_instance;
            }
        }
        #endregion

        #region Events
        public delegate void OnValueChange();
        public event OnValueChange onPlayerNameChange;
        public event OnValueChange onPlayerTitleChange;
        public event OnValueChange onPlayerStageChange;
        public event OnValueChange onPlayerGameChange;


        //Old
        //public delegate void OnSelectStage(bool[] arr, GAME_INDEX index);
        //public OnSelectStage onSelectStage;

        /// <summary>
        /// 플레이어가 선택한 게임과 스테이지를 해당스테이지로 변경합니다.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="stageNum"></param>
        public delegate void OnSetToStage(GAME_INDEX index, int stageNum);
        public OnSetToStage del_SetPlayerSelectedStage;

        public delegate void OnChangeGameStageValue(GAME_INDEX index, int stageNum, bool inputValue);
        /// <summary>
        /// 스테이지를 클리어할 때 호출됩니다.
        /// </summary>
        public event OnChangeGameStageValue del_ClearGameStage;
        /// <summary>
        /// 스테이지를 해금할 때 실행시킵니다.
        /// </summary>
        public event OnChangeGameStageValue del_OpenGameStage;
        #endregion


        #region Field
        [SerializeField] private string playerName;
        [SerializeField] private string playerTitle;

        /// <summary>
        /// 각 게임(챕터)별 스토리 SO
        /// </summary>
        [SerializeField] private ChapterStorySO chapterStorySO;
        /// <summary>
        /// 각 게임별 스테이지 수나 해금이벤트발생 위치 SO
        /// </summary>
        [SerializeField] private StageEventSO stageEventSO;

        [SerializeField] private GameObject PopUpUI;
        [SerializeField] private GAME_INDEX cur_Game_Index;
        [SerializeField] private int curStageSeletedNum;
        #endregion

        #region Properties
        public string PlayerName
        {
            get
            {
                playerName = PlayerPrefs.GetString("PlayerName", "");
                return playerName;
            }
            set
            {
                PlayerPrefs.SetString("PlayerName", value);
                playerName = value;
                onPlayerNameChange?.Invoke();
            }
        }
        public string PlayerTitle
        {
            get
            {
                playerTitle = PlayerPrefs.GetString("PlayerTitle", "");
                return playerTitle;
            }
            set
            {
                PlayerPrefs.SetString("PlayerTitle", value);
                playerTitle = value;
                onPlayerTitleChange?.Invoke();
            }
        }
        public float WholeClearRate
        {
            get
            {
                return GetWholeClearRate();
            }
        }

        /// <summary>
        /// 현재 플레이어가 선택한 게임의 인덱스번호입니다.
        /// </summary>
        public GAME_INDEX Cur_Game_Index { get => cur_Game_Index; set { cur_Game_Index = value; onPlayerStageChange?.Invoke(); } }
        /// <summary>
        /// 현재 플레이어가 선택한 게임의 스테이지 넘버입니다.
        /// </summary>
        public int CurStageSelectedNum { get => curStageSeletedNum; set { curStageSeletedNum = value; onPlayerGameChange?.Invoke(); } }

        public StageEventSO StageEventSO => stageEventSO;



        //public bool IsStageClear

        #endregion


        /// <summary>
        /// 스테이지 데이터를 SO로부터 받을 수 있도록, PlayerDataXref에서 관리하도록 합니다. 
        /// 플레이어프랩스는 Awake단계에서 초기화가 가능합니다. 
        /// 인스턴스 생성을 awake에서 1회만 호출될 수 있도록 합니다.
        /// </summary>
        public PlayerData()
        {
            //스테이지 정보 관련
            //onClearGame += ClearGame;
            //this.stageCountPerGames = stageCountPerGames;
            //this.targetStage = targetStage;

            ///플레이어가 선택한 스테이지 에 이벤트 등록
            del_SetPlayerSelectedStage += (index, stageNum) =>
            {
                cur_Game_Index = index;
                curStageSeletedNum = stageNum;
            };


            ///업적 관련///
            //isAchievement = new Dictionary<ACHEIVE_INDEX, int>();
            //cur_AchievementValue = new Dictionary<ACHEIVE_INDEX, AchieveResult>();
            //isGetReward = new Dictionary<ACHEIVE_INDEX, int>();
            //onClearAchieve += onClearAchieveCallBack;
            //onUpdateAchieve += onUpdateAchieveCallBack;


            //챕터 관련
            //openChapter = new Dictionary<GAME_INDEX, int>();
            //clearChapter = new Dictionary<GAME_INDEX, int>();

            //컷씬 관련
            //dic_CutSceneOpen = new Dictionary<CUTSCENE_INDEX, int>();
            //첫번째 챕터를 엽니다.

        }

        #region Methods

        #region Public Methods
        public void OnInitialized()
        {
            PopUpUI = Resources.Load<GameObject>("AchieveCanvas");
            chapterStorySO = Resources.Load<ChapterStorySO>("ChapterStorySO");
            stageEventSO = Resources.Load<StageEventSO>("StageEventSO");
            achieveSo = Resources.Load<AchieveAndTitle>("AchieveTitle");

            //스테이지 정보 이니셜라이징
            for (int i = 0; i < (int)GAME_INDEX.None; i++)
            {
                var stageEventInfo = StageEventSO[(GAME_INDEX)i];

                //해금스테이지 이벤트 등록
                var targetOpenStage = stageEventInfo.stageElements[stageEventInfo.ChapterOpenStage - 1];
                targetOpenStage.ClearEvent.AddListener(
                    (index) => OpenChapter(index + 1)
                    );

                //클리어 스테이지 이벤트 등록
                var targetclearstage = stageEventInfo.stageElements[stageEventInfo.StageCount - 1];
                targetclearstage.ClearEvent.AddListener(
                    (index) => ClearChapter(index)
                    );
            }

            //업적 이니셜라이징
            SetInitializeAchieve();
        }

        /// <summary>
        /// 게임스테이지의 길이를 받아옵니다.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetStageLength(GAME_INDEX index)
        {
            return StageEventSO[index].StageElements.Count;
        }

        /// <summary>
        /// 플레이어가 스테이지를 클리어했는지 여부를 받아옵니다.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="stageNum"></param>
        /// <returns></returns>
        public bool GetGameStageClear(GAME_INDEX index, int stageNum)
        {
            ////Old////
            ////게임을 클리어했으므로 UI팝업시키기
            //PlayerPrefs.SetInt(index.ToString() + stageNum.ToString(), 1);
            //saveData[index][stageNum] = 1;
            //if (stageNum >= targetStage[(int)index])
            //{
            //    onOpenChapter?.Invoke(index + 1);
            //}

            ///New///
            var _stringSaveKey = index.ToString() + stageNum.ToString() + isClearStageString;
            int cur_SaveValue = GetSaveDataValue(_stringSaveKey);

            return cur_SaveValue > 0 ? true : false;
        }

        /// <summary>
        /// 게임 스테이지가 해금된 상태인지 받아옵니다.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="stageNum"></param>
        /// <returns></returns>
        public bool GetGameStageOpen(GAME_INDEX index, int stageNum)
        {
            string key = index.ToString() + stageNum.ToString() + isOpenStageString;
            int value = GetSaveDataValue(key);
            return value > 0 ? true : false;
        }

        /// <summary>
        /// 인덱스 게임의 스테이지에 대한 클리어 여부를 변경합니다. (1 이상이면 클리어) 동시에 다음스테이지도 해금됩니다.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="stageNum"></param>
        /// <param name="saveValue"></param>
        public void SetGameStageClear(GAME_INDEX index, int stageNum, int saveValue)
        {
            //현재 스테이지 클리어정보 저장
            var _stringSaveKey = index.ToString() + stageNum.ToString() + isClearStageString;
            SetSaveDataValue(_stringSaveKey, saveValue);
            stageEventSO[index].StageElements[stageNum].ClearEvent?.Invoke(index);
            del_ClearGameStage?.Invoke(index, stageNum, saveValue > 0 ? true : false);


            //다음 스테이지 해금정보 저장
            var _stringSaveKey2 = index.ToString() + (stageNum + 1).ToString() + isOpenStageString;
            SetSaveDataValue(_stringSaveKey2, saveValue);
            del_OpenGameStage?.Invoke(index, stageNum + 1, saveValue > 0 ? true : false);
        }



        //Old
        ///// 해당게임에서 해당 스테이지의 클리어 여부를 받아옵니다.
        ///// <param name="index"></param>
        ///// <param name="stageNum"></param>
        //public static bool GetStageClearDataPerGame(GAME_INDEX index, int stageNum)
        //{
        //    var instacne = PlayerData.instance;
        //    int value = instance.saveData[index][stageNum];
        //    bool retVal = false;
        //    if (value > 0)
        //    {
        //        retVal = true;
        //    }
        //    else
        //    {
        //        retVal = false;
        //    }
        //    return retVal;
        //}

        /// <summary>
        /// 해당 게임의 모든 스테이지의 클리어 여부를 불린 배열로 받아옵니다.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool[] GetStageClearDataPerGame(GAME_INDEX index)
        {
            int count = stageEventSO[index].StageElements.Count;
            bool[] retVal = new bool[count];
            for (int i = 0; i < count; i++)
            {
                retVal[i] = GetGameStageClear(index, i);
            }
            return retVal;
        }

        /// <summary>
        /// 해당 게임모드의 클리어율을 소수점으로 반환합니다.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public float GetGameIndexClearRate(GAME_INDEX index)
        {
            bool[] arr = GetStageClearDataPerGame(index);
            float rate = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i])
                {
                    rate++;
                }
            }
            float retVal = rate / (float)arr.Length;
            return retVal;
        }
        #endregion

        /// <summary>
        /// 모든 스테이지의 게임클리어율을 반환합니다.
        /// </summary>
        /// <returns></returns>
        private float GetWholeClearRate()
        {
            float stageCount = 0;
            float clearCount = 0;
            for (int i = 0; i < (int)GAME_INDEX.None; i++)
            {
                var arr = GetStageClearDataPerGame((GAME_INDEX)i);
                stageCount += arr.Length;
                for (int j = 0; j < arr.Length; j++)
                {
                    if (arr[j])
                    {
                        clearCount++;
                    }
                }
            }
            float retVal = clearCount / stageCount;
            if (retVal >= 1)
            {
                PlayerDataXref.instance.SetAchieveSuccess(ACHEIVE_INDEX.ALL_CLEAR);
            }
            return retVal;
        }

        /// <summary>
        /// 플레이어프랩스로부터 저장된 데이터를 받아옵니다.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private int GetSaveDataValue(string key)
        {
            int retVal = 0;
            if (PlayerPrefs.HasKey(key))
            {
                retVal = PlayerPrefs.GetInt(key);
            }
            else
            {
                retVal = PlayerPrefs.GetInt(key, 0);
            }
            //Debug.LogWarning($"LOAD KEY : {key} -- Value : {retVal}");
            return retVal;
        }

        /// <summary>
        /// 플레이어 프랩스에 value값을 저장합니다.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void SetSaveDataValue(string key, int value)
        {
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.SetInt(key, value);
            }
            else
            {
                PlayerPrefs.SetInt(key, value);
            }
            //Debug.LogWarning($"SAVE KEY : {key} -- Value : {value}");
        }
        #endregion
    }
}