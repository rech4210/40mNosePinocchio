using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.EditorTools;
#endif
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{

    private Dictionary<KeyCode, string> keyValuePairs = new Dictionary<KeyCode, string>(); // 키 저장한 딕셔너리

    public int signNumbers;// 나올 버튼의 개수

    GameObject makedsprite = null; // 생성 후 버튼 객체

    GameObject[] spritesArray = new GameObject[50]; //생성된 버튼 객체들을 다룰 배열
    public GameObject[] spritesPrefab; // 프리팹 이미지들 넣을 배열
    public GameObject extention; // 노트 갯수에 따라 확장되는 Panel

    public GameObject parentPrefab; // 복제할 부모

    GameObject[] spriteParents = new GameObject[10]; //생성된 부모 객체들
    ChangeAnimation changeScripts;
    ScoreManager scoreManager;


    AudioSource audioSource;
    public AudioClip hit_sound;
    public AudioClip miss_sound;



    int adjustPanelPos = 10;

    Vector3 defaultPos = new Vector3(0,0,0);

    #region 상태 반환
    public enum State //상태 enum
    {
        success,
        fail,
        normal,
        clear
    }
    private State _nowState;

    public State nowState // enum 생성자
    {
        get { return nowState; } //get 프로퍼티

        set
        {
            _nowState = value;
            changeScripts.changeAnimation(_nowState);  // state 값 set 할 시 changeAnimation 메소드 실행

        }
    }
    #endregion

    

    void Awake()
    {
        AddDictionary(); // 키 설정
        initallize(); // 초기 갯수에 따라 이미지 생성하는 메소드
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        extention.GetComponent<RectTransform>().sizeDelta = new Vector2((extention.GetComponent<RectTransform>().rect.width * (signNumbers / 5f) + (adjustPanelPos * signNumbers)), 1900);
        changeScripts = gameObject.GetComponent<ChangeAnimation>(); // changeAnimation 함수를 실행시킬 인스턴스 객체
        scoreManager = gameObject.GetComponent<ScoreManager>();
    }


    #region 노트 제어
    void initallize()
    {
        for (int i = 0; i < signNumbers; i++)
        {
            var parent = Instantiate(parentPrefab); // 부모 객체 생성

            parent.name = i.ToString();
            spriteParents[i] = parent; // 부모 배열에 객체 넣어주기
            spriteParents[i].transform.SetParent(this.gameObject.transform, false); // 매니저 아래에 부모 생성

            spriteParents[i].GetComponent<RectTransform>().anchoredPosition = gameObject.transform.position + new Vector3((i * 175) + 175, 6, 0);

            Vector3 tempParentPos = spriteParents[i].GetComponent<RectTransform>().anchoredPosition;

            int randomNumber = Random.Range(0, spritesPrefab.Length - 1); //랜덤 이미지 넘버
            var temp_sprite = Instantiate(spritesPrefab[randomNumber]); // 랜덤 이미지 생성
            spritesArray[i] = temp_sprite;

            temp_sprite.transform.SetParent(spriteParents[i].transform, false); // i번째 부모에 이미지 넣어주기
            temp_sprite.GetComponent<RectTransform>().anchoredPosition = defaultPos;
        }
    }

    void makeSprite() // 스프라이트를 재생성하는 메소드
    {
        int randomNumber = Random.Range(0, spritesPrefab.Length - 1); //랜덤 이미지 넘버
        var temp_sprite = Instantiate(spritesPrefab[randomNumber]); // 랜덤 이미지 생성
        makedsprite = temp_sprite;
        spritesArray[signNumbers] = makedsprite;

    }

    void push(GameObject temp_sprite)
    {
        temp_sprite.transform.SetParent(spriteParents[signNumbers - 1].transform, false);
    }

    void Scroll() // 작동안될시 method로 변경
    {
        for (int i = 0; i < signNumbers; i++)
        {//삭제후 배열을 한칸씩 당기고, 위치를 바꾼다 그리고 마지막 자리에 push
            spritesArray[i] = spritesArray[i + 1]; // push를 먼저 해줘야함
            spritesArray[i].transform.SetParent(spriteParents[i].transform, false); //한칸씩 부모에게 당기기 스프라이트 1 -> 0으로
        }
    } 
    #endregion
    #region 키 입력
    private void Update()
    {
        if (Input.anyKeyDown) // 다운
        {
            isKeyDown();
        }
    }


    void isKeyDown()
    {
        foreach (var dic in keyValuePairs)
        {
            if (Input.GetKey(dic.Key))
            {
                isRight(dic.Value); // 키보드 입력에 따른 비교 구현
            }
        }
    }

    public void AddDictionary()
    {
        keyValuePairs.Add(KeyCode.DownArrow, "Down(Clone)");
        keyValuePairs.Add(KeyCode.LeftArrow, "Left(Clone)");
        keyValuePairs.Add(KeyCode.RightArrow, "Right(Clone)");
        keyValuePairs.Add(KeyCode.UpArrow, "Up(Clone)");
        keyValuePairs.Add(KeyCode.Space, "Space(Clone)");

    } 
    #endregion

    void isRight(string keyString)
    {
        var temp_sprite = spriteParents[0].transform.GetChild(0);
        if (keyString == temp_sprite.name && Time.timeScale != 0)
        {
            nowState = State.success; // 성공
            scoreManager.scoreDel(); // 성공시 스코어 점수 증가 델리게이트 실행

            SoundManager.PlaySFX(hit_sound);

            Destroy(temp_sprite.gameObject); // 첫번째 노드 삭제안됨 수정
            makeSprite(); // 생성
            Scroll(); // 부모 객체에 넣을 이미지 한칸씩 당기기
            push(makedsprite); // 마지막 부모 객체에 자식 이미지 넣기

        }
        else
        {
            scoreManager.failDel();

            SoundManager.PlaySFX(miss_sound);
            nowState = State.fail; // 실패
        }
    }


}
