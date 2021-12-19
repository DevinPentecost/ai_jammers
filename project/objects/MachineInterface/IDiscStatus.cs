using Godot;

namespace GodotProject.objects.MachineInterface
{
	public enum DiscState
	{
		Flying,
		Hammer,
		Stopped,
		Caught,
		Custom
	}

	public interface IDiscStatus
	{
		float Speed { get; set; } // Units per second
		float Direction { get; set; } // Radians of current direction
		float Curve { get; set; } // Radians per second the disc rotates
		DiscState State { get; set; } // Current state of the disc
		Vector2 Position { get; set; } // Current game location of the disc
	}
}