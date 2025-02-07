extends CanvasLayer

# Update score label
func update_score(score: int):
	get_node("ScoreLabel").text = "Score: " + str(score)

# Update lives label
func update_lives(lives: int):
	get_node("LivesLabel").text = "Lives: " + str(lives)

# Called when the node enters the scene tree for the first time.
func _ready():
	pass

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float):
	pass
