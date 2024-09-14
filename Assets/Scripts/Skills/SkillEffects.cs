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
    private Vector3 dashVector;
    private SkillState state;
    public bool vanishActive;
    public bool moveMode;
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
        if (context.performed && !cooldownActive)
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
                    state = SkillState.vanished;
                    //disable the players collision for the duration
                    //add a darkness to the player or screen
                    //stop all enemy movement towards the player
                    //revert those once the duration has finished
                    Debug.Log("Player has Vanished!");
                    break;
                }
                case SkillEnum.decoy:
                {
                    //spawn another duck prefab
                    //make all enemies target that one for the duration
                    //after duration all eneimes go back to targeting the player
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
            Physics2D.IgnoreLayerCollision(7, 9, true);
            //add a darkness to the player or screen
            GetComponentInChildren<SpriteRenderer>().color = new Color(255,255,255,0.5f);
            //stop all enemy movement towards the player
            vanishActive = true;
        }
        else
        {
            Physics2D.IgnoreLayerCollision(7, 9, false);
            GetComponentInChildren<SpriteRenderer>().color = new Color(255,255,255,1f);
            vanishActive = false;
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
