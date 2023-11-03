using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{
    public float Moeda;
    
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
              GameController.instance.UpdateQuantidade(Moeda);
              Destroy(gameObject);
        }
    }
}
