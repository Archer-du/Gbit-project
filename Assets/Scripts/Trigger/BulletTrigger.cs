using System.Collections;
using System.Collections.Generic;
using GbitProjectControl;
using UnityEngine;

public class BulletTrigger : MonoBehaviour
{
    public Vector2 direction;
    public float speed;

    private void Update()
	{
        GetComponent<Rigidbody2D>().velocity = speed * direction;
    }

	void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null)
        {
            Destroy(gameObject);
        }
    }
}
