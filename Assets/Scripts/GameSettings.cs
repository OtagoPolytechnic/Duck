using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    InGame,
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

public static class GameSettings
{
    public static GameState gameState = GameState.InGame;
    public static int waveNumber;
    public static controlType controlType = controlType.Keyboard; //Default to keyboard. Can change this for the arcade build
}
