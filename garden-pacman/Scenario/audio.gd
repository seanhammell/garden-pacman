extends Node

var powerup_type = 0

# Called when the node enters the scene tree for the first time.
func _ready():
	$MusicMain.play()

func enemy_power_up():
	$MusicMain.stop()
	$PlantPowerUp.play()
	$Timer.start()

func on_timer_timeout():
	$MusicAction.play()

func enemy_power_down():
	$MusicAction.stop()
	$MusicMain.play()

func pick_up_spray():
	powerup_type = 1
	$Spray.play()

func pick_up_clippers():
	powerup_type = 2
	$Clippers.play()

func powerup_used():
	if powerup_type == 1:
		$Spray.play()
	elif powerup_type == 2:
		$Clippers.play()

func powerup_dropped():
	powerup_used()
	powerup_type = 0
