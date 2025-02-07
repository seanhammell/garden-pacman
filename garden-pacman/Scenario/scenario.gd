extends Node2D

var _score: int
var _player_lives: int = 4
var hud

func new_game():
	get_node("ScoreTimer").start()
	get_node("PelletsTimer").start()
	_score = 10000
	
	hud.update_score(_score)
	
	var player = get_node("Player") as CharacterBody2D
	player.reset()

func game_over(player_won: bool):
	if player_won:
		print("Player Won!")
	else:
		print("Player Lost!")
	get_tree().change_scene_to_file("res://Scenario/GameOverScreen.tscn")

func _on_score_timer_timeout():
	_score -= 1
	hud.update_score(_score)
	if _score == 0:
		game_over(false)

#func _on_pellets_timer_timeout():
#	var player_node = get_node("Player") as CharacterBody2D
#	var pellet_scene = load("res://Scenario/pellets.tscn") as PackedScene
#	var pellet_instance = pellet_scene.instantiate() as Area2D
#	pellet_instance.global_position = player_node.global_position
#	get_tree().current_scene.add_child(pellet_instance)

func on_player_death():
	_player_lives -= 1
	if _player_lives > 0:
		new_game()
	else:
		game_over(false)
	hud.update_lives(_player_lives)

func on_player_win():
	game_over(true)

func _ready():
	hud = get_node("HUD")
	new_game()

func _process(delta):
	if Input.is_action_pressed("reset"):
		new_game()
	var specific_child = get_child(13)
	if specific_child.get_child_count() == 0:
		_score = 0
