extends Control

# Called when the node enters the scene tree for the first time.
func _ready():
	var timer = Timer.new()
	timer.autostart = true
	timer.wait_time = 5.0
	add_child(timer)
	timer.connect("timeout", self, "_on_timer_timeout")

# Timer timeout handler
func _on_timer_timeout():
	get_tree().change_scene("res://Scenario/TitleScreen.tscn")
