using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Scoreboard : MonoBehaviour
{
    private const int MAX_SCORE_ENTRIES = 50;

    [HideInInspector] public HighscoreSaveData bossSavedScores;
    [HideInInspector] public HighscoreSaveData endlessSavedScores;
    public static Scoreboard Instance;

    // Generates path based on user system0
    // Windows path: AppData\LocalLow\DefaultCompany\DuckGame
    private string endlessSavePath => $"{Application.persistentDataPath}/endlessHighscores.json";
    private string bossSavePath => $"{Application.persistentDataPath}/finalBossHighscores.json";

    void Awake()
    {
        //Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

        LoadScores();
        HighscoreUI.Instance.DisplayBossHighscores();
    }

    private void LoadScores()
    {
        bossSavedScores = GetSavedScores(bossSavePath);
        endlessSavedScores = GetSavedScores(endlessSavePath);
        SortScores(bossSavedScores.highscores);
        SortScores(endlessSavedScores.highscores);
    }

    //Public method to add an entry to one of the two highscore lists
    public void AddEntry(EntryData entryData, bool isEndless)
    {
        if (isEndless)
        {
            AddEntry(entryData, endlessSavedScores, endlessSavePath);
            HighscoreUI.Instance.DisplayEndlessHighscores();
        }
        else
        {
            AddEntry(entryData, bossSavedScores, bossSavePath);
            HighscoreUI.Instance.DisplayBossHighscores();
        }
    }


    private void AddEntry(EntryData entryData, HighscoreSaveData savedScores, string savePath)
    {
        //Either gets the correct position on the list or the end position
        int insertIndex = savedScores.highscores.Count;
        for (int i = 0; i < savedScores.highscores.Count; i++)
        {
            if (entryData.entryScore > savedScores.highscores[i].entryScore)
            {
                insertIndex = i;
                break;
            }
        }
        savedScores.highscores.Insert(insertIndex, entryData); //Inserts the entry into the list at the index

        //Remove last entry if list is too long
        if (savedScores.highscores.Count > MAX_SCORE_ENTRIES)
        {
            savedScores.highscores.RemoveAt(MAX_SCORE_ENTRIES);
        }
        SaveScores(savedScores, savePath);
    }
    public HighscoreSaveData GetSavedScores(string savePath)
    {
        string json;
        if (!File.Exists(savePath))
        {
            //Writing in the base json structure to avoid null reference exceptions
            json = JsonUtility.ToJson(new HighscoreSaveData());
            File.WriteAllText(savePath, json);
            return new HighscoreSaveData();
        }

        json = File.ReadAllText(savePath);
        return JsonUtility.FromJson<HighscoreSaveData>(json);
    }

    private void SaveScores(HighscoreSaveData highscoreSaveData, string savePath)
    {
        string json = JsonUtility.ToJson(highscoreSaveData, true);
        File.WriteAllText(savePath, json);
    }

    private void SortScores(List<EntryData> entries)
    {
        entries.Sort((x, y) => y.entryScore.CompareTo(x.entryScore));
    }

    //Check if the score is higher than the top score on the list
    public bool CheckTopScore(int score, bool isEndless)
    {
        if (isEndless)
        {
            if (endlessSavedScores.highscores.Count == 0)
            {
                return true;
            }
            else if (score > endlessSavedScores.highscores[0].entryScore)
            {
                return true;
            }
        }
        else
        {
            if (bossSavedScores.highscores.Count == 0)
            {
                return true;
            }
            else if (score > bossSavedScores.highscores[0].entryScore)
            {
                return true;
            }
        }
        return false;
    }

    //Check if the score is higher than the lowest score on the list
    public bool CheckHighScore(int score, bool isEndless)
    {
        if (isEndless)
        {
            if (endlessSavedScores.highscores.Count < MAX_SCORE_ENTRIES)
            {
                return true;
            }
            else if (score > endlessSavedScores.highscores[MAX_SCORE_ENTRIES - 1].entryScore)
            {
                return true;
            }
        }
        else
        {
            if (bossSavedScores.highscores.Count < MAX_SCORE_ENTRIES)
            {
                return true;
            }
            else if (score > bossSavedScores.highscores[MAX_SCORE_ENTRIES - 1].entryScore)
            {
                return true;
            }
        }
        return false;
    }
}
