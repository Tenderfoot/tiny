using InControl;
using UnityEngine;


public class PlayerActions : PlayerActionSet
{
	public PlayerAction Left;
	public PlayerAction Right;
	public PlayerAction Up;
	public PlayerAction Down;
	public PlayerAction Jump;
	public PlayerOneAxisAction Move;

	public PlayerAction Join;

	public PlayerActions()
	{
		Left = CreatePlayerAction("Move Left");
		Right = CreatePlayerAction("Move Right");
		Up = CreatePlayerAction("Move Up");
		Down = CreatePlayerAction("Move Down");
		Jump = CreatePlayerAction("Jump");
		Move = CreateOneAxisPlayerAction(Left, Right);

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
		// D-Pad
		playerActions.Left.AddDefaultBinding(InputControlType.DPadLeft);
		playerActions.Right.AddDefaultBinding(InputControlType.DPadRight);
		// Buttons
		playerActions.Jump.AddDefaultBinding(InputControlType.Action1);

		playerActions.Jump.AddDefaultBinding(InputControlType.Action1);

		// Joining the game
		playerActions.Join.AddDefaultBinding(InputControlType.Start);
		playerActions.Join.AddDefaultBinding(Key.Return);

		return playerActions;
	}
}
