using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooking_Meat : MonoBehaviour
{
    public GameObject raw_meat;
    public GameObject cooked_meat;


    private void OnTriggerEnter(Collider other)

    {

        if (other.CompareTag("campfire"))

        {

            CookMeat();

        }

    }

    void CookMeat()

    {

        raw_meat.SetActive(false);

        cooked_meat.SetActive(true);

    }
}
