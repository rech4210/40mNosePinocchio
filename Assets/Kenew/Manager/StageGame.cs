using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.SceneManagement;
using UnityEngine;

public enum StageFailedReason { TimeOut, EnemyPush }
public enum GameState {Pause, Play, GameOver, Clear}

public class StageGame : MonoSingleton<StageGame>
{
    protected int currentStage = 1;
    protected int finalStage = 10;
    
    public bool possibleClick = true;

    public GameState gameState = GameState.Play;
    
    public virtual void StageInit()
    {
        
    }

    public virtual void StageFailed(StageGame stageGame)
    {
        gameState = GameState.GameOver;
    }
    // 타이머, 루틴UI
}
