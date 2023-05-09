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
			touching = false;
			hitted = false;
			coyote = false;
			death = false;
		}
	}
}

