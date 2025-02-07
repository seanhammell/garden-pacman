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
		if !spawn_timer.is_stopped():
			return
		
		body.GetPowerup()
		
		if self.name == "PowerupPlayerSpray":
			$/root/Scenario/Audio.pick_up_spray()
		else:
			$/root/Scenario/Audio.pick_up_clippers()
		
		hide()
		spawn_timer.start()


func _on_timer_timeout() -> void:
	show()
	pass # Replace with function body.
