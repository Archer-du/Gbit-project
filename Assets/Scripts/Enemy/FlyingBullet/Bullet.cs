using GbitProjectControl;
using GbitProjectState;
using UnityEngine;
using EnemyController;
public class Bullet : FlyingBullet
{
    public Vector2 direction;
    public float speed;

    private void Update()
    {
        GetComponent<Rigidbody2D>().velocity = speed * direction;
        if (transform.position.y > 30 || transform.position.y < -30)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null)
        {
            Destroy(gameObject);//TODO:damage
        }
        else
		{
            Destroy(gameObject);
		}
    }
}
