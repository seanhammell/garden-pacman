extends Area2D
var teleport_location = Vector2(480, 192)

func _on_body_entered(body: Node2D) -> void:
	if (body.name == 'Enemy') or (body.name == 'Player'):
		body.global_position = teleport_location
