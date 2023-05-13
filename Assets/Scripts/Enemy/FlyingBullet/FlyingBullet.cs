using GbitProjectControl;
using GbitProjectState;
using UnityEngine;
using EnemyController;

public class FlyingBullet : Enemy
{
    //[SerializeField] public GameObject bulletPrefab;
    void Start()
    {
        enemyState = new EnemyState();
        enemyEnergy = 1;
        attackScale = 10;
        maxInterval = 20;
        currentInterval = 0;
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
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
        animator.SetBool("Attack", false);//TODO:播放待机动画
	}

	void Attack()
	{
        animator.SetBool("Attack", true); 
        if(currentInterval < maxInterval)
		{
            currentInterval += Time.deltaTime * 10;
		}
        else
		{
            Shoot();
            currentInterval = 0;
		}
	}

    void Shoot()
    {
        Bullet bullet = BulletPool.Instance.GetFromPool();
        if (bullet != null)
        {
            bullet.gameObject.SetActive(true);
            bullet.transform.SetParent(transform);
            bullet.transform.position = transform.position + Vector3.up * 0.5f;
            bullet.direction = (player.transform.position - transform.position);
            bullet.speed = 2;
            bullet.speedFactor = 0.5f;
        }
    }


    void Dead()
	{
        //TODO:播放Boom动画
        Destroy(gameObject);
	}

}