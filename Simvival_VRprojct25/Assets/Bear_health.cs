using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearHealth : MonoBehaviour

{
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject meatDrop;    // assign raw meat prefab
    public Animator bearAnimator;  // assign animator

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

        bearAnimator.SetTrigger("Death");
        Invoke(nameof(DisableBear), 2f); // waits for animation

    }

    void DisableBear()
    {
        gameObject.SetActive(false);
        meatDrop.SetActive(true); // drop the meat prefab
    }

}


