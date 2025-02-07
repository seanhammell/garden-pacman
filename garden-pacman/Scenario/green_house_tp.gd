extends Area2D
var teleport_location = Vector2(655, 640)

func _on_body_entered(body: Node2D) -> void:
	if (body.name == 'Enemy'):
		body.global_position = teleport_location
