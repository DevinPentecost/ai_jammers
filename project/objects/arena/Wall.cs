using Godot;

public class Wall : Area2D
{
	private RectangleShape2D Shape => GetNode<CollisionShape2D>("CollisionShape2D").Shape as RectangleShape2D;

	public override void _Ready()
	{
		Shape.Extents = new Vector2(300, Game.WallSize);
	}

	public void _SetTop(Vector2 arenaSize)
	{
		var transform2D = Transform;
		transform2D.origin.x = arenaSize.x / 2;
		transform2D.origin.y = Game.WallSize / 2;
		Transform = transform2D;
		Update();
	}

	public void _SetBottom(Vector2 arenaSize)
	{
		var transform2D = Transform;
		transform2D.origin.x = arenaSize.x / 2;
		transform2D.origin.y = arenaSize.y - Game.WallSize / 2;
		Transform = transform2D;
		Update();
	}

	public override void _Draw()
	{
		// DrawRect(new Rect2(Transform.origin - new Vector2(150, Game.WallSize/2), Shape.Extents), Colors.Azure);
	}
}