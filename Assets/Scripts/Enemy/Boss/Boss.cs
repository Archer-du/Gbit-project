using GbitProjectControl;
using UnityEngine;
using EnemyController;

public class Boss : Enemy
{
    private BossState bossState;

    void Awake()
    {
        bossState = new BossState();
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        PositionUpdate();
        StateJudge();
        StateUpdate();
    }

    void PositionUpdate()
    {
         
    }

    void StateJudge()
    {

    }

    public void StateUpdate()
    {
        switch (bossState.CurrentState)
        {
            case BossState.State.Chase:
                Chase();
                break;
            default:
                Debug.Log("Error");
                break;
        }
    }

    void Chase()
	{

	}
}
