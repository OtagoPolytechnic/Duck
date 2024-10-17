using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameState
{
    InGame,
    Dead,
    Paused,
    EndGame,
    ItemSelect,
    BossVictory //For when the level 25 boss is killed
}

public enum controlType
{
    Keyboard,
    Controller,
    Arcade
}

[Serializable]
public enum GameMode //There is an endless and a boss gamemode
{
    None,
    Boss,
    Endless
}

public static class GameSettings
{
    public static GameState gameState = GameState.InGame;
    public static int waveNumber;
    public static SkillEnum activeSkill = SkillEnum.dash;
    public static controlType controlType;
    public static GameMode gameMode = GameMode.Boss; //Boss mode by default if you start the game from the main scene
}
