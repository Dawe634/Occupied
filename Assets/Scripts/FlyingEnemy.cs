using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float speed = 5f;  // Adjust the speed of the enemy
    public int health = 100;  // Adjust the initial health of the enemy

    private Transform player;  // Reference to the player's transform

    private void Start()
    {
        // Find the player's transform using a tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private float smoothness = 0.5f; // Adjust the smoothness of the movement

private void Update()
{
    // Calculate the direction towards the player
    Vector3 direction = player.position - transform.position;

    // Normalize the direction to get a consistent speed
    direction = direction.normalized;

    // Smoothly move towards the player using Lerp
    transform.position = Vector3.Lerp(transform.position, transform.position + direction, smoothness * Time.deltaTime);

    // Check if the enemy has reached the player
    if (transform.position == player.position)
    {
        // TODO: Handle player reached logic (e.g., deal damage to the player)
    }
}

    

    public void TakeDamage(int damage)
    {
        // Reduce the enemy's health by the specified damage amount
        health -= damage;

        if (health <= 0)
        {
            // TODO: Handle enemy death logic (e.g., play death animation, award points, etc.)
            Destroy(gameObject);
        }
    }
}

