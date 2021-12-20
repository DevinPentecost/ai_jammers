using Godot;

namespace GodotProject.objects.MachineInterface
{
	public enum PlayerState
	{
		Regular,
		Dashing,
		Custom
	}

	public interface IPlayerStatus
	{
		Vector2 Position { get; set; } // Current game position of the player
		int PlayerIndex { get; set; } // The current player ID (0 for left, 1 for right)
		PlayerState State { get; set; } // The current gameplay state of this player
		bool HoldingDisc { get; set; } // Whether we are holding a disc
		int DashStep { get; set; } // Which step of the player dash are they on?
	}

	public interface IPlayerInput
	{
		bool Left { get; set; }
		bool Right { get; set; }
		bool Up { get; set; }
		bool Down { get; set; }
		bool A { get; set; }
		bool B { get; set; }
		void Reset();
	}

	public class PlayerInput : IPlayerInput
	{
		public bool Left { get; set; }
		public bool Right { get; set; }
		public bool Up { get; set; }
		public bool Down { get; set; }
		public bool A { get; set; }
		public bool B { get; set; }

		public void Reset()
		{
			Left = default;
			Right = default;
			Up = default;
			Down = default;
			A = default;
			B = default;
		}
	}

	public class Dash
	{
		public readonly float[] SpeedSteps;

		public Dash()
		{
			SpeedSteps = new[] { 3, 3, 3, 2.5f, 2, 1.5f, 1 };
		}

		public int DashStep { get; set; } = -1;

		public bool Complete => DashStep < 0 || DashStep >= SpeedSteps.Length;

		public void Stop()
		{
			DashStep = -1;
		}

		public void Start()
		{
			DashStep = 0;
		}
	}
}