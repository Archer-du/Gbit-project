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

		public PlayerState state;//very important

		[Header("Physics")]
		//[SerializeField] private float jumpImpulse;
		private float jumpHeight = 4f;
		private float gravityScale = 3f;
		private float fallGravityScale = 8f;

		[Header("Collision")]
		[SerializeField] private float distance;

		[Header("Basic")]
		private float speed = 6.0f;
		public int health { get; protected set; }
		protected int maxHealth = 100;

		[Header("Attack")]
		[SerializeField] private float attackTimer = 0f;
		[SerializeField] private float attackColdDown = 0f;
		[HideInInspector] public float attackPower = 1f;
		private AttackType attackType;
		private float attackTimeMax = 1.16f;
		private float attackInterval = 1f;

		[Header("Slide")]
		[SerializeField] private float slideTimer = 0f;
		[SerializeField] private float slideColdDown = 0f;
		private float slideTimeMax = 1f;
		private float slideBoxSize;
		private float slideBoxOffset;
		private float originBoxSize;
		private float originBoxOffset;

		[SerializeField] private float coyoteTimer = 0f;
		//private bool coyoteCheck = true;
		private float coyoteTimeMax = 0.2f;
		[SerializeField] private float jumpPressedTime = 0f;
		[SerializeField] private float jumpPressedWindow;

		public PlayerController()
		{
			state = new PlayerState();
		}
		private void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
			box = GetComponent<BoxCollider2D>();
			animator = GetComponent<Animator>();
		}
		private void Start()
		{
			distance = box.size.y / 2 - box.offset.y + 0.3f;
			slideBoxSize = box.size.y / 2;
			slideBoxOffset = box.offset.y / 2;
			originBoxSize = box.size.y;
			originBoxOffset = box.offset.y;
			jumpPressedWindow = Mathf.Sqrt(2 * jumpHeight / (-Physics2D.gravity.y * gravityScale));
		}
		private void Update()
		{
			float x = Input.GetAxis("Horizontal");//for testing
			transform.Translate(Vector2.right * Time.deltaTime * speed * x);

			AnimationState();
			Jump();
			Attack();
			Slide();
		}
		private void Jump()
		{
			state.onGround = Physics2D.Raycast(transform.position, Vector2.down, distance, groundLayer);
			if (state.falling && state.onGround)
			{
				state.jumping = false;
				state.falling = false;
				state.running = true;
			}
			if(rb.velocity.y < -0.01f)
			{
				state.falling = true;
				state.running = false;
			}
			if (state.jumpPressing)
			{
				jumpPressedTime += Time.deltaTime;
			}
			if (Input.GetButtonDown("Jump") && !state.attacking && (state.running || state.coyote))//warn the input lock
			{
				state.jumping = true;
				state.running = false;
				state.jumpPressing = true;

				jumpPressedTime = 0f;
				coyoteTimer = 0f;
				rb.gravityScale = gravityScale;
				rb.velocity = new Vector2(rb.velocity.x, Mathf.Sqrt(jumpHeight * (-Physics2D.gravity.y * gravityScale) * 2));
			}
			if (Input.GetButtonUp("Jump") || jumpPressedTime > jumpPressedWindow)
			{
				rb.gravityScale = fallGravityScale;
				state.jumpPressing = false;
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
			switch (attackType)
			{
				case AttackType.light:
				case AttackType.heavy:
				default: break;
			}
			if (state.attacking)
			{
				if(attackTimer < attackTimeMax)
				{
					//TODO:
				}
				else
				{
					state.attacking = false;
				}
				attackTimer += Time.deltaTime;
			}
			else
			{
				if(attackColdDown > 0)
				{
					attackColdDown -= Time.deltaTime;
				}
				else if (Input.GetMouseButton(0) && state.running)
				{
					attackTimer = 0f;
					attackColdDown = 1f;
					state.attacking = true;
					attackType = AttackType.light;//TODO:
				}
			}
		}//TODO:
		private void Slide()
		{
			if(state.sliding)
			{
				if (slideTimer < slideTimeMax)
				{
					box.size = new Vector2(box.size.x, slideBoxSize);
					box.offset = new Vector2(box.offset.x, slideBoxOffset);
				}
				else
				{
					box.size = new Vector2(box.size.x, originBoxSize);
					box.offset = new Vector2(box.offset.x, originBoxOffset);
					state.sliding = false;
				}
				slideTimer += Time.deltaTime;
			}
			else //not sliding
			{
				if(slideColdDown > 0)
				{
					slideColdDown -= Time.deltaTime;
				}
				else if (Input.GetButtonDown("Slide") && state.running)
				{
					slideTimer = 0f;
					slideColdDown = 2f;
					state.sliding = true;
				}
			}
		}
		private void Dash()
		{
		}
		//state machine
		private void AnimationState()
		{
			//animation adjustment
			animator.SetBool("running", state.running);
			animator.SetBool("jumping", state.jumping);
			animator.SetBool("falling", state.falling);
			animator.SetBool("attacking", state.attacking);
			animator.SetBool("sliding", state.sliding);
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

