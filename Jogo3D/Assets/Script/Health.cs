
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float CoracaoValue;

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<Player>().IncreaseHealth(CoracaoValue);
        
    }
}
