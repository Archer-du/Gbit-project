//author: Archer
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GbitProjectState
{
	[System.Serializable]
	public class PlayerState
	{
		public bool running;	//grounded
		public bool jumping;
		public bool falling;
		public bool attacking;
		public bool dashing;
		public bool sliding;
		public bool touching;	//entered the designated range of the enemy
		public bool hitted;
		public bool coyote;		//coyote time range check
		public bool death;
		public PlayerState()
		{
			jumping = false;
			running = true;
			attacking = false;
			dashing = false;
			sliding = false;
			touching = false;
			hitted = false;
			coyote = false;
			death = false;
		}
	}

    public class EnemyState
    {

        public enum State
        {
            Idle,
            Attack,
            Dead
        }

        //定义一个私有变量，表示敌人当前的状态
        private State currentState;

        //定义一个公共属性，用于获取或设置敌人的状态
        public State CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        public EnemyState()
        {
            currentState = State.Idle;
        }
    }
}

