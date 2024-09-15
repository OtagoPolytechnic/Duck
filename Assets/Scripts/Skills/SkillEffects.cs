using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum SkillState
{
    none,
    dashing,
    vanished,
}
public class SkillEffects : MonoBehaviour
{
    [Header("Cooldowns")]
    public bool cooldownActive;
    private float cooldownRemaining;
    public bool durationActive;
    private float durationRemaining;
    [Header("Dash")]
    [SerializeField]
    private float dashForce;
    private List<Skill> skillList = new List<Skill>();
    private Rigidbody2D rb;
    public bool moveMode;
    private Vector3 dashVector;
    private SkillState state;
    [Header("Vanish")]
    public bool vanishActive;
    [Header("Decoy")]

    [SerializeField]
    private GameObject decoy;
    private GameObject spawnedDecoy;
    public bool decoyActive;
    public static SkillEffects Instance;

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
    }
    public void RunSkill(InputAction.CallbackContext context)
    {
        if (context.performed && !cooldownActive && GameSettings.gameState == GameState.InGame)
        {   
            //depending on active skill, setup the base values and set the state to activate the skill on button press.
            switch (GameSettings.activeSkill)
            {
                case SkillEnum.dash:
                {
                    Debug.Log("Player has Dashed!");
                    state = SkillState.dashing;
                    if (!moveMode)
                    {
                        dashVector = new Vector3(TopDownMovement.Instance.moveInput.x, TopDownMovement.Instance.moveInput.y, 0);
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
                    Debug.Log("Player has Vanished!");
                    state = SkillState.vanished;
                    break;
                }
                case SkillEnum.decoy:
                {
                    //spawn another duck 
                    spawnedDecoy = Instantiate(decoy, transform.position, Quaternion.identity);  
                    decoyActive = true;
                    Debug.Log("Player has Decoyed!");
                    break;
                }
            }
            StartDuration();
            StartCooldown();
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
            CheckDuration();
        }
        if (cooldownActive)
        {
            CheckCooldown();
        }
    }

    void LateUpdate()
    {
        if (state == SkillState.dashing && durationActive) 
        {
            //add some velocity to the player and push them some distance towards that direction
            rb.AddForce(dashVector * dashForce, ForceMode2D.Impulse);
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
        cooldownActive = true;
    }

    private void StartDuration()
    {
        durationRemaining = skillList[(int)GameSettings.activeSkill].duration;
        durationActive = true;
    }

    private void CheckDuration()
    {
        if (durationRemaining > 0)
        {
            durationRemaining -= Time.deltaTime;
        }
        else
        {
            Debug.Log("Duration ended");
            durationActive = false;
            state = SkillState.none;
        }
    }

    private void CheckCooldown()
    {
        if (cooldownRemaining > 0)
        {
            cooldownRemaining -= Time.deltaTime;
        }
        else
        {
            Debug.Log("Cooldown ended");
            cooldownActive = false;
        }
    }

}
