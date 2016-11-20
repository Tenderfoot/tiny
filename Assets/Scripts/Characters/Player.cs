using UnityEngine;
using System.Collections.Generic;
using InControl;

public class Player : Movable {

	// Object
	protected Rigidbody2D body;
	protected Animator animator;
	protected InputDevice input;
	private AnimatorStates states;

	// Controls
	public PlayerActions actions { get; set; }
	private float joystickThreshold = 0.8f;

	// Moving
	public float force = 20f;
	public float jumpForce = 20000f;
	private float feetOffset = 1.54f;

	public Transform ladder;

	// Health stuff
	private int health;
	private int maxHealth;
	private float deadTime;
	private int respawnTime;

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

	// Use this for initialization
	void Start ()
	{
		body = gameObject.GetComponent<Rigidbody2D>();
		animator = transform.GetComponent<Animator>();

		// some player variables...
		health = 25;
		maxHealth = 100;
		respawnTime = 5;

		states = new AnimatorStates(animator, new string[] {
			"walk",
			"idle",
			"jump",
			"climb"
		});

		states.ChangeState("idle");
	}

	protected bool IsGrounded()
	{
		return Physics2D.Linecast(transform.position, new Vector2((float)transform.position.x, transform.position.y - feetOffset), 1 << LayerMask.NameToLayer("ground"));
	}

	void FixedUpdate()
	{
		if (actions.Jump.IsPressed && IsGrounded())
		{
			Vector2 force = (transform.up * jumpForce);
			body.AddForce(force);
		}
	}

	public bool IsDead()
	{
		return health == 0;
	}

	// Update is called once per frame
	void Update ()
	{
		if (OnLadder() && VerticalAction())
			Climb();
		else if (HorizontalAction())
			Walk();
		else
			Idle();

		UpdateVitals();

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

	protected void Idle()
	{
		states.ChangeState("idle");
		body.velocity = new Vector2(0, body.velocity.y);
	}

	protected void Climb()
	{
		Idle();
		states.ChangeState("climb");

		if (actions.Up.Value > joystickThreshold)
			body.velocity = new Vector2(0, force);
		else if (actions.Down.Value > joystickThreshold)
			body.velocity = new Vector2(0, -force);

		transform.position = new Vector2(ladder.position.x, transform.position.y);
	}

	public float HealthAsPercent()
	{
		float result = ((float)health) / 100;
		return result;
	}

	protected void Walk()
	{
		states.ChangeState("walk");

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
