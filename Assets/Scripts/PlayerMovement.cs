// Universidad del Valle de Guatemala
// Daniel Garcia, 14152
// Programacion de plataformas moviles y juegos

using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    // Character controller variables
    private Animator anim;
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    public VirtualJoystick inputJoystick;

    //motion variables
    private float horizontal = 0.0f;
    private float vertical = 0.0f;
    private float motion;


    //animator control variables
    private float nextAttack = 0.0f;
    private float delay = 1.5f;
    private bool blockVar = false;
    private float ID = 0.0f;
    private int attackCounter = 1;
    private bool isAlive = true;
    public bool isDead = false;
    private bool canMove = true;
    private bool check = true;

    //spell cooldowns and timer initial values
    private float spell1Cooldown = 10.0f;
    private float spell1NextAttack = 0.0f;
    private float spell2Cooldown = 6.0f;
    private float spell2NextAttack = 0.0f;
    private float spell3Cooldown = 6.0f;
    private float spell3NextAttack = 0.0f;

    //importing the lifeSystem script
    private LifeSystem playerScript;

    //Animator variables
    static int idleState = Animator.StringToHash("Base Layer.Idle");
    static int motionState = Animator.StringToHash("Base Layer.Motion");
    static int attackHash = Animator.StringToHash("attack");
    static int blockHash = Animator.StringToHash("block");
    static int spell1Hash = Animator.StringToHash("spell1");
    static int spell2Hash = Animator.StringToHash("spell2");
    static int spell3Hash = Animator.StringToHash("spell3");
    static int motionHash = Animator.StringToHash("speed");
    static int attackIDHash = Animator.StringToHash("attackID");
    static int deathHash = Animator.StringToHash("dead");
    private AnimatorStateInfo currentBaseState;

    //Variables that are dependent on the items equiped
    private float speed = 0.0F;

    //References to particle systems and their locations
    public GameObject healingLocation;
    public GameObject healingEffect;
    public GameObject destructionLocation;
    public GameObject destructionEffect;
    public GameObject groundBreakLocation;
    public GameObject groundBreakEffect;
    private GameObject healInstance;
    private GameObject destructionInstance;
    private GameObject groundBreakInstance;


    void Start()
    {
        //sets up the player controller and references the animator 
        controller = GetComponent<CharacterController>();
        anim = gameObject.GetComponentInChildren<Animator>();

        //creates the link betwwen the lifesystem script and the player controller
        playerScript = (LifeSystem)this.GetComponent(typeof(LifeSystem));

        //links the speed to move from the value being read from the lifesystem script
        speed = playerScript.currentSpeed;
    }

    void FixedUpdate()
    {
        //checks if the player is dead, if not, proceeds to allow movement and animation
        if (isDead == false)
        {
            //if the player is idle or running it allows for the player to move
            currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
            if(currentBaseState.fullPathHash == idleState || currentBaseState.fullPathHash==motionState)
            {
                canMove = true;
            }
            else
            {
                canMove = false;
            }
            if (canMove == true)
            {
                horizontal = inputJoystick.horizontal();
                vertical = inputJoystick.vertical();
                motion = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
                moveDirection = new Vector3(horizontal, 0, vertical);
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= speed;
                controller.Move(moveDirection * Time.deltaTime);
                anim.SetFloat(motionHash, motion);
            }
            //checks if the player is still alive, if not executes death animation and blocks any kind of movement
            isAlive = playerScript.currentStatus;
            if (isAlive==false && check==true)
            {
                anim.SetTrigger(deathHash);
                isDead = true;
                canMove = false;
                horizontal = 0.0f;
                vertical = 0.0f;
                EventManager.triggerEvent("EndGame", 2);
                EventManager.triggerEvent("StopSpawner", 1);
                check = false;
            }
        }
    }

    //sets the animation of blocking with the shield on
        public void blockButtonIn()
    {
        if (isDead == false)
        {
            //dont allow the player to move, keeps the animation running until stated otherwise
            if (blockVar == false)
            {
                canMove = false;
                blockVar = true;
                EventManager.triggerEvent("BlockModifier", 1);
                anim.SetBool(blockHash, true);
            }
        }
    }

    //releases the player movement and exits the block animation
    public void blockButtonOut()
    {
        if (isDead == false)
        {
            canMove = true;
            anim.SetBool(blockHash, false);
            EventManager.triggerEvent("BlockModifier", 0);
            blockVar = false;
        }
    }

    public void attackButton()
    {
        if (canMove == true)
        {
            if (Time.time > nextAttack)
            {
                switch (attackCounter)
                {
                    case 1:
                        delay = 0.952f;
                        ID = 1;
                        attackCounter = 2;
                        break;
                    case 2:
                        delay = 0.8f;
                        ID = 0;
                        attackCounter = 1;
                        break;
                }
                nextAttack = Time.time + delay;
                anim.SetTrigger(attackHash);
                anim.SetFloat(attackIDHash, ID);
            }
        }
    }

    ////Runs the spell 1 animation
    public void spell1OnClick()
    {
        if (isDead == false)
        {
            if (Time.time > spell1NextAttack)
            {
                EventManager.triggerEvent("Spell1", 25);
                spell1NextAttack = Time.time + spell1Cooldown;
            }
        }
    }

    //runs the spell 2 animation
    public void spell2OnClick()
    {
        if (isDead == false)
        {
            if (Time.time > spell2NextAttack)
            {
                EventManager.triggerEvent("Spell2", 5);
                spell2NextAttack = Time.time + spell2Cooldown;
            }
        }
    }

    //runs the spell 3 animation
    public void spell3OnClick()
    {
        if (isDead == false)
        {
            if (Time.time > spell3NextAttack)
            {
                EventManager.triggerEvent("Spell3", 10);
                spell3NextAttack = Time.time + spell3Cooldown;
            }
        }
    }


    void OnEnable()
    {
        EventManager.startListening("SpellAnimation", SpellAnimation);
    }

    void OnDisable()
    {
        EventManager.stopListening("SpellAnimation", SpellAnimation);
    }

    void SpellAnimation(int info)
    {
        switch (info)
        {
            case 1:
                anim.SetTrigger(spell1Hash);
                healInstance = (GameObject)Instantiate(healingEffect, healingLocation.transform);
                Destroy(healInstance, 7.5f);
                break;
            case 2:
                anim.SetTrigger(spell2Hash);
                groundBreakInstance = (GameObject)Instantiate(groundBreakEffect, groundBreakLocation.transform);
                Destroy(groundBreakInstance, 5.5f);
                break;
            case 3:
                anim.SetTrigger(spell3Hash);
                destructionInstance = (GameObject)Instantiate(destructionEffect, destructionLocation.transform);
                Destroy(destructionInstance, 5.5f);
                break;
        }
    }

    //sends whether or not the player is allowed to move, to the rotation script
    public bool currentMobility
    {
        get { return canMove; }
        set { canMove = value; }   
    }

    //sends wheter or not the rotation ability is active.
    public bool livingStatus
    {
        get { return isDead; }
        set { isDead = value; }
    }
}