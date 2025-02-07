extends CharacterBody2D

@export var speed: float = 100.0
var normal_speed: float = 100.0
var chase_speed: float = 200.0

var has_power_up: bool = false

var sprite: AnimatedSprite2D

var direction: int # 0 = right, 1 = up, 2 = left, 3 = down

var top_collider: Area2D
var bottom_collider: Area2D
var left_collider: Area2D
var right_collider: Area2D
var colliders: Array

var previous_right_collision: bool
var previous_left_collision: bool
var previous_bottom_collision: bool
var previous_top_collision: bool

var event_occurred: bool = false
var event_timer: float = 0.0

var powerup_timer: float = 0.0
@export var PowerupDuration: float = 7.0

var player: CharacterBody2D

func _ready():
	speed = normal_speed
	sprite = $AnimatedSprite2D
	
	player = get_node("/root/Scenario/Player")
	
	top_collider = $TopCollider
	bottom_collider = $BottomCollider
	left_collider = $LeftCollider
	right_collider = $RightCollider
	colliders = [right_collider, top_collider, left_collider, bottom_collider]

	previous_right_collision = right_collider.get_overlapping_bodies().size() > 0
	previous_left_collision = left_collider.get_overlapping_bodies().size() > 0
	previous_top_collision = top_collider.get_overlapping_bodies().size() > 0
	previous_bottom_collision = bottom_collider.get_overlapping_bodies().size() > 0

	change_direction(0)
	event_timer = 0.0
	powerup_timer = 0.0
	
	#has_power_up = true

func _process(delta):
	var min_time = 0.3
	
	if event_timer > min_time:
		event_occurred = false
	elif event_occurred:
		event_timer += delta
		previous_right_collision = is_wall(0)
		previous_top_collision = is_wall(1)
		previous_left_collision = is_wall(2)
		previous_bottom_collision = is_wall(3)
	
	if not event_occurred:
		event_occurred = detect_change(previous_right_collision, 0) or \
						 detect_change(previous_top_collision, 1) or \
						 detect_change(previous_left_collision, 2) or \
						 detect_change(previous_bottom_collision, 3)
		event_timer = 0
	
	if has_power_up:
		powerup_timer += delta
		speed = chase_speed
		change_direction(direction)
		if powerup_timer >= PowerupDuration:
			speed = normal_speed
			has_power_up = false
			powerup_timer = 0.0
			get_node("/root/Scenario/Audio").enemy_power_down()
	
	if (event_occurred and event_timer == 0 or is_wall(direction)):
		var valid_moves = find_valid_moves()
		var best_moves = find_player(valid_moves)
		
		if has_power_up:
			change_direction(best_moves[0])
		elif player.call("HasPowerup"):
			change_direction(best_moves[1])
		else:
			change_direction(valid_moves[randi() % valid_moves.size()])

	move()

func detect_change(previous_state, dir):
	if previous_state != is_wall(dir):
		return true
	return false

func _on_body_entered(body):
	#print("pp")
	if body.name == 'Player':
		#print("hsf")
		if has_power_up:
			player.call("playerDie")

func enemy_die():
	queue_free()

func powerup():
	if not has_power_up:
		get_node("/root/Scenario/Audio").enemy_power_up()
	
	change_direction(direction)
	has_power_up = true
	powerup_timer = 0.0

func change_direction(dir):
	direction = dir
	match dir:
		0:
			sprite.flip_h = has_power_up
			sprite.play("forward" if has_power_up else "right_harmless")
		1:
			sprite.play("backward" if has_power_up else "up_harmless")
		2:
			sprite.flip_h = false
			sprite.play("forward" if has_power_up else "left_harmless")
		3:
			if not has_power_up:
				sprite.play("down_harmless")

func find_valid_moves():
	var directions = []
	var opposite_direction = []

	for i in range(4):
		if abs(direction - i) == 2 and (not has_power_up):
			opposite_direction.append(i)
			continue
		if not is_wall(i):
			directions.append(i)

	return directions if directions.size() > 0 else opposite_direction

func is_wall(dir):
	return colliders[dir].get_overlapping_bodies().size() > 0

func find_player(valid_moves):
	var best_directions = []
	var best_direction = direction if valid_moves.size() == 0 else valid_moves[randi() % valid_moves.size()]
	var run_direction = direction if valid_moves.size() == 0 else valid_moves[randi() % valid_moves.size()]
	
	var pos_diff = player.position - position
	
	if pos_diff.y > 0 and 3 in valid_moves:
		best_direction = 3
	elif pos_diff.y < 0 and 1 in valid_moves:
		best_direction = 1

	if pos_diff.y > 0 and 1 in valid_moves:
		run_direction = 1
	elif pos_diff.y < 0 and 3 in valid_moves:
		run_direction = 3
	
	if abs(pos_diff.x) > abs(pos_diff.y):
		if pos_diff.x > 0 and 0 in valid_moves:
			best_direction = 0
		elif pos_diff.x < 0 and 2 in valid_moves:
			best_direction = 2
	else:
		if pos_diff.x > 0 and 2 in valid_moves:
			run_direction = 2
		elif pos_diff.x < 0 and 0 in valid_moves:
			run_direction = 0
	
	return [best_direction, run_direction]

func move():
	position += Vector2((direction - 1) * speed * (direction % 2 - 1) * get_process_delta_time(),
						(direction - 2) * speed * (direction % 2) * get_process_delta_time())
