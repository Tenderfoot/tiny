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

	private Collider2D ladder = null;

	// Health stuff
	private int health = 100;
	private int maxHealth = 100;
	private float deadTime;
	private int respawnTime = 5;

	// Levelling
	private Leveller leveller;
	private Dictionary<int, Powerup> LEVEL_MAP = new Dictionary<int, Powerup>()
	{
		{ 1, new MachineGunPowerup() },
		{ 2, new SpeedPowerup() },
		{ 3, new DoubleJumpPowerup },
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
			"climb"
		});

		leveller = new Leveller(this, LEVEL_MAP);

		states.ChangeState("idle");
	}

	// Update is called once per frame
	void Update()
	{
		CheckFire();
		CheckLadder();

		if (!IsDead())
		{
			if (OnLadder() && VerticalAction())
				Climb();
			else if (HorizontalAction())
				Walk();
			else
				Idle();
		}

		UpdateVitals();

	}

	public void AddExperience(int exp) { leveller.AddExperience(exp); }

	void TogglePlayer(bool alive)
	{
		foreach (var collider in this.gameObject.GetComponents<Collider2D>())
		{
			collider.enabled = alive;
		}
		((SpriteRenderer)this.gameObject.GetComponent<SpriteRenderer>()).enabled = alive;
		((Rigidbody2D)this.gameObject.GetComponent<Rigidbody2D>()).isKinematic = !alive;
	}

	void KillPlayer()
	{
		health = 0;
		TogglePlayer(false);
		deadTime = 0;
	}

	void RespawnPlayer()
	{
		health = maxHealth;
		TogglePlayer(true);
		gameObject.transform.position = new Vector3(0, 50, 0);
	}

	public int GetSpawnTimeLeft()
	{
		return respawnTime - ((int)deadTime);
	}

	protected bool IsGrounded()
	{
		return Physics2D.Linecast(transform.position, new Vector2((float)transform.position.x, transform.position.y - feetOffset), 1 << LayerMask.NameToLayer("ground"));
	}

	protected void Jump()
	{
		Vector2 force = (transform.up * jumpForce);
		body.velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);
		body.AddForce(force);
		jumpReleased = false;
	}

	protected void CheckJump()
	{

		if (!actions.Jump.IsPressed)
		{
			jumpReleased = true;
		}

		if (IsGrounded() && jumpsUsed > 0)
		{
			jumpsUsed = 0;
		}

		if (actions.Jump.IsPressed && jumpReleased) 
		{
			if (IsGrounded())
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

	void FixedUpdate()
	{
		CheckJump();
	}

	public bool IsDead()
	{
		return health == 0;
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
		health -= ((Damager)tiny).damage;
		if(health<0)
		{
			KillPlayer();
		}

		GameObject.Destroy(tiny.gameObject);
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
