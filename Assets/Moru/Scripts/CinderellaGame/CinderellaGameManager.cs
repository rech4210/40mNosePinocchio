using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using PD;

namespace Moru.Cinderella
{
    public class CinderellaGameManager : SingleToneMono<CinderellaGameManager>
    {
        [BoxGroup("테스트모드?")] [SerializeField] private bool isTest = true;
        [BoxGroup("테스트모드?"), ShowIf("isTest")] public int GameStageNum;


        [BoxGroup("스프라이트 조각")] [SerializeField] private Sprite[] Atype;
        [BoxGroup("스프라이트 조각")] [SerializeField] private Sprite[] Btype;
        [BoxGroup("스프라이트 조각")] [SerializeField] private Sprite[] Ctype;
        [BoxGroup("스프라이트 조각")] [SerializeField] private Sprite[] Dtype;



        [BoxGroup("스프라이트 답")] [SerializeField] private Transform Atype_parent;
        [BoxGroup("스프라이트 답")] [SerializeField] private Transform[] Atype_Answer;

        [BoxGroup("스프라이트 답")] [SerializeField] private Transform Btype_parent;
        [BoxGroup("스프라이트 답")] [SerializeField] private Transform[] Btype_Answer;

        [BoxGroup("스프라이트 답")] [SerializeField] private Transform Ctype_parent;
        [BoxGroup("스프라이트 답")] [SerializeField] private Transform[] Ctype_Answer;

        [BoxGroup("스프라이트 답")] [SerializeField] private Transform Dtype_parent;
        [BoxGroup("스프라이트 답")] [SerializeField] private Transform[] Dtype_Answer;

        [BoxGroup("스프라이트 정답 오프셋")] [SerializeField] private float[] Collect_Offset = new float[4];

        [BoxGroup("타이머")] [SerializeField] private float[] timerList = new float[20];

        [BoxGroup("캔버스 사이즈")] [SerializeField] private float xSize;
        [BoxGroup("캔버스 사이즈")] [SerializeField] private float ySize;

        [BoxGroup("현재 게임")]
        [SerializeField] Transform PuzzelPivot;
        [BoxGroup("현재 게임")] [SerializeField] Transform[] selectedAnswer;
        [BoxGroup("현재 게임")] [SerializeField] float cur_Offset;
        [BoxGroup("현재 게임")] [SerializeField] float maxTimer;
        [BoxGroup("현재 게임")] [SerializeField] float cur_Timer;
        [BoxGroup("현재 게임")] [SerializeField] private AudioClip BGM;
        [BoxGroup("현재 게임")] [SerializeField] private AudioClip PickUp;
        [BoxGroup("현재 게임")] [SerializeField] private AudioClip Pull;



        [BoxGroup("부가 오브젝트들"), SerializeField, LabelText("신데렐라")] private SpriteRenderer Cinderella;
        [BoxGroup("부가 오브젝트들"), SerializeField, LabelText("계모")] private SpriteRenderer StepMom;
        [BoxGroup("부가 오브젝트들"), SerializeField, LabelText("실패 시")] private Sprite[] failSprite;
        [BoxGroup("부가 오브젝트들"), SerializeField, LabelText("시작타이머")] private Text startText;
        [BoxGroup("부가 오브젝트들"), SerializeField, LabelText("성공 UI")] private GameObject ClearUI;
        [BoxGroup("부가 오브젝트들"), SerializeField, LabelText("실패 UI")] private GameObject FailUI;
        [BoxGroup("부가 오브젝트들"), SerializeField, LabelText("타이머 UI")] private GameObject TimerUI;
        [BoxGroup("부가 오브젝트들"), SerializeField, LabelText("타겟성공 UI")] private Image targetClearUI;

        [LabelText("게임 실패수"), SerializeField] private static int failCount;
        public GameObject selectedPiece;

        private int pieceCount;

        public bool isGameOver;
        public bool isStopAndGoOtherPage;
        #region Events
        public delegate void OnValueChange(float cur, float max);
        public event OnValueChange onTimerValueChange;
        #endregion


        private void Start()
        {
            if (!isTest)
            {
                GameStageNum = PlayerDataXref.instance.GetCurrentStage().StageNum;
            }
            if (GameStageNum < 0) { Debug.Log($"잘못된 스테이지 넘버, 기능종료"); return; }
            else if (GameStageNum < 2)
            {
                SetInit(Atype);
                Atype_parent.gameObject.SetActive(true);
                selectedAnswer = Atype_Answer;
                cur_Offset = Collect_Offset[0];
            }
            else if (GameStageNum < 2+3)
            {
                SetInit(Btype);
                Btype_parent.gameObject.SetActive(true);
                selectedAnswer = Btype_Answer;
                cur_Offset = Collect_Offset[1];
            }
            else if (GameStageNum < 2+3+3)
            {
                SetInit(Ctype);
                Ctype_parent.gameObject.SetActive(true);
                selectedAnswer = Ctype_Answer;
                cur_Offset = Collect_Offset[2];
            }
            else if (GameStageNum < 2+3+3+2)
            {
                SetInit(Dtype);
                Dtype_parent.gameObject.SetActive(true);
                selectedAnswer = Dtype_Answer;
                cur_Offset = Collect_Offset[3];
            }

            pieceCount = selectedAnswer.Length;
            maxTimer = timerList[GameStageNum];
            cur_Timer = maxTimer;
            TimerUI.AddComponent<MoruTimer>();
            isGameOver = true;
            isStopAndGoOtherPage = false;
            SoundManager.PlayBGM(BGM);
            StartCoroutine(StartTextAnim());
        }

        private void SetInit(Sprite[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                var obj = new GameObject(arr[i].name);
                obj.transform.SetParent(PuzzelPivot);
                float x = Random.Range(-xSize, xSize);
                float y = Random.Range(-ySize, ySize);
                obj.transform.position = new Vector3(x, y, 0);

                var comp = obj.AddComponent<SpriteRenderer>();
                comp.sprite = arr[i];
                comp.sortingOrder = 3;

                var customComp = obj.AddComponent<PuzzelPiece>();
                customComp.Init(i);

                var collider = obj.AddComponent<PolygonCollider2D>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.timeScale == 0) return;
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit)
                {
                    if (hit.transform.gameObject.TryGetComponent<PuzzelPiece>(out var comp))
                    {
                        if (comp.isCanMoved)
                        {
                            selectedPiece = hit.transform.gameObject;
                            SoundManager.PlaySFX(PickUp);
                        }
                    }
                }

            }
            if (selectedPiece != null)
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                selectedPiece.transform.position = new Vector3(pos.x, pos.y, 0);
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (selectedPiece != null)
                {
                    var comp = selectedPiece.GetComponent<PuzzelPiece>();
                    selectedPiece = null;
                    if (comp.CheckCollect(selectedAnswer[comp.myIndex], cur_Offset))
                    {
                        pieceCount--;
                        SoundManager.PlaySFX(Pull);
                    }
                }
            }

            if (cur_Timer > 0 && !isGameOver)
            {
                cur_Timer -= Time.deltaTime;
                onTimerValueChange?.Invoke(cur_Timer, maxTimer);
            }
            else if (cur_Timer <= 0 && !isGameOver)
            {
                //게임 종료
                isGameOver = true;
                SetGameOver();
            }

            if (pieceCount <= 0 && !isGameOver)
            {
                isGameOver = true;
                SetGameClear();
            }

            if (isGameOver && Input.anyKey && isStopAndGoOtherPage)
            {
                //메인페이지로 돌아가기
            }
        }

        private void SetGameClear()
        {
            Debug.Log($"게임 클리어!");
            //플레이어 데이터 반영
            ClearUI?.SetActive(true);
            //Old
            //if (PlayerData.instance != null)
            //{
            //    PlayerData.onClearGame(GAME_INDEX.Cinderella, GameStageNum);
            //}
            //New
            //플레이어 데이터에 게임클리어를 업데이트합니다.
            PlayerDataXref.instance.ClearGame(GAME_INDEX.Cinderella, GameStageNum);
            WSceneManager.instance.OpenGameClearUI();
            

            //다음챕터을 엽니다. 다른분들도 이렇게 해주시면 되요
            if(GameStageNum == PlayerDataXref.instance.GetTargetState_ToOpenNextChapter(GAME_INDEX.Cinderella))
            {
                targetClearUI.gameObject.SetActive(true);
                PlayerDataXref.instance.OpenChapter(GAME_INDEX.Cinderella + 1);
            }
            else
            {
                targetClearUI.gameObject.SetActive(false);
            }

            //올클리어  & 1회도 실패하지 않고 클리어시 업적 이벤트 예시
            if (GameStageNum == PlayerDataXref.instance.GetMaxStageNumber(GAME_INDEX.Cinderella)- 1)
            {
                PlayerDataXref.instance.SetAchieveSuccess(ACHEIVE_INDEX.CINDERELLA_ALL_CLEAR);
                if (failCount == 0)
                {
                    PlayerDataXref.instance.SetAchieveSuccess(ACHEIVE_INDEX.PUZZLE_MASTER);
                }
            }
        }

        private void SetGameOver()
        {
            Debug.Log($"게임 실패!");
            FailUI?.SetActive(true);
            Cinderella.sprite = failSprite[0];
            StepMom.sprite = failSprite[1];
            failCount++;
            WSceneManager.instance.OpenGameFailUI();
        }

        private IEnumerator StartTextAnim()
        {
            WaitForSeconds textAnimDelay = new WaitForSeconds(2);
            var startTextComponent = startText.GetComponent<Text>();

            startTextComponent.fontSize = 120;

            startText.text = "준비..";
            yield return textAnimDelay;

            for (int nowRepetitionIndex = 3; nowRepetitionIndex >= 1; nowRepetitionIndex--)
            {
                startTextComponent.fontSize = 300;

                startText.text = $"{nowRepetitionIndex}";

                while (startTextComponent.fontSize > 2)
                {
                    startTextComponent.fontSize -= 1;
                    yield return null;
                }
            }

            startTextComponent.fontSize = 120;
            startText.text = "시작!";
            yield return textAnimDelay;

            startText.text = "";
            isGameOver = false;

            yield return null;
        }

    }

    public class PuzzelPiece : MonoBehaviour
    {
        public bool isCanMoved;
        private int _myIndex;
        public int myIndex { get => _myIndex; }
        public void Init(int index)
        {
            _myIndex = index;
            isCanMoved = true;
        }

        public bool CheckCollect(Transform target, float offset)
        {
            if (target == null) return false;

            float distance = Vector2.Distance(this.transform.position, target.position);
            distance = Mathf.Abs(distance);

            if (distance < offset)
            {
                this.transform.position = target.position;
                isCanMoved = false;
                Destroy(this.GetComponent<PuzzelPiece>());
                Destroy(this.GetComponent<PolygonCollider2D>());
                return true;
            }
            else
            {
                return false;
            }

        }
    }

    public class MoruTimer : MonoBehaviour
    {
        Text text;
        private void Start()
        {
            CinderellaGameManager.Instance.onTimerValueChange += UpdateTimer;
            text = GetComponent<Text>();
        }

        void UpdateTimer(float _cur, float _max)
        {
            float m = _cur / 60;
            float s = _cur % 60;
            text.text = $"{m.ToString("F0")}:{s.ToString("F0")}";
        }
    }

}