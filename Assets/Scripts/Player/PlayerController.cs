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
		[SerializeField] private Vector2 velocity;
		[SerializeField] public int health { get; protected set; }
		[SerializeField] protected int maxHealth = 100;

		[Header("Jump")]
		[SerializeField] private float jumpPressedTime = 0f;
		[SerializeField] private float jumpPressedWindow;
		[SerializeField] private float jumpHeight = 4.5f;
		[SerializeField] private float upwardGravityScale = 3f;
		[SerializeField] private float downwardGravityScale = 8f;

		[Header("Coyote")]
		[SerializeField] private float coyoteTimer = 0f;
		[SerializeField] private float coyoteTimeWindow = 0.4f;

		[Header("Attack")]
		[SerializeField] private bool comboEnable;
		[HideInInspector] public float attackPower = 1f;

		[Header("Slide")]
		[SerializeField] private float slideTimer = 0f;
		[SerializeField] private float slideColdDown = 0f;
		[SerializeField] private float slideTimeWindow = 0.7f;
		[SerializeField] private float slideInterval = 1.2f;
		[SerializeField] private float slideScaleX = 0.4f;
		[SerializeField] private float slideScaleY = 2.5f;
		[SerializeField] private Vector2 slideBoxSize;
		[SerializeField] private float slideBoxOffset;
		[SerializeField] private Vector2 originBoxSize;
		[SerializeField] private float originBoxOffset;

		[Header("Dash")]
		[SerializeField] private float dashColdDown = 0f;
		[SerializeField] private float dashInterval = 3f;
		[SerializeField] private float dashSpeed = 12f;

		[Header("EnvironmentDetection")]
		private float lowerDistance;
		private float upperDistance;
		private float foreDistance;
		private RaycastHit2D onGroundCheckFore;
		private RaycastHit2D onGroundCheckBack;
		private RaycastHit2D ceilCheck;
		private RaycastHit2D hangingCheckUpper;
		private RaycastHit2D hangingCheckLower;
		private RaycastHit2D hangingCheckDownward;
		private Vector2 onGroundForePoint;
		private Vector2 onGroundBackPoint;
		private Vector2 hangingUpperPoint;
		private Vector2 hangingLowerPoint;
		private Vector2 hangingDownwardPoint;

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
			jumpPressedWindow = Mathf.Sqrt(2 * jumpHeight / (-Physics2D.gravity.y * upwardGravityScale));
			//slide
			slideBoxSize = new Vector2(slideScaleX * box.size.x, slideScaleY * box.size.y);
			slideBoxOffset = box.offset.y - box.size.y * ((1 - slideScaleY) / 2f);
			originBoxSize = new Vector2(box.size.x, box.size.y);
			originBoxOffset = box.offset.y;
			//collision
			lowerDistance = originBoxSize.y / 2f - box.offset.y + 0.02f;
			upperDistance = originBoxSize.y / 2f + box.offset.y + 0.02f;
			foreDistance = originBoxSize.x / 2f + box.offset.x + 0.5f;
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

		//environment detection method
		private void StateCheck()
		{
			onGroundForePoint = new Vector2(transform.position.x + 1, transform.position.y);
			onGroundBackPoint = new Vector2(transform.position.x, transform.position.y);
			hangingUpperPoint = new Vector2(transform.position.x, transform.position.y + 1);
			hangingLowerPoint = new Vector2(transform.position.x, transform.position.y - 1);
			hangingDownwardPoint = new Vector2(transform.position.x + 1.5f, transform.position.y + 1);

			onGroundCheckFore = Physics2D.Raycast(onGroundForePoint, Vector2.down, lowerDistance, groundLayer);
			onGroundCheckBack = Physics2D.Raycast(onGroundBackPoint, Vector2.down, lowerDistance, groundLayer);
			ceilCheck = Physics2D.Raycast(onGroundBackPoint, Vector2.up, upperDistance, groundLayer);
			hangingCheckUpper = Physics2D.Raycast(hangingUpperPoint, Vector2.right, foreDistance, groundLayer);
			hangingCheckLower = Physics2D.Raycast(hangingLowerPoint, Vector2.right, foreDistance, groundLayer);
			hangingCheckDownward = Physics2D.Raycast(hangingDownwardPoint, Vector2.down, foreDistance, groundLayer);//TODO:

			state.onGround = onGroundCheckFore || onGroundCheckBack;
			state.hanging = !hangingCheckUpper && hangingCheckLower && hangingCheckDownward;
			state.ceiled = ceilCheck;

			//state adjustment
			if (rb.velocity.y < -0.01f)
			{
				if (!state.dashing)
				{
				state.falling = true;
				}
				state.running = false;
			}
			if (rb.velocity.y > 0.01f)//block the running check
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

		//Basic Movement methods
		private void Jump()
		{
			if (state.jumpPressing)
			{
				jumpPressedTime += Time.deltaTime;
				if (Input.GetButtonUp("Jump") || jumpPressedTime > jumpPressedWindow)
				{
					rb.gravityScale = downwardGravityScale;
					state.jumpPressing = false;
				}
			}
			else
			{
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
			}
		}
		private void Coyote()
		{
			if (!state.jumping && state.falling && !state.running)
			{
				if (coyoteTimer < coyoteTimeWindow)
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
		private void Slide()
		{
			if (state.sliding)
			{
				if (slideTimer < slideTimeWindow)
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
		private void Dash()
		{
			if (!state.dashing)
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
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag("Enemy"))
			{
				//enemy hitted
			}
		}

		//animation frame event
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

		public void DashShadowInstantiate()
		{
			ObjectPool.instance.PoolGet();
		}
		public void DashOver()
		{
			state.dashing = false;
			rb.gravityScale = downwardGravityScale;
			rb.velocity = new Vector2(rb.velocity.x, 0);
		}

		//animation state machine
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
			Gizmos.DrawLine(onGroundBackPoint, onGroundBackPoint + Vector2.down * lowerDistance);
			Gizmos.DrawLine(onGroundForePoint, onGroundForePoint + Vector2.down * lowerDistance);
			Gizmos.DrawLine(onGroundBackPoint, onGroundBackPoint + Vector2.up * upperDistance);
			Gizmos.color = Color.green;
			Gizmos.DrawLine(hangingUpperPoint, hangingUpperPoint + Vector2.right * foreDistance);
			Gizmos.DrawLine(hangingLowerPoint, hangingLowerPoint + Vector2.right * foreDistance);
			Gizmos.DrawLine(hangingDownwardPoint, hangingDownwardPoint + Vector2.down * foreDistance);
		}
	}
}


