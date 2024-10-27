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
    private Button skill1Button;
    private Button skill2Button;
    private Button skill3Button;
    private Button backButton;
    private List<Skill> skillList = new List<Skill>();
    private VisualElement document;

    void Awake()
    {
        document = GetComponent<UIDocument>().rootVisualElement;
        playButton = document.Q<Button>("PlayGame");

        skillLabel = document.Q<Label>("SkillLabel");
        skillDesc = document.Q<Label>("SkillDesc");
        skillTimers = document.Q<Label>("SkillTimers");

        playButton.style.backgroundColor = new StyleColor(new Color(0.5f, 0.5f, 0.5f)); //Turn gray while loading
        skill1Button = document.Q<Button>("Skill1");
        skill1Button.RegisterCallback<ClickEvent, SkillEnum>(SkillClick, SkillEnum.dash);
        skill1Button.RegisterCallback<NavigationSubmitEvent, SkillEnum>(SkillClick, SkillEnum.dash);
        skill2Button = document.Q<Button>("Skill2");
        skill2Button.RegisterCallback<ClickEvent, SkillEnum>(SkillClick, SkillEnum.vanish);
        skill2Button.RegisterCallback<NavigationSubmitEvent, SkillEnum>(SkillClick, SkillEnum.vanish);
        skill3Button = document.Q<Button>("Skill3");
        skill3Button.RegisterCallback<ClickEvent, SkillEnum>(SkillClick, SkillEnum.decoy);
        skill3Button.RegisterCallback<NavigationSubmitEvent, SkillEnum>(SkillClick, SkillEnum.decoy);
        backButton = document.Q<Button>("Return");
        backButton.RegisterCallback<ClickEvent>(ReturnToModeMenu);
        backButton.RegisterCallback<NavigationSubmitEvent>(ReturnToModeMenu);
        Load();
        navigationSetting();
    }

    private void Load()
    {
        string json = Resources.Load<TextAsset>("skills").text;
        SkillList skillsJson = JsonUtility.FromJson<SkillList>(json);
        skillList = skillsJson.skills;
    }

    public void PlayGame(EventBase evt)
    {
        GameSettings.gameState = GameState.InGame;
        StartCoroutine(LoadScene("MainScene"));
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

    private void navigationSetting()
    {
        skill1Button.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch(e.direction)
            {
                case NavigationMoveEvent.Direction.Up: backButton.Focus(); break;
                case NavigationMoveEvent.Direction.Down: playButton.Focus(); break;
                case NavigationMoveEvent.Direction.Left: skill3Button.Focus(); break;
                case NavigationMoveEvent.Direction.Right: skill2Button.Focus(); break;
            }
            e.PreventDefault();
        });

        skill2Button.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch(e.direction)
            {
                case NavigationMoveEvent.Direction.Up: backButton.Focus(); break;
                case NavigationMoveEvent.Direction.Down: playButton.Focus(); break;
                case NavigationMoveEvent.Direction.Left: skill1Button.Focus(); break;
                case NavigationMoveEvent.Direction.Right: skill3Button.Focus(); break;
            }
            e.PreventDefault();
        });

        skill3Button.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch(e.direction)
            {
                case NavigationMoveEvent.Direction.Up: backButton.Focus(); break;
                case NavigationMoveEvent.Direction.Down: playButton.Focus(); break;
                case NavigationMoveEvent.Direction.Left: skill2Button.Focus(); break;
                case NavigationMoveEvent.Direction.Right: skill1Button.Focus(); break;
            }
            e.PreventDefault();
        });

        playButton.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch(e.direction)
            {
                case NavigationMoveEvent.Direction.Up: skill2Button.Focus(); break;
                case NavigationMoveEvent.Direction.Down: skill2Button.Focus(); break;
                case NavigationMoveEvent.Direction.Left: backButton.Focus(); break;
                case NavigationMoveEvent.Direction.Right: backButton.Focus(); break;
            }
            e.PreventDefault();
        });

        backButton.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch(e.direction)
            {
                case NavigationMoveEvent.Direction.Up: skill1Button.Focus(); break;
                case NavigationMoveEvent.Direction.Down: skill1Button.Focus(); break;
                case NavigationMoveEvent.Direction.Left: playButton.Focus(); break;
                case NavigationMoveEvent.Direction.Right: playButton.Focus(); break;
            }
            e.PreventDefault();
        });
    }
    
    public void SkillClick(EventBase evt, SkillEnum skillEnum)
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
        playButton.RegisterCallback<NavigationSubmitEvent>(PlayGame);
        playButton.style.backgroundColor = new StyleColor(new Color(88, 255, 88, 255));
        GameSettings.activeSkill = skillEnum;
        Debug.Log(GameSettings.activeSkill);
    }

    private void ReturnToModeMenu(EventBase evt)
    {
        GameSettings.activeSkill = SkillEnum.dash;
        playButton.style.backgroundColor = new StyleColor(new Color(0.5f, 0.5f, 0.5f));
        playButton.UnregisterCallback<ClickEvent>(PlayGame);
        playButton.UnregisterCallback<NavigationSubmitEvent>(PlayGame);
        if (document != null)
        {
            document.style.display = DisplayStyle.None;
        }
        showModeMenu(evt);
    }

    private void showModeMenu(EventBase evt)
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
                if (evt is NavigationSubmitEvent)
                {
                    Button buttonToFocus = rootElement.Query<Button>(className: "focus-button").First();
                    if (buttonToFocus != null)
                    {
                        buttonToFocus.Focus();
                    }
                }
            }
        }
    }
}
