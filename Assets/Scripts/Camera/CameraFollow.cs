using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace GbitProjectCamera
{
	public class CameraFollow : MonoBehaviour
	{
		public GameObject player;

		private Vector3 offset = new Vector3(2f, 0.5f, -12);
		private Vector3 targetPos;
		private float followSpeed = 4f;

		private float upperBound = 10f;

		private void Start()
		{
		}
		private void Update()
		{
			targetPos = player.transform.position;
			if(transform.position.y > upperBound)
			{
				transform.position = new Vector3(transform.position.x, upperBound, -12);
			}
			else
			{
				transform.position = Vector3.Lerp(transform.position, targetPos + offset, followSpeed * Time.deltaTime);
			}
		}
	}
}
