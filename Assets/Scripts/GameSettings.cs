using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//At the moment I think we only need the InGame and Paused ones because end game is the same as paused
public enum GameState
{
    InGame,
    Paused,
    EndGame,
    ItemSelect,
    BossVictory //For when the level 25 boss is killed
}

public static class GameSettings
{
    public static GameState gameState = GameState.InGame;
    public static int waveNumber;
    public static SkillEnum activeSkill = SkillEnum.dash;
}
