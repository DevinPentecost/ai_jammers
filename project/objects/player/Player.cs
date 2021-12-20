using System;
using Godot;
using GodotProject.objects.MachineInterface;

public class Player : Area2D, IPlayerStatus
{
	private IDiscStatus _disc = null;
	[Export] public bool ManualControl;

	[Export] public IPlayerInput PlayerInput = new PlayerInput();

	[Export] public float Speed { get; set; } = 100;

	[Export] public int PlayerIndex { get; set; }

	[Export] public PlayerState State { get; set; }
	[Export] public bool HoldingDisc { get; set; }
	public int DashStep
	{
		get => _dash.DashStep;
		set => _dash.DashStep = value;
	}

	private Dash _dash = new Dash();
	private Vector2 _dashDirection = Vector2.Zero;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetProcessUnhandledKeyInput(ManualControl);
	}

	public override void _PhysicsProcess(float delta)
	{
		//If we have the disc, we can't move
		if (HoldingDisc)
		{
			_ProcessHoldingDisc(delta);
			return;
		}

		switch (State)
		{
			case PlayerState.Regular:
				_HandleStartDash();
				_ProcessRunningMovement(delta);
				break;
			case PlayerState.Dashing:
				_ProcessDashMovement(delta);
				break;
			case PlayerState.Custom:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private void _HandleStartDash()
	{
		//Did the player dash?
		if (PlayerInput.A)
		{
			//Which direction were they pressing
			_dashDirection = Vector2.Zero;
			if (PlayerInput.Left) _dashDirection += Vector2.Left;
			if (PlayerInput.Right) _dashDirection += Vector2.Right;
			if (PlayerInput.Up) _dashDirection += Vector2.Up;
			if (PlayerInput.Down) _dashDirection += Vector2.Down;
			
			// No direction, no dash
			if (_dashDirection == Vector2.Zero) return;
			
			//Begin a dash
			State = PlayerState.Dashing;
			_dash.Reset();
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

	private void _ProcessDashMovement(float delta)
	{
		//Continue moving in the same direction but at a new speed
		var speed = Speed * _dash.SpeedSteps[DashStep];
		var movement = _dashDirection * speed * delta;
		var transform = Transform;
		transform.origin += movement;
		Transform = transform;
		
		// Move to the next step
		DashStep += 1;
		if (!_dash.Dashing)
		{
			_dash.Reset();
			State = PlayerState.Regular;
		}
	}

	private void _ProcessHoldingDisc(float delta)
	{
		if (PlayerInput.A)
		{
			//Throw the disc
			_ThrowDisc();
		}
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

	public void _on_Player_area_entered(Area2D other)
	{
		switch (other)
		{
			case IDiscStatus disc:
				_CatchDisc(disc);
				break;
		}
	}

	private void _CatchDisc(IDiscStatus disc)
	{
		if (disc.ThrowingPlayer == PlayerIndex) return;

		// Catch it!
		_disc = disc;
		disc.ThrowingPlayer = PlayerIndex;
		HoldingDisc = true;
		disc.State = DiscState.Caught;
	}

	private void _ThrowDisc()
	{
		if (_disc.ThrowingPlayer != PlayerIndex) return;

		//Get the direction from the player
		var direction = PlayerIndex == 0 ? 0 : Mathf.Pi;
		if (PlayerInput.Left) direction = Mathf.Tau * (4 / 8F);
		if (PlayerInput.Right) direction = Mathf.Tau * (0 / 8F);
		if (PlayerInput.Up) direction = Mathf.Tau * (2 / 8F);
		if (PlayerInput.Down) direction = Mathf.Tau * (6 / 8F);
		if (PlayerInput.Left && PlayerInput.Up) direction = Mathf.Tau * (3 / 8F);
		if (PlayerInput.Right && PlayerInput.Up) direction = Mathf.Tau * (1 / 8F);
		if (PlayerInput.Left && PlayerInput.Down) direction = Mathf.Tau * (5 / 8F);
		if (PlayerInput.Right && PlayerInput.Down) direction = Mathf.Tau * (7 / 8F);

		//Any curve on it?
		_disc.Curve = 0;

		//Throw it!
		_disc.ThrowingPlayer = null;
		_disc.State = DiscState.Flying;
		_disc.Direction = direction;
		_disc = null;
		HoldingDisc = false;
	}
}