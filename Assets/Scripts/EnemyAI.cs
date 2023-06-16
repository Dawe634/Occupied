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


    public Vector3 destinationPoint;
    bool destinationSet;
    public float destinationRange;

    public float ChaseRange;
    public bool playerInChaseRange;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().transform;
        myAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInChaseRange = Physics.CheckSphere(transform.position, ChaseRange, whatIsPlayer);

        if (!playerInChaseRange)
        {
            Guarding();
        }
        else if (playerInChaseRange)
        {
            ChasingPlayer();
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);

    }
}
