using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    public int currentHealth;
    [SerializeField] private int maxHealth = 15;

    EnemyUICanvasController healthSlider;

    public void Start()
    {
        healthSlider = GetComponentInChildren<EnemyUICanvasController>();
        healthSlider.SetMaxHealth(maxHealth);

        currentHealth = maxHealth;

    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        healthSlider.SetHealth(currentHealth);

        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
