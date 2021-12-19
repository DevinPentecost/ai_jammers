using Godot;
using System;
using GodotProject.objects.MachineInterface;

public class Disc : Area2D, IDiscStatus
{
[Export]
	public DiscState State { get; set; } = DiscState.Flying;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	public override void _PhysicsProcess(float delta)
	{

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
		
		//Now move in that direction
		var movement = Vector2.Right.Rotated(Direction) * Speed * delta;
		var transform2D = Transform;
		transform2D.origin += movement;
		Transform = transform2D;
	}

	[Export]
	public float Speed { get; set; } = 100;
	[Export]
	public float Direction { get; set; } = 0;
	[Export]
	public float Curve { get; set; } = 1;
}
