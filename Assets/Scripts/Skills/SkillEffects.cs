using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum SkillState
{
    none,
    dashing,
    vanished,
}
public class SkillEffects : MonoBehaviour
{
    [Header("References and other variables")]
    private List<Skill> skillList = new List<Skill>();
    public static SkillEffects Instance;
    private SkillState state;
    private Rigidbody2D rb;
    [Header("UI")]
    [SerializeField]
    private GameObject document;
    private VisualElement hud;
    private VisualElement cooldownBG;
    private Label cooldownTimer;
    private VisualElement activeSkillIcon;
    private IMGUIContainer durationBar;
    private IMGUIContainer durationContainer;
    [Header("Cooldowns")]
    public bool cooldownActive;
    private float cooldownRemaining;
    public float cooldownModifier = 1.0f;

    public bool durationActive;
    private float durationRemaining;
    public float durationModifier = 1.0f;
    [Header("Dash")]
    [SerializeField]
    private float dashForce;
    public bool moveMode;
    private Vector3 dashVector;

    [Header("Vanish")]
    public bool vanishActive;
    [Header("Decoy")]

    [SerializeField]
    private GameObject decoy;
    private GameObject spawnedDecoy;
    public bool decoyActive;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Load();
        rb = GetComponent<Rigidbody2D>();
        hud = document.GetComponent<UIDocument>().rootVisualElement;

        cooldownBG = hud.Q<VisualElement>("SkillCooldown");
        cooldownBG.visible = false; //error checking
        cooldownTimer = hud.Q<Label>("SkillTimer");

        activeSkillIcon = hud.Q<VisualElement>("ActiveSkill");
        if (GameSettings.activeSkill == SkillEnum.dash)
        {
            activeSkillIcon.style.backgroundImage = Resources.Load<Texture2D>("DashV2");
        }
        else if (GameSettings.activeSkill == SkillEnum.vanish)
        {
            activeSkillIcon.style.backgroundImage = Resources.Load<Texture2D>("VanishV2");
        }
        else if (GameSettings.activeSkill == SkillEnum.decoy)
        {
            activeSkillIcon.style.backgroundImage = Resources.Load<Texture2D>("Decoy");
        }

        durationBar = hud.Q<IMGUIContainer>("Duration");
        durationContainer = hud.Q<IMGUIContainer>("DurationBackground");
        durationContainer.style.opacity = 0;
    }
    public void RunSkill(InputAction.CallbackContext context)
    {
        if (context.performed && !cooldownActive && !durationActive && GameSettings.gameState == GameState.InGame)
        {   
            //depending on active skill, setup the base values and set the state to activate the skill on button press.
            switch (GameSettings.activeSkill)
            {
                case SkillEnum.dash:
                {
                    state = SkillState.dashing;
                    if (!moveMode)
                    {
                        if (TopDownMovement.Instance.moveInput == Vector2.zero)
                        {
                            return;
                        }
                        else
                        {
                            dashVector = new Vector3(TopDownMovement.Instance.moveInput.x, TopDownMovement.Instance.moveInput.y, 0);
                        }
                        dashForce = 20f;
                    }
                    else 
                    {
                        //get the position of the cursor on the screen and make a vector of its direction from the player
                        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        dashVector = (mouseWorldPosition - transform.position).normalized;
                        dashForce = 30f;//these two dash forces are arbitrary and can be changed to make both dash styles feel the same.
                    }

                    break;
                }
                case SkillEnum.vanish:
                {
                    state = SkillState.vanished;
                    break;
                }
                case SkillEnum.decoy:
                {
                    //spawn another duck 
                    spawnedDecoy = Instantiate(decoy, transform.position, Quaternion.identity);  
                    decoyActive = true;
                    break;
                }
            }
            StartDuration();
        }
    }
    private void Load()
    {
        string json = Resources.Load<TextAsset>("skills").text;
        SkillList skillsJson = JsonUtility.FromJson<SkillList>(json);
        skillList = skillsJson.skills;
    }

    void Update()
    {
        if (GameSettings.gameState != GameState.InGame) { return; }
        if (durationActive)
        {
            //lerp the opacity of the duration box in update
            durationContainer.style.opacity = Mathf.Lerp(0, 1, 0.3f); //unsure if this lerp function is doing anything :/
            CheckDuration();
        }
        if (cooldownActive)
        {
            CheckCooldown();
        }
    }

    void LateUpdate()
    {
        if (GameSettings.gameState != GameState.InGame) { return; }
        if (state == SkillState.dashing && durationActive) 
        {
            //add some velocity to the player and push them some distance towards that direction
            rb.AddForce(dashVector * dashForce, ForceMode2D.Impulse);
            PlayerStats.Instance.StartCoroutine(PlayerStats.Instance.DisableCollisionForDuration(durationRemaining));
        }
        if (state == SkillState.vanished && durationActive)
        {   
            //disable the players collision for the duration
            PlayerStats.Instance.StartCoroutine(PlayerStats.Instance.DisableCollisionForDuration(durationRemaining));
            //add a darkness to the player or screen
            GetComponentInChildren<SpriteRenderer>().color = new Color(255,255,255,0.5f);
            //stop all enemy movement towards the player
            vanishActive = true;
        }
        else
        {
            GetComponentInChildren<SpriteRenderer>().color = new Color(255,255,255,1f);
            vanishActive = false;
        }
        if (decoyActive && !durationActive)
        {
            decoyActive = false;
            Destroy(spawnedDecoy);
        }

    }

    private void StartCooldown()
    {
        cooldownRemaining = skillList[(int)GameSettings.activeSkill].cooldown;//this restricts the ability to make one conjoined method of StartCooldown, StartDuration, etc. etc.
        cooldownRemaining *= cooldownModifier;
        cooldownActive = true;
        Debug.Log(cooldownRemaining);
    }

    private void StartDuration()
    {
        durationRemaining = skillList[(int)GameSettings.activeSkill].duration;
        durationRemaining *= durationModifier;
        durationActive = true;
        cooldownBG.visible = true;
        Debug.Log(durationRemaining);
    }

    private void CheckDuration()
    {
        if (durationRemaining > 0)
        {
            durationRemaining -= Time.deltaTime;
            float durationFraction = durationRemaining / (skillList[(int)GameSettings.activeSkill].duration * durationModifier);
            durationBar.style.width = Length.Percent(durationFraction * 100);
        }
        else
        {
            durationActive = false;
            state = SkillState.none;
            durationContainer.style.opacity = 0;
            StartCooldown();
        }
    }

    private void CheckCooldown()
    {
        if (cooldownRemaining > 0)
        {
            cooldownRemaining -= Time.deltaTime;
            cooldownTimer.text = Mathf.Round(cooldownRemaining).ToString();
        }
        else
        {
            cooldownActive = false;
            cooldownBG.visible = false;
            cooldownTimer.text = "";
        }
    }

}
