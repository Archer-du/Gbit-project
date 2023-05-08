//author: Archer
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GbitProject
{
	public class PlayerController : MonoBehaviour
	{
		public Rigidbody2D rb;

		private int state;

		[Header("Physics")]
		[SerializeField] private float jumpImpulse;
		[SerializeField] private float jumpHeight = 3f;
		[SerializeField] private float gravityScale = 5f;
		[SerializeField] private float fallGravityScale = 10f;

		private float speed = 6.0f;
		public PlayerController()
		{

		}
		void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
		}
		void Update()
		{
			float x = Input.GetAxis("Horizontal");
			transform.Translate(Vector2.right * Time.deltaTime * x * speed);
			MovementAdjust();
			if (Input.GetButtonDown("Jump"))
			{
				Jump();
			}
		}
		private void Jump()
		{
			rb.velocity = new Vector2(rb.velocity.x, Mathf.Sqrt(jumpHeight * (-Physics2D.gravity.y * rb.gravityScale) * 2));
			//rb.AddForce(Vector2.up * jumpImpulse, ForceMode2D.Impulse);
		}
		private void MovementAdjust()
		{
			//gravity adjustment
			if (rb.velocity.y >= -0.2f)
			{
				rb.gravityScale = gravityScale;
			}
			if (rb.velocity.y < -0.2f)
			{
				rb.gravityScale = fallGravityScale;
			}
		}
	}
}

