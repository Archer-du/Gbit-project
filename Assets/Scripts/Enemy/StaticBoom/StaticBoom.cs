using UnityEngine;
using EnemyController;

public class StaticBoom : Enemy
{
    void Start()
    {
        enemyState = new EnemyState();
        enemyEnergy = 1;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        StateJudge();
        StateUpdate();
    }

    void StateJudge()
    {
        if (enemyEnergy < 1 && enemyState.CurrentState == EnemyState.State.Idle)
        {
            enemyState.CurrentState = EnemyState.State.Alert;
        }
    }

    public void StateUpdate()
    {
        switch (enemyState.CurrentState)
        {
            case EnemyState.State.Idle:
                Idle();
                break;
            case EnemyState.State.Alert:
                Alert();
                break;
            case EnemyState.State.Dead:
                Dead();
                break;
            default:
                Debug.Log("Error");
                break;
        }
    }
    
    public void OnTrigger()
	{
        if(enemyEnergy < 1 && enemyState.CurrentState == EnemyState.State.Alert)
		{
            //TODO:damage
            enemyState.CurrentState = EnemyState.State.Dead;
		}
    }

    void Idle()
    {

    }

    void Alert()
    {
        animator.SetBool("Alert", true);
    }

    void Dead()
	{
        //TODO:anim
        Destroy(gameObject);
	}
}