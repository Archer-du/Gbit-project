using GbitProjectControl;
using GbitProjectState;
using UnityEngine;
using EnemyController;

public class FlyingBullet : Enemy
{
    [SerializeField] public GameObject bulletPrefab;
    void Start()
    {
        enemyState = new EnemyState();
        enemyEnergy = 1;
        attackScale = 10;
        maxInterval = 20;
        currentInterval = 0;
        player = GameObject.Find("Player");
    }

	void Update()
	{
        StateJudge();
        StateUpdate();
	}

	void StateJudge()
	{
        if (enemyEnergy < 1 && enemyState.CurrentState != EnemyState.State.Dead)
        {
            enemyState.CurrentState = EnemyState.State.Dead;
        }
        else if (Vector2.Distance(transform.position, player.transform.position) < attackScale)
        {
            enemyState.CurrentState = EnemyState.State.Attack;
        }
        else if(Vector2.Distance(transform.position, player.transform.position) > attackScale)
		{
            enemyState.CurrentState = EnemyState.State.Idle;
		}
    }

    public void StateUpdate()
    {
        switch (enemyState.CurrentState)
        {
            case EnemyState.State.Idle:
                Idle();
                break;
            case EnemyState.State.Attack:
                Attack();
                break;
            case EnemyState.State.Dead:
                Dead();
                break;
            default:
                Debug.Log("Error");
                break;  
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
	{
        PlayerController controller = other.GetComponent<PlayerController>();
        if(controller != null)
		{
            Damage(1);
		}
    }
    
    void Idle()
	{
        //TODO:播放待机动画
	}

	void Attack()
	{
         if(currentInterval < maxInterval)
		{
            currentInterval += Time.deltaTime * 10;
		}
         else if(currentInterval < (maxInterval * 0.5f))
		{
            //TODO:播放射击提示动画
		}
        else
		{
            Shoot();
            currentInterval = 0;
		}
	}

    void Shoot()
	{
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Bullet Bullet = bullet.GetComponent<Bullet>();
        Bullet.speed = 3;
        Bullet.direction = (player.transform.position - transform.position);
    }

    void Dead()
	{
        //TODO:播放Boom动画
        Destroy(gameObject);
	}

}