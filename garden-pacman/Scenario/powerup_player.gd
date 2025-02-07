extends Area2D
@onready var spawn_timer: Timer = $spawn_timer

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass


func _on_body_entered(body: Node2D) -> void:
	if body.name == "Player":
		body.GetPowerup()
		hide()
		spawn_timer.start()
		


func _on_timer_timeout() -> void:
	show()
	pass # Replace with function body.
