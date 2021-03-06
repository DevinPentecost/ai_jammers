using Godot;

public interface ITicks
{
	void Tick(float delta);
}

public class Game : Node2D
{
	public const float WallSize = 10;
	private PackedScene WallScene = ResourceLoader.Load<PackedScene>("res://objects/arena/wall.tscn");

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_InitializeWalls();
	}

	private void _InitializeWalls()
	{
		var arenaSize = GetViewportRect().Size;
		var topWall = WallScene.Instance() as Wall;
		AddChild(topWall);
		topWall?._SetTop(arenaSize);

		var bottomWall = WallScene.Instance() as Wall;
		AddChild(bottomWall);
		bottomWall?._SetBottom(arenaSize);
	}

	public override void _PhysicsProcess(float delta)
	{
		//Get everything that can tick
		var ticks = GetTree().GetNodesInGroup("ticks");
		foreach (var tick in ticks)
		{
			var tickable = tick as ITicks;
			tickable?.Tick(delta);
		}
	}
}