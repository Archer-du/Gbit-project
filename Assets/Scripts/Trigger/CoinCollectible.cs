using System.Collections;
using System.Collections.Generic;
using GbitProjectControl;
using UnityEngine;

public class CoinCollectible : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null)
        {
            //TODO:ÊÕ¼¯½ð±Ò
            Destroy(gameObject);
        }
    }
}
