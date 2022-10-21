using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{

    private Dictionary<KeyCode, string> keyValuePairs = new Dictionary<KeyCode, string>();

    public int singNumbers;// 나올 버튼의 개수

    GameObject sprite = null; // 버튼 객체
    GameObject[] spritesArray = new GameObject[50]; //생성된 버튼 객체들을 다룰 배열
    public GameObject[] spritesPrefab; // 프리팹 이미지들 넣을 배열
    public GameObject extention;

    public GameObject parentPrefab; // 복제할 부모

    GameObject[] spriteParents = new GameObject[10]; //생성된 부모 객체들
    ChangeAnimation changeScripts;

    Vector3 defaultPos = new Vector3(0,0,0);

    public enum State //상태 enum
    {
        success,
        fail,
        normal,
        clear
    }
    public State nowState // enum 생성자
    {
        get { return nowState; } //^^^ 전체적으로 오버플로우 뜸

        set 
        { 
            changeScripts.changeAnimation(nowState);  // state 값 set 할 시 changeAnimation 메소드 실행
        }
    }

    void Awake()
    {
        AddDictionary(); // 키 설정
        initallize(); // 초기 갯수에 따라 이미지 생성하는 메소드
    }

    private void Start()
    {

        extention.GetComponent<RectTransform>().sizeDelta = new Vector2(0.5f * (singNumbers/5), 0.15f); // 안변함
        changeScripts = gameObject.GetComponent<ChangeAnimation>(); // changeAnimation 함수를 실행시킬 인스턴스 객체
    }


    void initallize()
    {
        for (int i = 1; i < singNumbers+1; i++)
        {
            var parent = Instantiate(parentPrefab); // 부모 객체 생성

            parent.name = i.ToString();
            spriteParents[i - 1] = parent; // 부모 배열에 객체 넣어주기
            spriteParents[i - 1].transform.SetParent(this.gameObject.transform,false); // 매니저 아래에 부모 생성
            
            spriteParents[i - 1].GetComponent<RectTransform>().anchoredPosition = gameObject.transform.position + new Vector3(i*175, 6,0); // 부모 위치 지정 값 수정해야함

            Vector3 tempParentPos = spriteParents[i - 1].GetComponent<RectTransform>().anchoredPosition;

            int randomNumber = Random.Range(0, spritesPrefab.Length-1); //랜덤 이미지 넘버
            var temp_sprite = Instantiate(spritesPrefab[randomNumber]); // 랜덤 이미지 생성
            sprite = temp_sprite;
            spritesArray[i - 1] = sprite;

            sprite.transform.SetParent(spriteParents[i - 1 ].transform,false); // i번째 부모에 이미지 넣어주기
            sprite.GetComponent<RectTransform>().anchoredPosition = defaultPos;
        }
    }

    void makeSprite() // 스프라이트를 재생성하는 메소드
    {
        int randomNumber = Random.Range(0, spritesPrefab.Length - 1); //랜덤 이미지 넘버
        var temp_sprite = Instantiate(spritesPrefab[randomNumber]); // 랜덤 이미지 생성
        sprite = temp_sprite;
    }

    void push(GameObject temp_sprite)
    {
        temp_sprite.transform.SetParent(spriteParents[spriteParents.Length-1].transform,false);
    }

    void Scroll() // 작동안될시 method로 변경
    {
        for (int i = 1; i < singNumbers; i++)
        {
            spritesArray[i].transform.SetParent(spriteParents[i - 1].transform, false); //한칸씩 부모에게 당기기
            spriteParents[i].transform.position = defaultPos; // 위치값 초기화
        }
        spritesArray[singNumbers].transform.SetParent(spriteParents[singNumbers - 1].transform); //마지막 칸 할당
        spriteParents[singNumbers].transform.position = defaultPos; // 위치값 초기화

    }

    private void Update()
    {
        if (Input.anyKey)
        {
            isKeyDown();
        }
    }

    #region 키 입력
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
        keyValuePairs.Add(KeyCode.DownArrow, "Down");
        keyValuePairs.Add(KeyCode.LeftArrow, "Left");
        keyValuePairs.Add(KeyCode.RightArrow, "Right");
        keyValuePairs.Add(KeyCode.UpArrow, "Up");
        keyValuePairs.Add(KeyCode.Space, "Space");

    } 
    #endregion

    void isRight(string keyString)
    {
        if(keyString == sprite.name)
        {
            nowState = State.success; // 성공

            Destroy(sprite); // destroy 맞는지?
            makeSprite(); // 생성
            Scroll(); // 부모 객체에 넣을 이미지 한칸씩 당기기
            push(sprite); // 마지막 부모 객체에 자식 이미지 넣기

        }
        else
        {
            nowState = State.fail; // 실패
        }
    }

    //클리어 함수 추가
}
