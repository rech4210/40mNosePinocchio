using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Game.JackNStemp
{
    public enum GamePhase
    {
        /// <summary>
        /// 잠시 대기 (콩넣는중)
        /// </summary>
        WAIT,
        /// <summary>
        /// 섞는 중
        /// </summary>
        MIX,
        /// <summary>
        /// 플레이어가 고르는 중
        /// </summary>
        SELECT,
        /// <summary>
        /// 정답을 맞춤
        /// </summary>
        CORRECT,
        /// <summary>
        /// 게임 종료
        /// </summary>
        END,
        /// <summary>
        /// 게임 클리어
        /// </summary>
        CLEAR,
        /// <summary>
        /// NONE
        /// </summary>
        MAX

    }

    [System.Serializable]
    public class GameLevelDesign
    {
        [LabelText("스테이지 진행회수"), SerializeField]
        private SettingValue[] _settingValue = new SettingValue[5];
        public SettingValue[] SettingValues => _settingValue;
        [System.Serializable]
        public class SettingValue
        {
            [SerializeField, LabelText("컵 개수")] private uint cupCount = 3;
            [SerializeField, LabelText("섞는 회수")] private uint mixCount = 10;
            [SerializeField, LabelText("이동시간")] private float mixSpeed = 1f;
            [SerializeField, LabelText("정답 시간")] private float maxTime = 5f;

            public int CupCount => (int)cupCount;
            public int MixCount => (int)mixCount;
            public float MixSpeed => mixSpeed;
            public float MaxTime => maxTime;
        }
    }

    public class Cup : MonoBehaviour
    {
        public SpriteRenderer cup;
        public Sprite[] cupState;
        public SpriteRenderer other;
        private Sequence sq;
        private void Awake()
        {
            cup = GetComponent<SpriteRenderer>();
            other = transform.GetChild(0).GetComponent<SpriteRenderer>();
            other.color = new Color(1, 1, 1, 0);
        }

        public void Input()
        {
            other.color = new Color(1, 1, 1, 1);
            cup.sprite = cupState[1];
            sq = DOTween.Sequence();
            sq.Append(other.gameObject.transform.DOLocalMoveY(-1.93f, 2).From(4));
            sq.Append(other.DOColor(new Color(1, 1, 1, 0), 0.5f));
            sq.AppendCallback(() => cup.sprite = cupState[0]);
            sq.Play();
            sq.SetAutoKill(true);
        }

        public void Show()
        {
            cup.sprite = cupState[1];
            other.color = new Color(1, 1, 1, 1);
        }

        public void hide()
        {
            cup.sprite = cupState[0];
            other.color = new Color(1, 1, 1, 0);
        }
    }



    public class JackAndStempGameManager : MonoBehaviour
    {
        [SerializeField, BoxGroup("게임 상태 관리"), LabelText("현재 스테이지 번호")] int curStage;
        [SerializeField, BoxGroup("게임 상태 관리"), LabelText("스테이지 내 페이즈 번호")] int phaseIndex;
        [SerializeField, BoxGroup("게임 상태 관리"), LabelText("현재 게임상태")] bool isGameOver;
        bool isGameEnd;
        [SerializeField, BoxGroup("게임 상태 관리"), LabelText("현재 게임상태"), ReadOnly] GamePhase cur_Phase;
        [SerializeField, BoxGroup("게임 상태 관리"), LabelText("인풋 게임상태")] public GamePhase input_Phase;
        [SerializeField, BoxGroup("부가 오브젝트"), LabelText("컵 부모")] private Transform cups_Parent;
        [SerializeField, BoxGroup("부가 오브젝트"), LabelText("컵들")] private List<Cup> cups;
        [SerializeField, BoxGroup("부가 오브젝트"), LabelText("현재컵들")] private List<GameObject> cur_cups;
        [SerializeField, BoxGroup("부가 오브젝트"), LabelText("정답 컵")] private Cup correctCup;
        [SerializeField, BoxGroup("부가 오브젝트"), LabelText("게임 스타트 카운터")] private Text startCounterUI;
        [SerializeField, BoxGroup("부가 오브젝트"), LabelText("셀렉트 카운터")] private Text selectTimer;
        public float yOffset = -3f;
        public float xWide = 3f;

        [SerializeField, BoxGroup("게임레벨 관리"), LabelText("스테이지 디자인")]
        private GameLevelDesign[] stages;
        GameLevelDesign cur_Stage => stages[curStage];
        GameLevelDesign.SettingValue cur_SettingValue => cur_Stage.SettingValues[phaseIndex];

        [SerializeField, BoxGroup("디자인"), LabelText("컵 스프라이트")] private Sprite[] cupSprites;
        [SerializeField, BoxGroup("디자인"), LabelText("콩 스프라이트")] private Sprite correctSprite;
        [SerializeField, BoxGroup("디자인"), LabelText("콩나물 스프라이트")] private Sprite wrongSprite;

        public class GameEvent : UnityEvent { }
        [ShowInInspector]
        public Dictionary<GamePhase, GameEvent> _gameEventPair = new Dictionary<GamePhase, GameEvent>();

        // Start is called before the first frame update
        void Start()
        {
            curStage = PlayerDataXref.pl.CurStageSelectedNum;
            phaseIndex = 0;
            isGameOver = true;
            isGameEnd = false;
            input_Phase = GamePhase.WAIT;
            cur_Phase = GamePhase.MAX;
            for (int i = 0; i < (int)GamePhase.MAX; i++)
            {
                _gameEventPair.Add((GamePhase)i, new GameEvent());
            }
            _gameEventPair[GamePhase.WAIT].AddListener(WaitEvent);
            _gameEventPair[GamePhase.MIX].AddListener(MixEvent);
            _gameEventPair[GamePhase.SELECT].AddListener(SelectEvent);
            _gameEventPair[GamePhase.CORRECT].AddListener(CorrectEvent);
            _gameEventPair[GamePhase.END].AddListener(EndEvent);
            _gameEventPair[GamePhase.CLEAR].AddListener(ClearEvent);

            for (int i = 0; i < cups_Parent.childCount; i++)
            {
                var obj = cups_Parent.GetChild(i).gameObject;
                cups.Add(obj.AddComponent<Cup>());
                obj.SetActive(false);
            }
            StartCoroutine(StartTextAnim());
        }

        // Update is called once per frame
        void Update()
        {
            if (isGameOver && !isGameEnd) return;
            if (cur_Phase != input_Phase)
            {
                cur_Phase = input_Phase;
                _gameEventPair[cur_Phase]?.Invoke();
            }
            if (cur_Phase == GamePhase.SELECT)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    var rayVec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    var RaycastResult = Physics2D.Raycast(rayVec, transform.forward);
                    if (!RaycastResult) return;
                    if (RaycastResult.collider.gameObject.TryGetComponent<Cup>(out var comp))
                    {
                        foreach (var cups in cur_cups)
                        {
                            cups.GetComponent<Cup>().Show();
                        }
                        if (comp == correctCup)
                        { 
                            bool isLastGame = phaseIndex >= cur_Stage.SettingValues.Length ? true : false;
                            if (isLastGame)
                            {
                                input_Phase = GamePhase.CLEAR;
                            }
                            else
                            {
                                input_Phase = GamePhase.CORRECT;
                            }
                        }
                        else
                        {
                            input_Phase = GamePhase.END;
                        }

                    }
                }
            }

        }

        /// <summary>
        /// 대기시간동안에 발생하는 이벤트
        /// </summary>
        public void WaitEvent()
        {
            selectTimer.text = $"남은 게임 : {cur_Stage.SettingValues.Length - (phaseIndex)}";
            StartCoroutine(Wait());
            //콩 넣기
            IEnumerator Wait()
            {
                int cupCount = cur_SettingValue.CupCount;
                List<GameObject> activesList = new List<GameObject>();
                for (int i = 0; i < cups.Count; i++)
                {
                    bool setActiveValue = i < cupCount ? true : false;
                    cups[i].gameObject.SetActive(setActiveValue);
                    if (setActiveValue)
                    {
                        activesList.Add(cups[i].gameObject);
                    }
                }
                cur_cups = activesList;
                //위치 정렬
                float _startXPos = xWide * -1;
                float endXPos = xWide;
                float xlength = endXPos - _startXPos;
                float xAddValue = xlength / (activesList.Count - 1);
                Debug.Log($"{xAddValue} //{xlength}");

                //정답 정하기
                int beanIndex = Random.Range(0, activesList.Count);
                correctCup = cups[beanIndex];

                for (int i = 0; i < activesList.Count; i++)
                {
                    Vector3 posValue = new Vector3(_startXPos + (xAddValue * i), yOffset, 0);
                    activesList[i].transform.position = posValue;
                    cups[i].cupState = cupSprites;
                    if (i == beanIndex)
                    {
                        cups[beanIndex].Input();
                        cups[beanIndex].other.sprite = correctSprite;
                    }
                    else
                    {
                        cups[i].other.sprite = wrongSprite;
                    }
                }

                yield return null;
                float waitTime = 0;
                while (waitTime < 2.5f)
                {
                    waitTime += Time.deltaTime;
                    yield return null;
                }


                input_Phase = GamePhase.MIX;
            }
        }


        /// <summary>
        /// 섞는 동안에 발생하는 이벤트
        /// </summary>
        public void MixEvent()
        {
            StartCoroutine(Co_MixCup());
        }
        /// <summary>
        /// 실제 믹스 코루틴
        /// </summary>
        /// <returns></returns>
        private IEnumerator Co_MixCup()
        {
            yield return null;
            int count = cur_SettingValue.MixCount;
            for (int i = 0; i < count; i++)
            {
                float t = 0;
                var mixedIndex = SetMixIndex();
                var obj1 = cur_cups[mixedIndex.first];
                Vector3 obj1_Pos = obj1.transform.position;

                var obj2 = cur_cups[mixedIndex.second];
                Vector3 obj2_Pos = obj2.transform.position;
                while (t < cur_SettingValue.MixSpeed)
                {
                    t += Time.deltaTime;
                    float lerpValue = t / cur_SettingValue.MixSpeed;
                    obj1.transform.position = Vector3.Lerp(obj1_Pos, obj2_Pos, lerpValue);
                    obj2.transform.position = Vector3.Lerp(obj2_Pos, obj1_Pos, lerpValue);
                    yield return null;
                }
                obj1.transform.position = obj2_Pos;
                obj2.transform.position = obj1_Pos;

                //약간의 대기시간
                t = 0;
                while (t < 0.3f)
                {
                    t += Time.deltaTime;
                    yield return null;
                }
            }
            input_Phase = GamePhase.SELECT;

            (int first, int second) SetMixIndex()
            {
                int first = Random.Range(0, cur_cups.Count);
                int second = first;
                //중복체크 무한뽑기 메커니즘이 기억이 안난다.
                while (first == second)
                {
                    second = Random.Range(0, cur_cups.Count);
                }
                return (first, second);
            }
        }

        /// <summary>
        /// 선택단계에서 이벤트
        /// </summary>
        public void SelectEvent()
        {
            StartCoroutine(CO_Select());
            IEnumerator CO_Select()
            {
                if (cur_Phase == GamePhase.SELECT)
                {
                    float t = 0;
                    float maxT = cur_SettingValue.MaxTime;
                    while (t < maxT)
                    {
                        t += Time.deltaTime;
                        //타이머 업데이트
                        selectTimer.text = $"제한시간 : {((int)(maxT- t)).ToString("D2")}";
                        yield return null;
                    }
                    input_Phase = GamePhase.END;
                }
            }
        }

        /// <summary>
        /// 정답을 맞췄을 때 발생하는 이벤트
        /// </summary>
        public void CorrectEvent()
        {
            phaseIndex++;
            foreach(var comp in cur_cups)
            {
                comp.GetComponent<Cup>().Show();
            }
            StartCoroutine(wait());
            IEnumerator wait()
            {
                float t = 0;
                float maxT = 2;
                while (t < maxT)
                {
                    t += Time.deltaTime;
                    yield return null;
                }
                foreach (var comp in cur_cups)
                {
                    comp.GetComponent<Cup>().hide();
                }
                input_Phase = GamePhase.WAIT;
            }

        }

        /// <summary>
        /// 게임종료 시 발생 이벤트
        /// </summary>
        public void EndEvent()
        {
            isGameOver = true;
            isGameEnd = true;
            WSceneManager.instance.OpenGameFailUI();
        }

        /// <summary>
        /// 스테이지를 클리어하면 발생하는 이벤트
        /// </summary>
        public void ClearEvent()
        {
            isGameOver = true;
            isGameEnd = true;
            WSceneManager.instance.OpenGameClearUI();
            PlayerDataXref.instance.ClearGame(GAME_INDEX.Jack_And_Beanstalk, curStage);

        }


        private IEnumerator StartTextAnim()
        {
            float waitTime = 2;
            float cur_T = 0;
            var startTextComponent = startCounterUI.GetComponent<Text>();
            startTextComponent.fontSize = 120;
            startCounterUI.text = "준비..";

            WaitForSecondsRealtime textAnimDelay = new WaitForSecondsRealtime(0.5f);
            while (cur_T < waitTime)
            {
                cur_T += Time.deltaTime;
                yield return null;
            }
            cur_T = 0;
            for (int i = 3; i > 0; i--)
            {
                startTextComponent.fontSize = 300;
                startTextComponent.text = i.ToString();
                while (cur_T < 1)
                {
                    startTextComponent.fontSize = (int)Mathf.Lerp(300, 0, cur_T);
                    cur_T += Time.deltaTime;
                    yield return null;
                }
                cur_T = 0;
            }
            startTextComponent.fontSize = 300;
            startCounterUI.text = "시작!";
            var cur_Color = startCounterUI.color;
            isGameOver = false;
            while (cur_T < 2)
            {
                cur_Color.a = Mathf.Lerp(1, 0, cur_T * 0.5f);
                startCounterUI.color = cur_Color;
                startTextComponent.fontSize = (int)Mathf.Lerp(300, 0, cur_T * 0.5f);
                cur_T += Time.deltaTime;
                yield return null;
            }
            startCounterUI.gameObject.SetActive(false);
        }
    }
}