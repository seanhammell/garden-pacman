extends CharacterBody2D

signal death
signal win

@export var speed: float = 20000.0
var normal_speed: float
var powerup: bool = false
var powerup_timer: float = 0.0
@export var powerup_duration: float = 7.0

var kill_area: Area2D
@export var ray_length: int = 15
var top_right_cast: RayCast2D
var top_left_cast: RayCast2D
var bottom_right_cast: RayCast2D
var bottom_left_cast: RayCast2D
var right_top_cast: RayCast2D
var right_bottom_cast: RayCast2D
var left_top_cast: RayCast2D
var left_bottom_cast: RayCast2D

var previous_position: Vector2
var game_manager
var animated_sprite: AnimatedSprite2D

func _ready():
	reset()
	game_manager = get_node("/root/Scenario")
	kill_area = $KillArea
	kill_area.body_entered.connect(_on_body_entered)
	
	top_right_cast = $topRightCast
	top_left_cast = $topLeftCast
	bottom_right_cast = $bottomRightCast
	bottom_left_cast = $bottomLeftCast
	right_top_cast = $rightTopCast
	right_bottom_cast = $rightBottomCast
	left_top_cast = $leftTopCast
	left_bottom_cast = $leftBottomCast
	
	top_right_cast.target_position = Vector2(0, ray_length)
	top_left_cast.target_position = Vector2(0, ray_length)
	bottom_right_cast.target_position = Vector2(0, ray_length)
	bottom_left_cast.target_position = Vector2(0, ray_length)
	right_top_cast.target_position = Vector2(0, ray_length)
	right_bottom_cast.target_position = Vector2(0, ray_length)
	left_top_cast.target_position = Vector2(0, ray_length)
	left_bottom_cast.target_position = Vector2(0, ray_length)
	
	animated_sprite = $AnimatedSprite2D

func reset():
	position = Vector2(655, 703)
	previous_position = position
	normal_speed = speed
	velocity = Vector2.ZERO

func GetPowerup():
	speed *= 1.5
	powerup = true
	powerup_timer = 0.0
	print("powerup")
	print(powerup)

func _process(delta):
	if powerup:
		powerup_timer += delta
		if powerup_timer >= powerup_duration:
			speed = normal_speed
			powerup = false

func _on_body_entered(body):
	if body.has_method("enemy_die"):
		if powerup:
			body.call("enemy_die")
			game_manager.on_player_win()

func player_die():
	game_manager.on_player_death()
	if game_manager.game_over:
		print("Player Died")
		queue_free()

func HasPowerup() -> bool:
	return powerup

func _physics_process(delta):
	var direction = Input.get_vector("ui_left", "ui_right", "ui_up", "ui_down")
	var new_velocity = velocity

	if direction.x != 0:
		if right_top_cast.is_colliding() and right_bottom_cast.is_colliding() and direction.x > 0:
			direction.x = 0
		if left_top_cast.is_colliding() and left_bottom_cast.is_colliding() and direction.x < 0:
			direction.x = 0
		new_velocity.x = direction.x * speed * delta
	else:
		new_velocity.x = move_toward(velocity.x, 0, delta * speed)

	if direction.y != 0:
		if top_right_cast.is_colliding() and top_left_cast.is_colliding() and direction.y < 0:
			direction.y = 0
		if bottom_right_cast.is_colliding() and bottom_left_cast.is_colliding() and direction.y > 0:
			direction.y = 0
		new_velocity.y = direction.y * speed * delta
	else:
		new_velocity.y = move_toward(velocity.y, 0, delta * speed)

	velocity = new_velocity
	move_and_slide()

	if velocity.x > 0:
		animated_sprite.play("right")
	elif velocity.x < 0:
		animated_sprite.play("left")
	elif velocity.y > 0:
		animated_sprite.play("down")
	elif velocity.y < 0:
		animated_sprite.play("up")

	var footstep_timer = $FootstepSound/Timer
	if (position - previous_position).length() > 1.0 and footstep_timer.is_stopped():
		$FootstepSound.play()
		footstep_timer.start()

	previous_position = position
