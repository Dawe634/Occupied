using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{

    NavMeshAgent myAgent;
    public LayerMask whatIsGround, whatIsPlayer;
    public Transform player;
    Animator myAnimator;
    public Transform firePosition;


    public Vector3 destinationPoint;
    bool destinationSet;
    public float destinationRange;

    public float ChaseRange;
    private bool playerInChaseRange;

    public float AttackRange, attackTime;
    private bool playerInAttackRange, readyToAttack = true;
    public GameObject AttackProjectile;

    public bool meleeAttacker;
    public bool airDrone;
    public int meleeDamageAmount;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        player = FindObjectOfType<Player>().transform;
        myAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInChaseRange = Physics.CheckSphere(transform.position, ChaseRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, whatIsPlayer);

        if (!playerInChaseRange && !playerInAttackRange)
        {
            Guarding();
        }
        if (playerInChaseRange && !playerInAttackRange)
        {
            ChasingPlayer();
        }
        if(playerInAttackRange && playerInChaseRange)
        {
            AttackingPlayer();
        }
    }

   

    private void Guarding()
    {
        if (!destinationSet)
        {
            SearchForDestination();
        }
        else
        {
            myAgent.SetDestination(destinationPoint);
        }

        Vector3 distanceToDestination = transform.position - destinationPoint;

        if (distanceToDestination.magnitude < 1f)
        {
            destinationSet = false;
        }

    }

    private void ChasingPlayer()
    {
        myAgent.SetDestination(player.position);
    }

 private void AttackingPlayer()
    {
        myAgent.SetDestination(transform.position);
        transform.LookAt(player);
        if (readyToAttack && !meleeAttacker)
        {
            firePosition.LookAt(player);

            myAnimator.SetTrigger("Attack");
            
            Instantiate(AttackProjectile, firePosition.position, firePosition.rotation);
            readyToAttack = false;
            
            StartCoroutine(ResetAttack());
        }
        else if(readyToAttack && meleeAttacker)
        {
            myAnimator.SetTrigger("Attack");
        }
    }

    public void MeleeDamage()
    {
        player.GetComponent<PlayerHealthSystem>().TakeDamage(meleeDamageAmount);
    }

    private void SearchForDestination()
    {
        //Create random point for enemy to walk too
        float randPositionZ = Random.Range(-destinationRange, destinationRange);
        float randPositionX = Random.Range(-destinationRange, destinationRange);

        //Move to the created point
        destinationPoint = new Vector3(
            transform.position.x + randPositionX,
            transform.position.y,
            transform.position.z + randPositionZ);

        if (Physics.Raycast(destinationPoint, -transform.up, 2f, whatIsGround))
        {
            destinationSet = true;
        }
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackTime);

        readyToAttack = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);

    }
}
