using System.Collections;
using System.Collections.Generic;
using GbitProjectControl;
using UnityEngine;

public class EnergyCollectible : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null)
        {
            //TODO:ª÷∏¥ÃÂ¡¶
            Destroy(gameObject);
        }
    }
}
