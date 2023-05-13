using GbitProjectControl;
using GbitProjectState;
using UnityEngine;
using EnemyController;

public class Bullet : FlyingBullet
{
    //public GameObject FlyingBullet;
    public Vector2 direction;
    public float speed;
    public float speedFactor;

    private void Update()
    {
        speed += speed * speedFactor * Time.deltaTime;
        transform.Translate(direction * speed * Time.deltaTime);
        if (transform.position.y > 30 || transform.position.y < -30)
        {
            BulletPool.Instance.ReturnToPool(this);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //PlayerController controller = other.GetComponent<PlayerController>();
        if (other.CompareTag("Terrain"))
        {
            BulletPool.Instance.ReturnToPool(this);//TODO:damage
        }
        else if(other.CompareTag("Player"))
		{
            //TODO:damage
            BulletPool.Instance.ReturnToPool(this);
        }
    }
}
