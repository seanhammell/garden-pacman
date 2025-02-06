using Godot;
using System;
using System.Collections.Generic;


public partial class Enemymovement : CharacterBody2D
{
	[Export]
	public float Speed { get; set; }
	private float normalSpeed = 100f;
	private float chaseSpeed = 200f;
	
	public bool hasPowerUp { get; set; } = false;
	
	private AnimatedSprite2D sprite;
	
	private int direction; // Direction the enemy is facing; right=0, up=1; left=2; down=3
	
	private Area2D topCollider;
	private Area2D bottomCollider;
	private Area2D leftCollider;
	private Area2D rightCollider;
	private Area2D[] colliders;
	
	// Booleans to hold the previous state of the colliders (whether they're touching the wall or not)
	private bool previousRightCollision;
	private bool previousLeftCollision;
	private bool previousBottomCollision;
	private bool previousTopCollision;
	
	// To detect and time event changes for the colliders
	private bool eventOccurred;
	private double eventTimer;
	
	// Powerup timer
	private double powerupTimer;
	public double PowerupDuration { get; set; } = 10.0;
	
	// Player reference
	private CharacterBody2D player;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Speed = normalSpeed;
		// Get the sprite to change animations
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		// Get the player reference
		player = GetNode<CharacterBody2D>("/root/Scenario/Player");
		
		// Get the colliders
		topCollider = GetNode<Area2D>("TopCollider");
		bottomCollider = GetNode<Area2D>("BottomCollider");
		leftCollider = GetNode<Area2D>("LeftCollider");
		rightCollider = GetNode<Area2D>("RightCollider");
		colliders = new Area2D[] {rightCollider, topCollider, leftCollider, bottomCollider};
		
		// Set the previous state of the colliders
		previousRightCollision = rightCollider.GetOverlappingBodies().Count > 0;
		previousLeftCollision = leftCollider.GetOverlappingBodies().Count > 0;
		previousTopCollision = topCollider.GetOverlappingBodies().Count > 0;
		previousBottomCollision = bottomCollider.GetOverlappingBodies().Count > 0;
		
		// Initialize variables (direction and the event timer)
		changeDirection(0);
		eventTimer = 0.0;
		powerupTimer = 0.0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// The minimum time before the enemy can change directions again
		double minTime = 0.3f;
		
		// If event occurred, wait for a bit before testing event occurred again
		if (eventTimer > minTime) { // If the timer is up, allow events to occur again
			eventOccurred = false;
		} else if (eventOccurred) { // If an event occurred and the timer hasn't gone off, prevent enemy from changing direction
			eventTimer += delta; // Add time to the timer
			
			// Set the previous states of the colliders so they don't think they have changed states
			previousRightCollision = isWall(0); 
			previousTopCollision = isWall(1);
			previousLeftCollision = isWall(2);
			previousBottomCollision = isWall(3);
		}
		
		// Check to see if something changed to cause us to change direction
		if (!eventOccurred) {
			// If a collider was previously detecting a wall and it now isn't (or vice versa), indicate that an event occurred
			eventOccurred = detectChange(previousRightCollision, 0) || detectChange(previousTopCollision, 1) || detectChange(previousLeftCollision, 2) || detectChange(previousBottomCollision, 3);
			eventTimer = 0; // Reset the timer
		}
		
		if (hasPowerUp) {
			powerupTimer += delta;
			Speed = chaseSpeed;
			if (powerupTimer >= PowerupDuration) {
				Speed = normalSpeed;
				hasPowerUp = false;
				changeDirection(direction);
				powerupTimer = 0.0;
				GetNode<Audio>("../Audio").EnemyPowerDown();
			}
		}
		
		// Find the best direction to go
		if (eventOccurred && eventTimer == 0 || isWall(direction)) {
			// Check surroundings for walls and determine all possible directions of movement
			List<int> validMoves = findValidMoves();
			Random random = new Random(); // Initialize a random generator
			
			if (hasPowerUp) { // If the enemy has the powerup, chase the player
				List<int> bestMoves = findPlayer(validMoves);
				changeDirection(bestMoves.Count > 0 ? bestMoves[random.Next(bestMoves.Count)] : validMoves[random.Next(validMoves.Count)]);
			} else { // Otherwise, randomly move
				changeDirection(validMoves[random.Next(validMoves.Count)]);
			}
		}

		// Move the sprite
		move();
	}
	
	private bool detectChange(bool previousColliderState, int direction) {
		// Detect whether the enemy hit a wall or found a new path
		if (previousColliderState != isWall(direction)) {
			previousColliderState = !previousColliderState;
			return true;
		}
		return false;
	}
	
	private void OnBodyEntered(Node2D body) {
		if (body == player) {
			
			if (hasPowerUp) {
				player.Call("playerDie");
				
			}
		}
	}
	
	public void enemyDie()
	{
		QueueFree();
	}
	
	public void Powerup() {
		if (!hasPowerUp) {
			GetNode<Audio>("../Audio").EnemyPowerUp();
		}
		
		// Collect or delete powerup
		changeDirection(direction);
		hasPowerUp = true;
		powerupTimer = 0.0;
	}
	
	private void changeDirection(int dir) {
		direction = dir;
		
		switch (dir) {
			case 0:
				if (hasPowerUp) {
					sprite.FlipH = true;
					sprite.Play("forward");
				} else {
					sprite.FlipH = false;
					sprite.Play("right_harmless");
				}
				break;
			case 1:
				if (hasPowerUp) {
					sprite.Play("backward");
				} else {
					sprite.Play("up_harmless");
				}
				break;
			case 2:
				if (hasPowerUp) {
					sprite.FlipH = false;
					sprite.Play("forward");
				} else {
					sprite.FlipH = false;
					sprite.Play("left_harmless");
				}
				break;
			case 3:
				if (!hasPowerUp) {
					sprite.Play("down_harmless");
				}
				break;
			default:
				break;
		}
	}
	
	private List<int> findValidMoves() {
		// Returns a list of all possible directions to go
		List<int> directions = new List<int>();
		
		List<int> oppositeDirection = new List<int>();

		// Go through every direction and determine if that direction is open or a wall
		for (var i = 0; i < 4; i++) {
			if (Math.Abs(direction-i) == 2) { 
				oppositeDirection.Add(i); // Detect which direction is backwards and don't include it in valid moves
				continue;
			}
			if (!isWall(i)) {
				// If there isn't a wall in that direction, mark it as valid
				directions.Add(i);
			}
		}
		
		// Return the list of valid moves, otherwise tell it to go in the opposite direction
		return directions.Count > 0 ? directions : oppositeDirection;
	}
	
	private bool isWall(int dir) {
		// Returns whether the specified collider detects a wall
		return colliders[dir].GetOverlappingBodies().Count > 0;
	}
	
	private List<int> findPlayer(List<int> validMoves) {
		// Returns the best direction to go to find the player
		List<int> bestDirections = new List<int>();
		
		// Find the difference between the player and enemy positions
		Vector2 posDifference = player.Position - Position;
		
		// Determine if the player is to the right or left
		if (posDifference[0] > 10) {
			if (validMoves.Contains(0) || Math.Abs(direction-0) == 2) { // if right is a valid move or it's the opposite direction
				bestDirections.Add(0);
			}
		} else if (posDifference[0] < -10) {
			if (validMoves.Contains(2) || Math.Abs(direction-0) == 2) { // if left is a valid move or it's the opposite direction
				bestDirections.Add(2);
			}
		}
	
		
		// Determine if the player is up or down
		if (posDifference[1] > 10) {
			if (validMoves.Contains(3) || Math.Abs(direction-0) == 2) { // if down is a valid move or it's the opposite direction
				bestDirections.Add(3);
			}
		} else if (posDifference[1] < -10) {
			if (validMoves.Contains(1) || Math.Abs(direction-0) == 2) { // if up is a valid move or it's the opposite direction
				bestDirections.Add(1);
			}
		}
		
		return bestDirections;
	}
	
	private int findEdible(List<int> validMoves) {
		// Returns the best direction to go to find the next edible
		return 0;
	}
	
	private void move() {
		// Move the enemy
		// Right and down are positive, left and up are negative 
		Position += new Vector2((direction-1)*Speed*(direction%2-1)*(float)GetProcessDeltaTime(), (direction-2)*Speed*(direction%2)*(float)GetProcessDeltaTime()); // (x, y)
	}
}
