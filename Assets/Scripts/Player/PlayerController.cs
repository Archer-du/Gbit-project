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

		public PlayerState state;

		[Header("Physics")]
		//[SerializeField] private float jumpImpulse;
		private float jumpHeight = 3f;
		private float gravityScale = 3f;
		private float fallGravityScale = 10f;

		[Header("Collision")]
		[SerializeField] private float distance;

		[Header("Basic")]
		private float speed = 6.0f;
		public int health { get; protected set; }
		protected int maxHealth = 100;

		[Header("Manipulation")]
		private float attackRange = 1f;
		private float attackPower = 1f;
		private float attackInterval = 1f;
		private float attackTimer = 0f;
		private AttackType attackType;

		private float slideTimeMax = 1f;
		private float slideInterval = 1f;
		private float slideTimer = 0f;
		private float slideColdDown = 0f;

		[SerializeField] private float coyoteTimer = 0f;
		private bool coyoteCheck = true;
		private float coyoteTimeMax = 0.4f;//TODO:
		[SerializeField] private float jumpPressedWindow;
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
			distance = box.size.y / 2 - box.offset.y + 0.6f;
			jumpPressedWindow = Mathf.Sqrt(2 * jumpHeight / (-Physics2D.gravity.y * gravityScale));
		}
		private void Update()
		{
			float x = Input.GetAxis("Horizontal");//for testing
			transform.Translate(Vector2.right * Time.deltaTime * speed * x);

			StateAdjustment();
			Jump();
			Attack();
		}
		private void Jump()
		{
			if (pressing)
			{
				jumpPressedTime += Time.deltaTime;
			}
			if (Input.GetButtonDown("Jump") && (state.running || state.coyote))//warn the input lock
			{
				pressing = true;
				jumpPressedTime = 0f;
				coyoteTimer = 0f;
				rb.gravityScale = gravityScale;
				rb.velocity = new Vector2(rb.velocity.x, Mathf.Sqrt(jumpHeight * (-Physics2D.gravity.y * gravityScale) * 2));
			}
			if (Input.GetButtonUp("Jump") || jumpPressedTime > jumpPressedWindow)
			{
				rb.gravityScale = fallGravityScale;
				pressing = false;
			}

			//coyote time
			if (!state.jumping && state.falling && !state.running)
			{
				if (coyoteTimer < coyoteTimeMax)
				{
					if (!state.coyote)
					{
						state.coyote = true;
					}
					coyoteTimer += Time.deltaTime;
				}
				else state.coyote = false;
			}
			else state.coyote = false;
		}
		private void Attack()
		{
			if (Input.GetMouseButton(0) && !state.attacking && attackTimer < attackInterval && state.running)
			{
				attackTimer += Time.deltaTime;
				state.attacking = true;
				attackType = AttackType.light;//TODO:
			}
		}//TODO:
		private void Slide()
		{
			if (Input.GetButtonDown("Slide") && !state.sliding && slideColdDown >= slideInterval && state.running)
			{
				slideTimer += Time.deltaTime;
				state.sliding = true;

			}
		}
		private void Dash()
		{
		}
		//state machine
		private void StateAdjustment()
		{
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
				state.running = false;
				state.falling = true;
			}
		}

		public enum AttackType
		{
			light,
			heavy
		}

		//debug functions
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(transform.position, transform.position + Vector3.down * distance);
		}
	}
}

