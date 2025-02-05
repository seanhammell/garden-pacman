extends Area2D
var teleport_location = Vector2(764, 164)

func _on_body_entered(body: Node2D) -> void:
	if (body.name == 'Enemy') or (body.name == 'Player'):
		body.global_position = teleport_location
