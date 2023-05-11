//author: Archer
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GbitProjectState
{
	[System.Serializable]
	public class PlayerState
	{
		public bool onGround;
		public bool running;	//grounded
		public bool jumping;
		public bool jumpPressing;
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
			onGround = true;
			jumping = false;
			jumpPressing = false;
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

        //����һ��˽�б�������ʾ���˵�ǰ��״̬
        private State currentState;

        //����һ���������ԣ����ڻ�ȡ�����õ��˵�״̬
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

