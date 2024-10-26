using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.InputSystem;

public enum SkillEnum
{
    none, //Need to add a none for highscore importing
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
    private VisualElement document;

    public InputActionAsset inputActions;

    void Awake()
    {
        document = GetComponent<UIDocument>().rootVisualElement;
        playButton = document.Q<Button>("PlayGame");

        skillLabel = document.Q<Label>("SkillLabel");
        skillDesc = document.Q<Label>("SkillDesc");
        skillTimers = document.Q<Label>("SkillTimers");

        playButton.style.backgroundColor = new StyleColor(new Color(0.5f, 0.5f, 0.5f)); //Turn gray while loading
        Button skill1Button = document.Q<Button>("Skill1");
        skill1Button.RegisterCallback<ClickEvent, SkillEnum>(SkillClick, SkillEnum.dash);
        skill1Button.RegisterCallback<KeyDownEvent, SkillEnum>(SkillClick, SkillEnum.dash);
        Button skill2Button = document.Q<Button>("Skill2");
        skill2Button.RegisterCallback<ClickEvent, SkillEnum>(SkillClick, SkillEnum.vanish);
        skill2Button.RegisterCallback<KeyDownEvent, SkillEnum>(SkillClick, SkillEnum.vanish);
        Button skill3Button = document.Q<Button>("Skill3");
        skill3Button.RegisterCallback<ClickEvent, SkillEnum>(SkillClick, SkillEnum.decoy);
        skill3Button.RegisterCallback<KeyDownEvent, SkillEnum>(SkillClick, SkillEnum.decoy);
        Button backButton = document.Q<Button>("Return");
        backButton.RegisterCallback<ClickEvent>(ReturnToModeMenu);
        backButton.RegisterCallback<KeyDownEvent>(ReturnToModeMenu);
        Load();
    }

    private void Load()
    {
        string json = Resources.Load<TextAsset>("skills").text;
        SkillList skillsJson = JsonUtility.FromJson<SkillList>(json);
        skillList = skillsJson.skills;
    }

    public void PlayGame(EventBase evt)
    {
        if (SubmitCheck.Submit(evt, inputActions))
        {
            GameSettings.gameState = GameState.InGame;
            StartCoroutine(LoadScene("MainScene"));
        }
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
    public void SkillClick(EventBase evt, SkillEnum skillEnum)
    {
        if (SubmitCheck.Submit(evt, inputActions))
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
            playButton.RegisterCallback<KeyDownEvent>(PlayGame);
            playButton.style.backgroundColor = new StyleColor(new Color(88, 255, 88, 255));
            GameSettings.activeSkill = skillEnum;
            Debug.Log(GameSettings.activeSkill);
        }
    }

    private void ReturnToModeMenu(EventBase evt)
    {
        if (SubmitCheck.Submit(evt, inputActions))
        {            
            GameSettings.activeSkill = SkillEnum.dash;
            playButton.style.backgroundColor = new StyleColor(new Color(0.5f, 0.5f, 0.5f));
            playButton.UnregisterCallback<ClickEvent>(PlayGame);
            playButton.UnregisterCallback<KeyDownEvent>(PlayGame);
            if (document != null)
            {
                document.style.display = DisplayStyle.None;
            }
            showModeMenu();
        }
    }

    private void showModeMenu()
    {
        Scene modeScene = SceneManager.GetSceneByName("ModeSelect");
        if (modeScene.IsValid())
        {
            GameObject[] rootObjects = modeScene.GetRootGameObjects(); // Gets an array of all the objects in the scene that aren't inside other objects
            UIDocument uiDocument = rootObjects
                .Select(obj => obj.GetComponent<UIDocument>())
                .FirstOrDefault(doc => doc != null); // Checking each object to see if it has a UIDocument component, and if it does, it returns it
            
            if (uiDocument != null)
            {
                VisualElement rootElement = uiDocument.rootVisualElement; // Getting the root visual element of the UI document
                rootElement.style.display = DisplayStyle.Flex;
            }
        }
    }
}
