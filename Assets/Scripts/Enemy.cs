// Universidad del Valle de Guatemala
// Daniel Garcia, 14152
// Programacion de plataformas moviles y juegos

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    //variables for the enemy animation system
    private Animator anim;
    private CharacterController controller;

    //states that indicate the current task of the enemy
    public enum State { Idle, Chasing, Attacking, Death };
    State currentState;

    //pathfinder variables to use in the navMesh system
    UnityEngine.AI.NavMeshAgent pathfinder;
    Transform target;

    //Variables for the enemy to move around and distances to attack
    float attackDistanceThreshold = .5f;
    float timeBetweenAttacks = 1;
    float nextAttackTime;
    float myCollisionRadius=0.75f;
    float targetCollisionRadius=0.75f;

    //control variables
    private bool playerIsDead=false;
    private bool hasTarget=true;
    private bool enemyIsDead = false;
    private bool messageSent = false;

    //animator variables
    static int attackHash = Animator.StringToHash("attack");
    static int motionHash = Animator.StringToHash("motion");
    static int deathHash = Animator.StringToHash("dead");

    //counter to receive initial values
    private int messageCounter = 0;

    //variables that are inherited
    public int life = 0;
    public int damage = 0;

    void Start()
    {

        //creates reference to the nav mesh and the player to follow
        pathfinder = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        //controller to play the character's animation
        controller = GetComponent<CharacterController>();
        anim = gameObject.GetComponentInChildren<Animator>();

        //current state that the enemy starts
        currentState = State.Chasing;


        //initiate the search if there is a player
        if (playerIsDead == false)
        {
            StartCoroutine(UpdatePath());
        }
    }

    void Update()
    {
        //if the enemy is not dead, keep chasing the player
        if (enemyIsDead == false)
        {
            //looks the current state of the main character and checks its vitality
            playerIsDead = GameObject.Find("MainCharacter").GetComponent<PlayerMovement>().isDead;
            //if the player is not dead, keep on chasing
            if (playerIsDead == false)
            {
                if (Time.time > nextAttackTime)
                {
                    float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
                    if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
                    {
                        nextAttackTime = Time.time + timeBetweenAttacks;
                        //jumps to the corroutine in charge of playing the attack animation
                        StartCoroutine(Attack());
                    }

                }
            }
            //if the main character is dead, stop chasing and plays idle.
            else
            {
                anim.SetBool(motionHash, false);
                currentState = State.Idle;
                hasTarget = false;
                
            }
        }
        //the enemy has died, therefore it plays death animation and can no longer move
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

    
    //makes the enemy attack the player once it is in close proximity
    IEnumerator Attack()
    {
        anim.SetTrigger(attackHash);
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

    ////checks if the player has attacked the enemy, reduces life.
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

    //looks for the player and moves towards it.
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