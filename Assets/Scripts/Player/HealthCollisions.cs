//author: Archer
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GbitProjectControl;

namespace GbitProjectState
{
	public class HealthCollisions : PlayerController
	{
		public void OnTriggerEnter2D(Collider2D other)
		{
			int damage = 1;//TODO:
			health -= damage;
			if (health <= 0)
			{
				Terminated();
			}
		}
		public void Terminated()
		{
			//TODO:
		}
	}
}
