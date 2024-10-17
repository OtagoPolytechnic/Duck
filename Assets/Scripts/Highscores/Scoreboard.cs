using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class Scoreboard : MonoBehaviour
{
    private const int MAX_SCORE_ENTRIES = 50; //This is a max for each type of highscore. The true max is 100 scores

    [HideInInspector] public HighscoreSaveData savedScores;
    public static Scoreboard Instance;
    //Dynamically get the company name and game name
    private string companyName => Application.companyName;
    private string gameName => Application.productName;

    //Generates path based on user system0
    //Windows path: AppData/LocalLow/DuckCompany/DuckGame
    //Adding all old save paths to convert them to the new format
    //Every time we change the save format, company name, or game name we need to add a new path so no users lose their highscores
    //This needs to be a returning method not a list so the dynamic companyName and gameName update properly
    private List<string> OldSavePaths()
    {
        return new List<string>
        {
            $"{Application.persistentDataPath.Replace(companyName, "DefaultCompany").Replace(gameName, "DuckGame")}/endlessHighscores.json", //The replacing is so it will work no matter what the name currently is
            $"{Application.persistentDataPath.Replace(companyName, "DefaultCompany").Replace(gameName, "DuckGame")}/finalBossHighscores.json",
            $"{Application.persistentDataPath.Replace(companyName, "DuckCompany").Replace(gameName, "Duck")}/endlessHighscores.json",
            $"{Application.persistentDataPath.Replace(companyName, "DuckCompany").Replace(gameName, "Duck")}/finalBossHighscores.json"
        };
    }
    private string savePath => $"{Application.persistentDataPath}/highscores.json";

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
        convertOldSaves();
        LoadScores();
        HighscoreUI.Instance.DisplayBossHighscores();
    }

    private void LoadScores()
    {
        savedScores = GetSavedScores();
        SortScores(savedScores.highscores);
    }

    private void convertOldSaves()
    {
        //Converts all old save files to the new format
        foreach (string oldSavePath in OldSavePaths())
        {
            //Only runs if the old save file exists
            if (File.Exists(oldSavePath))
            {
                HighscoreSaveData oldScores = GetSavedScores(oldSavePath);
                foreach (EntryData entry in oldScores.highscores)
                {
                    if (entry.entryScore > 0) //Filter any testing scores
                    {
                        //Adding any missing data from the old saves
                        if(oldSavePath.Contains("endless"))
                        {
                            if (entry.gameMode == GameMode.None)
                            {
                                entry.gameMode = GameMode.Endless;
                            }
                        }
                        else
                        {
                            if (entry.gameMode == GameMode.None)
                            {
                                entry.gameMode = GameMode.Boss;
                            }
                            if (entry.waveNumber == 25) //Assuming the player didn't die on the final boss. No way of telling with the old data
                            {
                                entry.endBossKilled = true;
                            }
                        }
                        if (entry.GameVersion == null)
                        {
                            entry.GameVersion = "Unknown"; //Old saves don't have the version number
                        }
                        AddEntry(entry);
                    }
                }
                File.Delete(oldSavePath);
            }
        }
    }

    //Public method to add an entry to the highscores
    public void AddEntry(EntryData entryData)
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

        //Check if there are too many entries for the game mode
        if (savedScores.highscores.Count(entry => entry.gameMode == entryData.gameMode) > MAX_SCORE_ENTRIES)
        {
            //If so remove the last entry with the same game mode
            for (int i = savedScores.highscores.Count - 1; i >= 0; i--)
            {
                if (savedScores.highscores[i].gameMode == entryData.gameMode)
                {
                    savedScores.highscores.RemoveAt(i);
                    break;
                }
            }
        }
        SaveScores(savedScores);
    }

    public HighscoreSaveData GetSavedScores(string tempSavePath = null)
    {
        if (tempSavePath == null)
        {
            tempSavePath = savePath;
        }
        string json;
        if (!File.Exists(tempSavePath))
        {
            //Writing in the base json structure to avoid null reference exceptions
            json = JsonUtility.ToJson(new HighscoreSaveData());
            File.WriteAllText(tempSavePath, json);
            return new HighscoreSaveData();
        }

        json = File.ReadAllText(tempSavePath);
        return JsonUtility.FromJson<HighscoreSaveData>(json);
    }

    private void SaveScores(HighscoreSaveData highscoreSaveData)
    {
        string json = JsonUtility.ToJson(highscoreSaveData, true);
        File.WriteAllText(savePath, json);
    }

    private void SortScores(List<EntryData> entries)
    {
        entries.Sort((x, y) => y.entryScore.CompareTo(x.entryScore));
    }

    //Check if the score is higher than the top score on that game modes list
    public bool CheckTopScore(int score, GameMode gameMode)
    {
        List<EntryData> highscores = savedScores.highscores.Where(entry => entry.gameMode == gameMode).ToList();
        if (highscores.Count == 0)
        {
            return true;
        }
        else if (score > highscores[0].entryScore)
        {
            return true;
        }
        return false;
    }

    //Check if the score is higher than the lowest score on that game modes list
    public bool CheckHighScore(int score, GameMode gameMode)
    {
        List<EntryData> highscores = savedScores.highscores.Where(entry => entry.gameMode == gameMode).ToList();
        if (highscores.Count < MAX_SCORE_ENTRIES)
        {
            return true;
        }
        else if (score > highscores[highscores.Count - 1].entryScore)
        {
            return true;
        }
        return false;
    }
}
