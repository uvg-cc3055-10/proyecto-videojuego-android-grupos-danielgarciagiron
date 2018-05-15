// Universidad del Valle de Guatemala
// Daniel Garcia, 14152
// Programacion de plataformas moviles y juegos

using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    private Animator anim;
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    public VirtualJoystick inputJoystick;


    private float horizontal = 0.0f;
    private float vertical = 0.0f;
    private float motion;


    private float nextAttack = 0.0f;
    private float delay = 1.5f;
    private bool blockVar = false;
    private float ID = 0.0f;
    private int attackCounter = 1;
    private bool isAlive = true;
    public bool isDead = false;
    private bool canMove = true;
    private bool check = true;

    private float spell1Cooldown = 10.0f;
    private float spell1NextAttack = 0.0f;
    private float spell2Cooldown = 6.0f;
    private float spell2NextAttack = 0.0f;
    private float spell3Cooldown = 6.0f;
    private float spell3NextAttack = 0.0f;

    private LifeSystem playerScript;

    //el usar hashes es una forma de optimizar cuando se tienen bastantes animaciones.
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

    private float speed = 0.0F;

    //sirve para todos los sistemas de particulas utilizados.
    public GameObject healingLocation;
    public GameObject healingEffect;
    public GameObject destructionLocation;
    public GameObject destructionEffect;
    public GameObject groundBreakLocation;
    public GameObject groundBreakEffect;
    private GameObject healInstance;
    private GameObject destructionInstance;
    private GameObject groundBreakInstance;

    //Audio 
    public AudioClip attackAudio;
    AudioSource playerAudioSource;

    void Start()
    {
        //controller y animator 
        controller = GetComponent<CharacterController>();
        anim = gameObject.GetComponentInChildren<Animator>();

        playerScript = (LifeSystem)this.GetComponent(typeof(LifeSystem));

        playerAudioSource = GetComponent<AudioSource>();

        speed = playerScript.currentSpeed;
    }

    void FixedUpdate()
    {
        if (isDead == false)
        {
            //deja que se mueva el jugador si esta en idle
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
            //cuando se muere el personaje, bloquea todos los inputs. 
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

        public void blockButtonIn()
    {
        if (isDead == false)
        {
            //no deja que se mueva el personaje cuando bloquea.
            if (blockVar == false)
            {
                canMove = false;
                blockVar = true;
                EventManager.triggerEvent("BlockModifier", 1);
                anim.SetBool(blockHash, true);
            }
        }
    }

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

    //maneja las dos animaciones para atacar, utiliza timers y contadores.
    public void attackButton()
    {
        if (canMove == true)
        {
            if (Time.time > nextAttack)
            {
                playerAudioSource.PlayOneShot(attackAudio, 0.5f);
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

    //Sirve para la primera habilidad, utiliza un timer para evitar que se utilice repetidas veces en tiempos cortos.
    public void spell1OnClick()
    {
        if (isDead == false)
        {
            if (Time.time > spell1NextAttack)
            {
                EventManager.triggerEvent("Spell1", 20);
                spell1NextAttack = Time.time + spell1Cooldown;
            }
        }
    }

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

    //Spawnea los efectos de las habilidades.
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

    public bool currentMobility
    {
        get { return canMove; }
        set { canMove = value; }   
    }

    public bool livingStatus
    {
        get { return isDead; }
        set { isDead = value; }
    }
}