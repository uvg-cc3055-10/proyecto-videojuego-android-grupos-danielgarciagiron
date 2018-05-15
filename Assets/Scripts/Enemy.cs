// Universidad del Valle de Guatemala
// Daniel Garcia, 14152
// Programacion de plataformas moviles y juegos

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class Enemy : MonoBehaviour
{

    private Animator anim;
    private CharacterController controller;

    //maquina de estados finitos para controlar los estados del enemigo. No es necesario puesto que se tienen muy pocos estados.
    public enum State { Idle, Chasing, Attacking, Death };
    State currentState;


    UnityEngine.AI.NavMeshAgent pathfinder;
    Transform target;


    float attackDistanceThreshold = .5f;
    float timeBetweenAttacks = 1;
    float nextAttackTime;
    float myCollisionRadius=0.75f;
    float targetCollisionRadius=0.75f;


    private bool playerIsDead=false;
    private bool hasTarget=true;
    private bool enemyIsDead = false;
    private bool messageSent = false;


    static int attackHash = Animator.StringToHash("attack");
    static int motionHash = Animator.StringToHash("motion");
    static int deathHash = Animator.StringToHash("dead");

    private int messageCounter = 0;

    public int life = 0;
    public int damage = 0;

    //audio
    public AudioClip attackAudio;
    AudioSource enemyAudioSource;

    void Start()
    {

        //crea la referencia al navmesh para que se puedan mover los enemigos.
        pathfinder = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        controller = GetComponent<CharacterController>();
        anim = gameObject.GetComponentInChildren<Animator>();

        enemyAudioSource = GetComponent<AudioSource>();

        currentState = State.Chasing;

        if (playerIsDead == false)
        {
            StartCoroutine(UpdatePath());
        }
    }

    void Update()
    {
        if (enemyIsDead == false)
        {
            //Verifica si el personaje esta muerto.
            playerIsDead = GameObject.Find("MainCharacter").GetComponent<PlayerMovement>().isDead;
            //si esta vivo, correr detras de el.
            if (playerIsDead == false)
            {
                if (Time.time > nextAttackTime)
                {
                    float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
                    if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
                    {
                        nextAttackTime = Time.time + timeBetweenAttacks;
                        //si esta en rango de ataque y ya paso el delay, puede atacar.
                        StartCoroutine(Attack());
                    }

                }
            }
            //si el personaje esta muerto, detenerse y quedarse en idle.
            else
            {
                anim.SetBool(motionHash, false);
                currentState = State.Idle;
                hasTarget = false;
            }
        }
        //Si el enemigo muere, deja de moverse, corre la animacion de morir.
        else
        {
            currentState = State.Death;
            if (messageSent == false)
            {
                EventManager.triggerEvent("EnemyDeath", 1);
                messageSent = true;
            }
            
        }
    }

    
    //corutina para atacar al personaje si esta en el rango para hacerlo.
    IEnumerator Attack()
    {
        anim.SetTrigger(attackHash);
        enemyAudioSource.PlayOneShot(attackAudio, 0.5f);
        currentState = State.Attacking;
        pathfinder.enabled = false;
        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);
        float attackSpeed = 3;
        float percent = 0;
        while (percent <= 1)
        {
            percent += Time.deltaTime * attackSpeed;

            yield return null;
        }
        currentState = State.Chasing;
        pathfinder.enabled = true;
    }

    //Si el personaje lo golpea, restar vida. Verifica que el collider sea el arma del personaje usando el tag.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerWeapon")
        {
            if (enemyIsDead == false)
            {
                life -= damage;
                if (life <= 0)
                {
                    life = 0;
                    anim.SetTrigger(deathHash);
                    enemyIsDead = true;
                    currentState = State.Death;
                }
            }
        }
    }

    //Busca al personaje y se mueve hacia el.
    IEnumerator UpdatePath()
    {
        float refreshRate = .25f;

        while (hasTarget)
        {
            if (currentState == State.Chasing)
            {
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);
            anim.SetBool(motionHash, true);
            pathfinder.SetDestination(targetPosition);
                
            }
            yield return new WaitForSeconds(refreshRate);
        }

    }

}