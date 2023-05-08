//author: Archer
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GbitProjectState;
using Unity.VisualScripting;

namespace GbitProjectControl
{
	public class PlayerController : MonoBehaviour
	{
		[Header("Components")]
		public Rigidbody2D rb;
		public BoxCollider2D box;
		public Animator animator;
		public LayerMask groundLayer;

		[SerializeField] private PlayerState state;

		[Header("Physics")]
		//[SerializeField] private float jumpImpulse;
		private float jumpHeight = 3f;
		private float gravityScale = 5f;
		private float fallGravityScale = 12f;

		[Header("Collision")]
		[SerializeField] private float distance;

		[Header("Basic")]
		private float speed = 6.0f;
		private Quaternion stdRotation;
		public int health { get; protected set; }

		[Header("Manipulation")]
		private float coyoteCounter = 0f;
		private float coyoteMax = 0f;
		private float jumpPressedWindow = 0.4f;
		[SerializeField] private float jumpPressedTime = 0f;
		[SerializeField] private bool pressing = false;
		public PlayerController()
		{
			state = new PlayerState();
		}
		private void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
			box = GetComponent<BoxCollider2D>();
		}
		private void Start()
		{
			stdRotation = transform.rotation;
			distance = box.size.y / 2 - box.offset.y + 0.6f;
		}
		private void Update()
		{
			float x = Input.GetAxis("Horizontal");//for testing
			transform.Translate(Vector2.right * Time.deltaTime * speed * x);
			StateAdjustment();
			Jump();
		}
		private void Jump()
		{
			if (pressing)
			{
				jumpPressedTime += Time.deltaTime;
			}
			if (Input.GetButtonDown("Jump") && (state.jumping == false))
			{
				rb.gravityScale = gravityScale;
				pressing = true;
				jumpPressedTime = 0f;
				rb.velocity = new Vector2(rb.velocity.x, Mathf.Sqrt(jumpHeight * (-Physics2D.gravity.y * rb.gravityScale) * 2));
			}
			if (Input.GetButtonUp("Jump") || jumpPressedTime > jumpPressedWindow)
			{
				rb.gravityScale = fallGravityScale;
				pressing = false;
			}
		}

		//state machine
		private void StateAdjustment()
		{
			//rotation adjustment
			transform.rotation = stdRotation;

			//state adjustment
			if(Physics2D.Raycast(transform.position, Vector2.down, distance, groundLayer))
			{
				state.running = true;
				state.jumping = false;
				state.falling = false;
			}
			else if (rb.velocity.y > 0.05f)
			{
				state.running = false;
				state.jumping = true;
				state.falling = false;
			}
			else if (rb.velocity.y < -0.05f)
			{
				state.falling = true;
			}
		}



		//debug functions
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(transform.position, transform.position + Vector3.down * distance);
		}
	}
}

