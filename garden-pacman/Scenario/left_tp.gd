extends Area2D
var teleport_location = Vector2(226, 4)

func _on_body_entered(body: Node2D) -> void:
	if (body.name == 'CharacterBody2D') or (body.name == 'Player'):
		body.global_position = teleport_location
