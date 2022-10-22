using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum GAME_INDEX
{
    Pinocchio,          //피노키오
    Snow_White,         //백설공주
    Little_Mermaid,     //인어공주
    Cinderella,         //신데렐라
    Jack_And_Beanstalk, //잭과 콩나무
    Tree_Little_Pigs,    //아기 돼지 삼형제
    None
}

public enum ACHEIVE_INDEX
{
    /// <summary>
    /// 톱질장인 : 피노키오 올클리어
    /// </summary>
    PINOCCHIO_ALL_CLEAR,

    /// <summary>
    /// 유리공예장인 : 신데렐라 올클리어
    /// </summary>
    CINDERELLA_ALL_CLEAR,

    /// <summary>
    /// 사과감별사 : 백설공주 올클리어
    /// </summary>
    SNOW_WHITE_ALL_CLEAR,

    /// <summary>
    /// 약쟁이 : 인어공주 올클리어
    /// </summary>
    LITTLE_MERMAID_ALL_CLEAR,

    /// <summary>
    /// 자라나라나무나무 : 잭과 콩나무 올클리어
    /// </summary>
    JACK_AND_BEANSTALK_ALL_CLEAR,

    /// <summary>
    /// 벽돌인, 아기돼지삼형제 올클리어
    /// </summary>
    TREE_LITTLE_PIGS_ALL_CLEAR,

    /// <summary>
    /// 엔딩 메이커 : 모든 스토리 엔딩 해금
    /// </summary>
    END_MAKER,

    /// <summary>
    /// The End : 모든 스테이지 올클리어
    /// </summary>
    ALL_CLEAR,

    /// <summary>
    /// 퍼즐장인 : 신데렐라 플레이 중 1회도 패배하지 않는 경우
    /// </summary>
    PUZZLE_MASTER,

    /// <summary>
    /// 톱질 마스터 : 피노키오 플레이 중 1회도 패배하지 않는 경우
    /// </summary>
    SAWING_MASTER,

    /// <summary>
    /// 사과 소믈리에 : 백설공주 플레이 중 1회도 패배하지 않는 경우
    /// </summary>
    APPLE_SOMMELIER,

    /// <summary>
    /// 약왕 : 인어공주 플레이 중 1회도 패배하지 않는 경우
    /// </summary>
    DRUG_KING,

    /// <summary>
    /// 거목 : 잭과 콩나무 플레이 중 1회도 패배하지 않는 경우
    /// </summary>
    GAINT_BEANSTALK,

    /// <summary>
    /// 인간벽돌 : 아기돼지삼형제 플레이 중 1회도 패배하지 않는 경우
    /// </summary>
    HUMAN_BRICK,

    /// <summary>
    /// 가장 마지막 인덱스 번호입니다.
    /// </summary>
    NONE,

}

public enum CUTSCENE_INDEX
{ 
    FIST_OPEN_GAME,

    OPEN_SNOW_WHITE_CHAPTER,

    SUCCESS_SNOW_WHITE_CHAPTER,

    OPEN_CINDERELLA_CHAPTER,

    SUCCESS_CINDERELLA_CHAPTER,

    OPEN_PINOCCHIO_CHAPTER,

    SUCCESS_PINOCCHIO_CHAPTER,

    OEPN_LITTLE_MERMAID_CHAPTER,

    SUCCESS_LITTLE_MERMAID_CHAPTER,

    OPEN_JACK_AND_BEANSTALK_CHAPTER,

    SUCCESS_JACK_AND_BEANSTALK_CHAPTER,

    OPEN_TREE_LITTLE_PIGS_CHAPTER,

    SUCCESS_TREE_LITTLE_PIGS_CHAPTER,

    LAST_CLEAR,

    NONE

}
