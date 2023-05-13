using GbitProjectControl;
using GbitProjectState;
using UnityEngine;
using EnemyController;

public class StaticBoom : Enemy
{
    void Start()
    {
        enemyState = new EnemyState();
        enemyEnergy = 1;
        player = GameObject.Find("Player");
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
            default:
                Debug.Log("Error");
                break;
        }
    }
    
    public void OnTrigger(PlayerController controller)
	{
        if (controller != null && enemyState.CurrentState == EnemyState.State.Idle)//TODO:attacked
        {
            Damage(1);
        }
        else if (controller != null && enemyState.CurrentState == EnemyState.State.Alert)
        {
            //TODO:destroy itself and damage player or Boss
        }
    }

    void Idle()
    {

    }

    void Alert()
    {
        //TODO:anim
        //TODO:
        Debug.Log("WARING!");
    }
}