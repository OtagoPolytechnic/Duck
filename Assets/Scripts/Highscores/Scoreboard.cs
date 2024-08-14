using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Scoreboard : MonoBehaviour
{
    private const int MAX_SCORE_ENTRIES = 50;
    public ScoreManager scoreManager;
    public ScoreInputField inputField;

    private HighscoreSaveData bossSavedScores;
    private HighscoreSaveData endlessSavedScores;

    // Generates path based on user system
    // Windows path: AppData\LocalLow\DefaultCompany\DuckGame
    private string endlessSavePath => $"{Application.persistentDataPath}/endlessHighscores.json";
    private string bossSavePath => $"{Application.persistentDataPath}/finalBossHighscores.json";

    private void Start()
    {
        //UpdateUI(savedScores);
        LoadScores();
    }

    private void LoadScores()
    {
        bossSavedScores = GetSavedScores(bossSavePath);
        endlessSavedScores = GetSavedScores(endlessSavePath);
    }

    //Public method to add an entry to one of the two highscore lists
    public void AddEntry(EntryData entryData, bool isEndless)
    {
        if(isEndless)
        {
            AddEntry(entryData, endlessSavedScores, endlessSavePath);
        }
        else
        {
            AddEntry(entryData, bossSavedScores, bossSavePath);
        }
    }

    
    private void AddEntry(EntryData entryData, HighscoreSaveData savedScores, string savePath)
    {
        //Either gets the correct position on the list or the end position
        int insertIndex = savedScores.highscores.Count;
        for (int i = 0; i < savedScores.highscores.Count; i++)
        {
            if(entryData.entryScore > savedScores.highscores[i].entryScore)
            {
                insertIndex = i;
                break;
            }
        }
        savedScores.highscores.Insert(insertIndex, entryData); //Inserts the entry into the list at the index

        //Remove last entry if list is too long
        if(savedScores.highscores.Count > MAX_SCORE_ENTRIES)
        {
            savedScores.highscores.RemoveAt(MAX_SCORE_ENTRIES);
        }
        //TODO: Change UI
        SaveScores(savedScores, savePath);
    }
    public HighscoreSaveData GetSavedScores(string savePath)
    {
        if (!File.Exists(savePath))
        {
            //Writing in the base json structure to avoid null reference exceptions
            string json = JsonUtility.ToJson(new HighscoreSaveData());
            File.WriteAllText(savePath, json);
            return new HighscoreSaveData();
        }

        string json = File.ReadAllText(savePath);
        return JsonUtility.FromJson<HighscoreSaveData>(json);
    }

    private void SaveScores(HighscoreSaveData highscoreSaveData, string savePath)
    {
        string json = JsonUtility.ToJson(highscoreSaveData, true);
        File.WriteAllText(savePath, json);
    }

    private void UpdateUI(HighscoreSaveData savedScores)
    {
        foreach (Transform child in scoreContainerTransform)
        {
            Destroy(child.gameObject);
        }
        if (savedScores == null)
        {
            savedScores = new HighscoreSaveData();
        }
        foreach(EntryData highscore in savedScores.highscores)
        {
            Instantiate(highscoreEntry, scoreContainerTransform).
                GetComponent<HighscoreUI>().Intialise(highscore);
        }
    }
}
