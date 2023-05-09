//author: Archer
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GbitProjectControl
{
	public class HealthCollisions : PlayerController
	{
		public void OnTriggerEnter2D(Collider2D other)
		{
			Healed(1);
			Damaged(1);
		}
		public void Healed(int gained)
		{
			health = Mathf.Clamp(health + gained, 0, maxHealth);
		}
		public void Damaged(int damage)
		{
			health = Mathf.Clamp(health + damage, 0, maxHealth);
		}
		public void Terminated()
		{
			//TODO:
		}
	}
}
