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
		public bool isblocked;
		public bool running;	//grounded
		public bool jumping;
		public bool jumpPressing;
		public bool falling;
		public bool attacking;
		public bool dashing;
		public bool sliding;
		public bool ceiled;
		public bool touching;	//entered the designated range of the enemy
		public bool hitted;
		public bool coyote;		//coyote time range check
		public bool death;
		public AttackType attackType;
		public enum AttackType
		{
			light1,
			light2,
			heavy
		}
		public PlayerState()
		{
			onGround = false;
			isblocked = false;
			running = false;
			jumping = false;
			jumpPressing = false;
			falling = true;
			attacking = false;
			dashing = false;
			sliding = false;
			ceiled = false;
			touching = false;
			hitted = false;
			coyote = false;
			death = false;
		}
	}
}

