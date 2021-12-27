using System;
using Godot;
using GodotProject.objects.MachineInterface;

public class Disc : Area2D, IDiscStatus, ITicks
{
	[Export] public DiscState State { get; set; } = DiscState.Flying;
	[Export] public float Speed { get; set; } = 100;
	[Export] public float Direction { get; set; } = 0;
	[Export] public float Curve { get; set; } = 1;
	public int? ThrowingPlayer { get; set; } = null;

	public void Tick(float delta)
	{
		Visible = true;
		switch (State)
		{
			case DiscState.Flying:
				_ProcessFlyingMovement(delta);
				break;
			case DiscState.Hammer:
				break;
			case DiscState.Stopped:
				break;
			case DiscState.Caught:
				Visible = false;
				break;
			case DiscState.Custom:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private void _ProcessFlyingMovement(float delta)
	{
		//First rotate
		Direction += Curve * delta;
		Direction = MathHelpers.CenterAngle(Direction);

		//Now move in that direction
		var movement = Vector2.Right.Rotated(Direction) * Speed * delta;
		var transform2D = Transform;
		transform2D.origin += movement;
		Transform = transform2D;
	}

	public void OnDiscAreaEntered(Area2D other)
	{
		switch (other)
		{
			case Wall wall:
				//Bounce
				Direction = -Direction;
				break;
		}
	}
}