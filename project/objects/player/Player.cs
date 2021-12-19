using System;
using Godot;
using GodotProject.objects.MachineInterface;

public class Player : Area2D, IPlayerStatus
{
	[Export] public bool ManualControl;

	[Export] public IPlayerInput PlayerInput = new PlayerInput();

	[Export] public float Speed { get; set; } = 100;

	[Export] public int PlayerIndex { get; set; }

	[Export] public PlayerState State { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetProcessUnhandledKeyInput(ManualControl);
	}

	public override void _PhysicsProcess(float delta)
	{
		switch (State)
		{
			case PlayerState.Regular:
				_ProcessRunningMovement(delta);
				break;
			case PlayerState.Dashing:
				break;
			case PlayerState.Holding:
				break;
			case PlayerState.Custom:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private void _ProcessRunningMovement(float delta)
	{
		//Determine direction of movement
		var direction = Vector2.Zero;
		if (PlayerInput.Left) direction.x = -1;
		if (PlayerInput.Right) direction.x = 1;
		if (PlayerInput.Up) direction.y = -1;
		if (PlayerInput.Down) direction.y = 1;

		//Move that amount
		var movement = direction * Speed * delta;
		var transform = Transform;
		transform.origin += movement;
		Transform = transform;
	}

	public override void _UnhandledKeyInput(InputEventKey @event)
	{
		if (@event.IsAction("player_1_left")) PlayerInput.Left = @event.Pressed;
		if (@event.IsAction("player_1_right")) PlayerInput.Right = @event.Pressed;
		if (@event.IsAction("player_1_up")) PlayerInput.Up = @event.Pressed;
		if (@event.IsAction("player_1_down")) PlayerInput.Down = @event.Pressed;
		if (@event.IsAction("player_1_a")) PlayerInput.A = @event.Pressed;
		if (@event.IsAction("player_1_b")) PlayerInput.B = @event.Pressed;
	}
}