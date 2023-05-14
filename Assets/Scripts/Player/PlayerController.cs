//author: Archer
//basic movements of the character
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
		public SpriteRenderer playerSprite;
		public Animator animator;
		public LayerMask groundLayer;

		[Header("State")]
		public PlayerState state;

		[Header("Basic")]
		private Vector2 velocity;
		private bool onGroundCheckFore;
		private bool onGroundCheckBack;
		[SerializeField] public int health { get; protected set; }
		protected int maxHealth = 100;

		[Header("Jump")]
		[SerializeField] private float jumpPressedTime;
		[SerializeField] private float jumpPressedWindow;
		[SerializeField] private float jumpHeight;
		[SerializeField] private float coyoteTimer;
		private float coyoteTimeMax;
		private float upwardGravityScale;
		private float downwardGravityScale;
		//private bool coyoteCheck = true;

		[Header("Attack")]
		[SerializeField] private bool comboEnable;
		[SerializeField] private int comboStep;
		[HideInInspector] public float attackPower = 1f;

		[Header("Slide")]
		[SerializeField] private float slideTimer;
		[SerializeField] private float slideColdDown;
		[SerializeField] private Vector2 slideBoxSize;
		[SerializeField] private float slideBoxOffset;
		[SerializeField] private Vector2 originBoxSize;
		[SerializeField] private float originBoxOffset;
		private float slideTimeMax;
		private float slideInterval;
		private float slideScaleX;
		private float slideScaleY;

		[Header("Dash")]
		[SerializeField] private float dashColdDown;
		private float dashInterval;
		private float dashSpeed;

		[Header("Collision")]
		[SerializeField] private float lowerDistance;
		[SerializeField] private float upperDistance;

		[Header("Test")]
		private float x;

		public PlayerController()
		{
			state = new PlayerState();
		}
		private void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
			box = GetComponent<BoxCollider2D>();
			playerSprite = GetComponent<SpriteRenderer>();
			animator = GetComponent<Animator>();
		}
		private void Start()
		{
			// status initialize:
			//Basic
			velocity = new Vector2(6f, 0);
			health = maxHealth;
			//Jump
			jumpPressedTime = 0f;
			coyoteTimer = 0f;
			coyoteTimeMax = 0.4f;
			jumpHeight = 4.5f;
			upwardGravityScale = 3f;
			downwardGravityScale = 8f;
			jumpPressedWindow = Mathf.Sqrt(2 * jumpHeight / (-Physics2D.gravity.y * upwardGravityScale));
			//slide
			slideTimer = 0f;
			slideColdDown = 0f;
			slideInterval = 1.2f;
			slideTimeMax = 0.7f;
			slideScaleY = 0.4f;
			slideScaleX = 2.5f;
			slideBoxSize = new Vector2(slideScaleX * box.size.x, slideScaleY * box.size.y);
			slideBoxOffset = box.offset.y - box.size.y * ((1 - slideScaleY) / 2f);
			originBoxSize = new Vector2(box.size.x, box.size.y);
			originBoxOffset = box.offset.y;
			//dash
			dashColdDown = 0f;
			dashInterval = 3f;
			dashSpeed = 12f;
			//collision
			lowerDistance = originBoxSize.y / 2f - originBoxOffset + 0.02f;
			upperDistance = originBoxSize.y / 2f + originBoxOffset + 0.02f;
		}
		private void Update()
		{
			x = Input.GetAxis("Horizontal");//for testing

			AnimationState();

			StateCheck();

			Jump();
			Coyote();
			Attack();
			Slide();
			Dash();
		}
		private void FixedUpdate()
		{
			transform.Translate(velocity * Time.deltaTime * x);
		}

		private void StateCheck()
		{
			onGroundCheckBack = Physics2D.Raycast(transform.position, Vector2.down, lowerDistance, groundLayer);
			onGroundCheckFore = Physics2D.Raycast(new Vector2(transform.position.x + 1, transform.position.y), Vector2.down, lowerDistance, groundLayer);
			state.onGround = onGroundCheckFore || onGroundCheckBack;

			if (rb.velocity.y < -0.01f)
			{
				if (!state.dashing)
				{
				state.falling = true;
				}
				state.running = false;
			}
			if (rb.velocity.y > 0.01f)//block the onGround check
			{
				if (!state.dashing)
				{
				state.jumping = true;
				}
				state.falling = false;
				state.running = false;
			}
			else if (state.onGround && !state.attacking && !state.sliding && !state.dashing)
			{
				state.jumping = false;
				state.falling = false;
				state.running = true;
			}
		}
		private void Jump()
		{
			if (state.jumpPressing)
			{
				jumpPressedTime += Time.deltaTime;
			}
			if (Input.GetButtonDown("Jump") && (state.running || state.coyote))
			{
				if (state.coyote)
				{
					animator.SetTrigger("coyote");
				}
				state.jumpPressing = true;
				jumpPressedTime = 0f;
				coyoteTimer = 0f;
				rb.gravityScale = upwardGravityScale;
				rb.velocity = new Vector2(rb.velocity.x, Mathf.Sqrt(jumpHeight * (-Physics2D.gravity.y * upwardGravityScale) * 2));
			}
			if (Input.GetButtonUp("Jump") || jumpPressedTime > jumpPressedWindow)
			{
				rb.gravityScale = downwardGravityScale;
				state.jumpPressing = false;
			}
		}
		private void Coyote()
		{
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
			if (Input.GetKeyDown(KeyCode.J))
			{
				if (state.running)
				{
					state.attacking = true;
					state.running = false;
					state.attackType = PlayerState.AttackType.light1;
					animator.SetTrigger("lightAttack1");
				}
				if(state.attacking && state.attackType == PlayerState.AttackType.light1 && comboEnable)
				{
					state.attackType = PlayerState.AttackType.light2;
				}//combo
			}
			if (Input.GetKeyDown(KeyCode.K))
			{
				if (state.running)
				{
					state.attacking = true;
					state.running = false;
					state.attackType = PlayerState.AttackType.heavy;
					animator.SetTrigger("heavyAttack");
				}
			}
		}
		public void ComboBegin()
		{
			comboEnable = true;
		}
		public void ComboOver()
		{
			if(state.attackType == PlayerState.AttackType.light2)
			{
				animator.SetTrigger("lightAttack2");
			}
			comboEnable = false;
		}
		public void AttackOver()
		{
			state.attacking = false;
		}
		public void RecoveryOver()
		{
			animator.SetTrigger("run");
		}

		private void Slide()
		{
			state.ceiled = Physics2D.Raycast(transform.position, Vector2.up, upperDistance, groundLayer);

			if (state.sliding)
			{
				if (slideTimer < slideTimeMax)
				{
					box.size = slideBoxSize;
					box.offset = new Vector2(box.offset.x, slideBoxOffset);
				}
				else if (!state.ceiled)//block
				{
					box.size = originBoxSize;
					box.offset = new Vector2(box.offset.x, originBoxOffset);
					state.sliding = false;
				}

				if (state.falling)//interrupt
				{
					box.size = originBoxSize;
					box.offset = new Vector2(box.offset.x, originBoxOffset);
					state.sliding = false;
				}
				slideTimer += Time.deltaTime;
			}
			else //if not sliding
			{
				if(slideColdDown > 0)
				{
					slideColdDown -= Time.deltaTime;
				}
				else if (Input.GetButtonDown("Slide") && state.running)
				{
					slideTimer = 0f;
					slideColdDown = slideInterval;
					state.running = false;
					state.sliding = true;
				}
			}
		}
		public void DashPrePos()
		{

		}
		private void Dash()
		{
			if (state.dashing)
			{
				ObjectPool.instance.PoolGet();
			}
			else //if not dashing
			{
				if (dashColdDown > 0)
				{
					dashColdDown -= Time.deltaTime;
				}
				else if (Input.GetButtonDown("Dash") && (state.running || state.jumping || state.falling))
				{
					animator.SetTrigger("dash");
					rb.gravityScale = 0;
					dashColdDown = dashInterval;
					state.running = false;
					state.jumping = false;
					state.falling = false;
					state.dashing = true;
					if (Input.GetKey(KeyCode.W))
					{
						rb.velocity = new Vector2(1, 1) * dashSpeed;
					}
					else
					{
						rb.velocity = new Vector2(1, 0) * dashSpeed;
					}
				}
			}
		}
		public void DashOver()
		{
			state.dashing = false;
			rb.gravityScale = downwardGravityScale;
		}
		//state machine
		private void AnimationState()
		{
			//animation adjustment
			animator.SetBool("running", state.running);
			animator.SetBool("jumping", state.jumping);
			animator.SetBool("falling", state.falling);
			animator.SetBool("sliding", state.sliding);
			animator.SetBool("attacking", state.attacking);
			animator.SetBool("dashing", state.dashing);
		}

		//debug functions
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(transform.position, transform.position + Vector3.down * lowerDistance);
			Gizmos.DrawLine(new Vector2(transform.position.x + 1, transform.position.y), new Vector2(transform.position.x + 1, transform.position.y) + Vector2.down * lowerDistance);
			Gizmos.DrawLine(transform.position, transform.position + Vector3.up * upperDistance);
		}
	}
}


