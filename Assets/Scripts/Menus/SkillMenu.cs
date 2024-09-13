using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public enum SkillEnum
{
    dash,
    vanish,
    decoy
}
public class SkillMenu : MonoBehaviour
{
    private Button playButton;
    private Label skillLabel;
    private Label skillDesc;
    private Label skillTimers;
    private List<Skill> skillList = new List<Skill>();


    void Awake()
    {
        VisualElement document = GetComponent<UIDocument>().rootVisualElement;
        playButton = document.Q<Button>("PlayGame");

        skillLabel = document.Q<Label>("SkillLabel");
        skillDesc = document.Q<Label>("SkillDesc");
        skillTimers = document.Q<Label>("SkillTimers");

        playButton.style.backgroundColor = new StyleColor(new Color(0.5f, 0.5f, 0.5f)); //Turn gray while loading
        Button skill1Button = document.Q<Button>("Skill1");
        skill1Button.RegisterCallback<ClickEvent, SkillEnum>(SkillClick, SkillEnum.dash);
        Button skill2Button = document.Q<Button>("Skill2");
        skill2Button.RegisterCallback<ClickEvent, SkillEnum>(SkillClick, SkillEnum.vanish);
        Button skill3Button = document.Q<Button>("Skill3");
        skill3Button.RegisterCallback<ClickEvent, SkillEnum>(SkillClick, SkillEnum.decoy);
        Load();
    }
    private void Load()
    {
        string json = Resources.Load<TextAsset>("skills").text;
        SkillList skillsJson = JsonUtility.FromJson<SkillList>(json);
        skillList = skillsJson.skills;
    }

    public void PlayGame(ClickEvent click)
    {
        GameSettings.gameState = GameState.InGame;
        StartCoroutine(LoadScene("SkillsTesting(Delete)")); //change to main scene
    }

    IEnumerator LoadScene(string sceneName)
    {
        //TODO: Add loading screen if the load times start to get longer
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        playButton.text = "Loading";
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void SkillClick(ClickEvent click, SkillEnum skillEnum)
    {
        foreach (Skill s in skillList)
        {
            if (s.id == skillEnum)
            {
                skillLabel.text = s.name;
                skillDesc.text = s.desc;
                skillTimers.text = $"Cooldown: {s.cooldown} seconds. Duration: {s.duration} seconds.";
            }
        }

        playButton.RegisterCallback<ClickEvent>(PlayGame);
        playButton.style.backgroundColor = new StyleColor(new Color(88, 255, 88, 255));
        GameSettings.activeSkill = skillEnum;
        Debug.Log(GameSettings.activeSkill);
    }
}
