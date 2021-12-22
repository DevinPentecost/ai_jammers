using Godot;

public class Wall : Area2D
{
	private bool _isTop;
	public CollisionShape2D Shape => GetNode<CollisionShape2D>("CollisionShape2D");


	public override void _Ready()
	{
		((RectangleShape2D)Shape.Shape).Extents = new Vector2(300, Game.WallSize);
	}

	public void _SetTop(Vector2 arenaSize)
	{
		var transform2D = Transform;
		transform2D.origin.x = arenaSize.x / 2;
		transform2D.origin.y = Game.WallSize / 2;
		Transform = transform2D;
		_isTop = true;
	}

	public void _SetBottom(Vector2 arenaSize)
	{
		var transform2D = Transform;
		transform2D.origin.x = arenaSize.x / 2;
		transform2D.origin.y = arenaSize.y - Game.WallSize / 2;
		Transform = transform2D;
		_isTop = false;
	}

	public void _on_Wall_area_entered(Area2D other)
	{
		if (other is Disc disc)
		{
			disc.Direction += Mathf.Pi;
		}
	}
}