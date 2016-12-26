using InControl;
using UnityEngine;


public class PlayerActions : PlayerActionSet
{
	public PlayerAction Left;
	public PlayerAction Right;
	public PlayerAction Up;
	public PlayerAction Down;
	public PlayerAction Jump;
	public PlayerAction Trigger;
	public PlayerOneAxisAction Move;

	// Aiming
	public PlayerAction AimLeft;
	public PlayerAction AimRight;
	public PlayerAction AimUp;
	public PlayerAction AimDown;
	public PlayerTwoAxisAction Aim;

	public PlayerAction Join;

	public PlayerActions()
	{
		Left = CreatePlayerAction("Move Left");
		Right = CreatePlayerAction("Move Right");
		Up = CreatePlayerAction("Move Up");
		Down = CreatePlayerAction("Move Down");
		Jump = CreatePlayerAction("Jump");
		Trigger = CreatePlayerAction("Trigger");

		Move = CreateOneAxisPlayerAction(Left, Right);

		// Aiming
		AimLeft = CreatePlayerAction("Aim Left");
		AimRight = CreatePlayerAction("Aim Right");
		AimUp = CreatePlayerAction("Aim Up");
		AimDown = CreatePlayerAction("Aim Down");
		Aim = CreateTwoAxisPlayerAction(AimLeft, AimRight, AimUp, AimDown);

		Join = CreatePlayerAction("Join");
	}


	public static PlayerActions CreateWithDefaultBindings()
	{
		var playerActions = new PlayerActions();

		// Movement
		// Keys
		playerActions.Left.AddDefaultBinding(Key.LeftArrow);
		playerActions.Right.AddDefaultBinding(Key.RightArrow);
		
		// Sticks
		playerActions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
		playerActions.Right.AddDefaultBinding(InputControlType.LeftStickRight);
		playerActions.Up.AddDefaultBinding(InputControlType.LeftStickUp);
		playerActions.Down.AddDefaultBinding(InputControlType.LeftStickDown);

		// Aim
		playerActions.AimLeft.AddDefaultBinding(InputControlType.RightStickLeft);
		playerActions.AimRight.AddDefaultBinding(InputControlType.RightStickRight);
		playerActions.AimUp.AddDefaultBinding(InputControlType.RightStickDown);
		playerActions.AimDown.AddDefaultBinding(InputControlType.RightStickUp);

		// D-Pad
		playerActions.Left.AddDefaultBinding(InputControlType.DPadLeft);
		playerActions.Right.AddDefaultBinding(InputControlType.DPadRight);
		
		// Buttons
		playerActions.Jump.AddDefaultBinding(InputControlType.Action1);
		playerActions.Trigger.AddDefaultBinding(InputControlType.RightTrigger);

		// Joining the game
		playerActions.Join.AddDefaultBinding(InputControlType.Start);
		playerActions.Join.AddDefaultBinding(Key.Return);

		return playerActions;
	}
}
