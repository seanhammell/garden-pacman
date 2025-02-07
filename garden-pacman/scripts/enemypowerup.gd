extends Area2D

# Get enemy node
var enemy : CharacterBody2D

var is_respawning = false
var timer : float

# Called when the node enters the scene tree for the first time.
func _ready():
	enemy = get_node("/root/Scenario/Enemy") as CharacterBody2D
	timer = 100.0

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float):
	if not visible and not is_respawning:
		timer += delta
		if timer >= float(enemy.get("PowerupDuration")):
			# Stop the chase & respawn the powerup
			show()

func respawn():
	# Respawn the powerup at a random valid place
	hide()
	var random = RandomNumberGenerator.new()
	var new_pos_x : float = random.randi_range(50, 1260)
	var new_pos_y : float = random.randi_range(430, 800)
	var valid_position = false
	
	# Get the parent node containing all items
	var items = get_node("/root/Scenario/Items")

	while not valid_position:
		new_pos_x = random.randi_range(50, 1260)
		new_pos_y = random.randi_range(430, 800)
		valid_position = true # Assume position is valid unless we find a conflict
		
		# Ensure it doesn't spawn in the greenhouse
		if (224 < new_pos_x and new_pos_x < 288) and (-224 < new_pos_y and new_pos_y < 288): # NEED TO UPDATE GREENHOUSE COORDS
			valid_position = false
			continue
	
	position = Vector2(new_pos_x, new_pos_y)
	is_respawning = false

func _on_Body_entered(body: Node2D):
	if body.name != 'Player' and body.name != 'Enemy': # Make sure that the powerup doesn't respawn on top of something
		is_respawning = true
		hide()
		respawn()
	
	# If the enemy eats it and it has been the proper amount of time since the last powerup
	if body.name == 'Enemy':
		# Hide powerup for timer duration
		is_respawning = true
		hide()
		respawn()
		enemy.call("powerup")
		start_timer()

func start_timer():
	timer = 0.0
