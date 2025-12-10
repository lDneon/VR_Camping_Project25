using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrSpary : MonoBehaviour
{

    public int sprayDamage = 10;

    private void OnTriggerStay(Collider other)

    {

        if (other.CompareTag("Bear"))

        {

            other.GetComponent<BearHealth>().TakeDamage(sprayDamage);

        }

    }

}



















