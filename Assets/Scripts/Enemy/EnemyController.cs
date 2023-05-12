using GbitProjectControl;
using GbitProjectState;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyState enemyState;
    public int enemyEnergy;
    public float attackScale;
    public float currentInterval;
    public float maxInterval;//攻击间隔
    public GameObject player;
    [SerializeField] public GameObject bulletPrefab;

    void Start()
    {
        enemyState = new EnemyState();
        enemyEnergy = 1;
        attackScale = 7;
        maxInterval = 20;
        currentInterval = 0;
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if(enemyEnergy < 1 && enemyState.CurrentState != EnemyState.State.Dead )
		{
            enemyState.CurrentState = EnemyState.State.Dead;
		}
        else if(Vector2.Distance(transform.position, player.transform.position) < attackScale)
		{
            enemyState.CurrentState = EnemyState.State.Attack;
		}
        
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
            enemyEnergy -= 1;
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
         else if(currentInterval < maxInterval - 5)
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
        BulletTrigger trigger = bullet.GetComponent<BulletTrigger>();
        trigger.speed = 3;
        trigger.direction = (player.transform.position - transform.position);
    }

    void Dead()
	{
        //TODO:播放Boom动画
        Destroy(gameObject);
	}
}