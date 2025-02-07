extends Node

# Called when the node enters the scene tree for the first time.
func _ready():
	$MusicMain.play()

func enemy_power_up():
	$MusicMain.stop()
	$Timer.start()

func on_timer_timeout():
	$MusicAction.play()

func enemy_power_down():
	$MusicAction.stop()
	$MusicMain.play()
