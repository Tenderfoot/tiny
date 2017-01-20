using UnityEngine;
using System.Collections.Generic;
using InControl;

public class Player : Movable {

	// Object
	protected Rigidbody2D body;
	protected Animator animator;
	private AnimatorStates states;
	public PlayerAssignment assignment;

	// Controls
	public PlayerActions actions { get; set; }
	private float joystickThreshold = 0.5f;

	// Moving
	public float force = 20f;
	public float jumpForce = 20000f;
	public int extraJumps = 0;
	public int jumpsUsed = 0;
	public bool jumpReleased = true;
	private float feetOffset = 1.54f;

	// Wall Slide
	private float wallSlideSpeed = 5f;
	public Transform wallSlideCheck;
	private float wallCheckRadius = 0.2f;

	public LayerMask whatIsGround;

	private Collider2D ladder = null;

	// Health stuff
	private int health = 100;
	private int maxHealth = 100;
	private float deadTime;
	private int respawnTime = 5;

	// Levelling
	public int experienceValue = 50; //amount of exp you get for killing this player
	private Leveller leveller;
	private Dictionary<int, Powerup> LEVEL_MAP = new Dictionary<int, Powerup>()
	{
		{ 1, new MachineGunPowerup() },
		{ 2, new SpeedPowerup() },
		{ 3, new DoubleJumpPowerup() },
		{ 4, new PlasmaRiflePowerup() },
	};

	// Weapon
	public GameObject weapon1;

	// Use this for initialization
	void Start ()
	{
		body = gameObject.GetComponent<Rigidbody2D>();
		animator = transform.GetComponent<Animator>();

		states = new AnimatorStates(animator, new string[] {
			"walk",
			"idle",
			"jump",
			"climb",
			"death",
			"wall_slide"
		});

		leveller = new Leveller(this, LEVEL_MAP);

		states.ChangeState("idle");
	}

	// Update is called once per frame
	void Update()
	{
		if (!IsDead())
		{
			CheckFire();
			CheckLadder();

			if (OnLadder() && VerticalAction())
				Climb();				
			else if (HorizontalAction())
				Walk();
			else
				Idle();
		}

		UpdateVitals();

	}

	void FixedUpdate()
	{
		CheckJump();

		if (IsWallSliding())
			WallSlide();
	}

	public void AddExperience(int exp) { leveller.AddExperience(exp); }

	void TogglePlayer(bool alive)
	{
		foreach (var collider in this.gameObject.GetComponents<Collider2D>())
		{
			collider.enabled = alive;
		}
		((Rigidbody2D)this.gameObject.GetComponent<Rigidbody2D>()).isKinematic = !alive;
	}

	void KillPlayer()
	{
		states.ChangeState("death");
		health = 0;
		TogglePlayer(false);
	}

	void RespawnPlayer()
	{
		health = maxHealth;
		TogglePlayer(true);
		gameObject.transform.position = new Vector3(0, 50, 0);
		deadTime = 0;
	}

	public int GetSpawnTimeLeft()
	{
		return respawnTime - ((int)deadTime);
	}

	protected bool IsWallSliding()
	{
		return Physics2D.OverlapCircle(wallSlideCheck.position, wallCheckRadius, whatIsGround) && !IsGrounded() && (actions.Left.IsPressed || actions.Right.IsPressed);
	}

	protected bool IsGrounded()
	{
		return Physics2D.Linecast(transform.position, new Vector2((float)transform.position.x, transform.position.y - feetOffset), whatIsGround);
	}

	protected void WallSlide()
	{
		states.ChangeState("wall_slide");
		body.velocity = new Vector2(body.velocity.x, Vector2.down.y * wallSlideSpeed);
	}

	protected void Jump()
	{
		Vector2 force = (transform.up * jumpForce);
		body.velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);
		body.AddForce(force);
		jumpReleased = false;
	}

	protected void CheckWallSlide()
	{
		if ((actions.Right.IsPressed || actions.Left.IsPressed))
		{
			states.ChangeState("wall_slide");
		}
	}

	protected void CheckJump()
	{

		if (!actions.Jump.IsPressed)
		{
			jumpReleased = true;
		}

		if ((IsGrounded() || IsWallSliding()) && jumpsUsed > 0)
		{
			jumpsUsed = 0;
		}

		if (actions.Jump.IsPressed && jumpReleased) 
		{
			if (IsGrounded() || IsWallSliding())
			{
				Jump();
			}
			else if (extraJumps > jumpsUsed)
			{
				Jump();
				jumpsUsed += 1;
			}


		}

	}

	public bool IsDead()
	{
		return health <= 0;
	}

	void CheckFire()
	{
		if (weapon1 != null)
		{
			if (actions.Trigger.IsPressed)
			{
				
				weapon1.GetComponent<TinyWeapon>().OnTriggerPressed();
			}
			else if (actions.Trigger.WasReleased)
			{
				weapon1.GetComponent<TinyWeapon>().OnTriggerReleased();
			}
		}
	}

	public override void HandleHit(TinyObject tiny)
	{
		// Once we have different things hitting the player we can diversify this
		var projectile = (Projectile)tiny;
		health -= projectile.damage;
		if(health<0)
		{
			KillPlayer();
			projectile.owner.AddExperience(experienceValue);
		}
	}

	// player death and respawn
	void UpdateVitals()
	{
		if (gameObject.transform.position.y < -25 && !IsDead())
			KillPlayer();

		if (health == 0)
			deadTime += Time.deltaTime;

		if (deadTime > respawnTime && IsDead())
			RespawnPlayer();
	}

	protected void CheckLadder()
	{
		var r = GetComponent<Renderer>();

		var corner1 = r.bounds.center + r.bounds.extents / 2;
		var corner2 = r.bounds.center - r.bounds.extents / 2;
		var prevLadder = ladder;

		ladder = Physics2D.OverlapArea(corner1, corner2, 1 << LayerMask.NameToLayer("ladder"));

		var isClimbing = actions.Up.Value > joystickThreshold || actions.Down.Value > joystickThreshold;

		if (ladder == null && prevLadder != null && isClimbing)
		{
			body.velocity = new Vector2(0, 0);
		}
	}

	protected bool OnLadder()
	{
		return ladder != null;
	}

	protected bool VerticalAction()
	{
		return actions.Up.Value > joystickThreshold || actions.Down.Value > joystickThreshold;
	}

	protected bool HorizontalAction()
	{
		return actions.Right.Value > joystickThreshold || actions.Left.Value > joystickThreshold;
	}

	public float HealthAsPercent()
	{
		float result = ((float)health) / 100;
		return result;
	}


	// MOVEMENTS
	protected void Idle()
	{
		states.ChangeState("idle");
		body.velocity = new Vector2(0, body.velocity.y);
	}

	protected void Climb()
	{
		states.ChangeState("climb");


		if (actions.Up.Value > joystickThreshold)
			body.velocity = new Vector2(0, force);
		else if (actions.Down.Value > joystickThreshold)
			body.velocity = new Vector2(0, -force);

		MoveHorizontal();
	}

	protected void Walk()
	{
		states.ChangeState("walk");

		MoveHorizontal();
	}

	protected void MoveHorizontal()
	{
		Vector2 forceVector = body.velocity;

		if (actions.Left.Value > joystickThreshold)
		{
			forceVector = Vector2.left * force;
			FaceLeft();
		}
		else if (actions.Right.Value > joystickThreshold)
		{
			forceVector = Vector2.right * force;
			FaceRight();
		}

		body.velocity = new Vector2(forceVector.x, body.velocity.y);
	}
}
