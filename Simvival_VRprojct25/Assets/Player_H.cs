using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_H : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Transform campSpawnPoint; // drag your campsite Transform

    void Start()

    {
        currentHealth = maxHealth;

    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)

            Die();

    }


    void Die()
    {

        transform.position = campSpawnPoint.position;
        currentHealth = maxHealth;

    }
}
