extends Sprite
tool

onready var shape = $"../CollisionShape2D"
export(Color) var color = Color.wheat

func _process(delta):
	update()

func _draw():
	var radius = shape.shape.radius
	draw_circle(Vector2.ZERO, radius, color)
