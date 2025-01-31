using Godot;
using System;
using System.Collections.Generic;


public partial class Enemymovement : CharacterBody2D
{
	[Export]
	public float Speed { get; set; } = 0.25f;
	
	private bool hasPowerUp = false;
	
	private AnimatedSprite2D sprite;
	
	private int direction; // Direction the enemy is facing; right=0, up=1; left=2; down=3
	
	private Area2D topCollider;
	private Area2D bottomCollider;
	private Area2D leftCollider;
	private Area2D rightCollider;
	private Area2D[] colliders;
	
	private bool previousRightCollision;
	private bool previousLeftCollision;
	private bool previousBottomCollision;
	private bool previousTopCollision;
	private bool eventOccurred;
	private double eventTimer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Set all the colliders
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		topCollider = GetNode<Area2D>("TopCollider");
		bottomCollider = GetNode<Area2D>("BottomCollider");
		leftCollider = GetNode<Area2D>("LeftCollider");
		rightCollider = GetNode<Area2D>("RightCollider");
		previousRightCollision = rightCollider.GetOverlappingBodies().Count > 0;
		previousLeftCollision = leftCollider.GetOverlappingBodies().Count > 0;
		previousTopCollision = topCollider.GetOverlappingBodies().Count > 0;
		previousBottomCollision = bottomCollider.GetOverlappingBodies().Count > 0;
		colliders = new Area2D[] {rightCollider, topCollider, leftCollider, bottomCollider};
		changeDirection(1);
		eventTimer = 0.0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		double maxTime = 0.3f;
		
		// If event occurred, wait for a bit before testing event occurred again
		if (eventTimer > maxTime) {
			eventOccurred = false;
		} else if (eventOccurred) {
			eventTimer += delta;
			previousRightCollision = isWall(0);
			previousTopCollision = isWall(1);
			previousLeftCollision = isWall(2);
			previousBottomCollision = isWall(3);
		}
		
		// Check to see if something changed to cause us to change direction
		if (!eventOccurred) {
			if (previousRightCollision != isWall(0)) {
				previousRightCollision = !previousRightCollision;
				eventOccurred = true;
				eventTimer = 0;
			}
			if (previousTopCollision != isWall(1)) {
				previousTopCollision = !previousTopCollision;
				eventOccurred = true;
				eventTimer = 0;
			}
			if (previousLeftCollision != isWall(2)) {
				previousLeftCollision = !previousLeftCollision;
				eventOccurred = true;
				eventTimer = 0;
			}
			if (previousBottomCollision != isWall(3)) {
				previousBottomCollision = !previousBottomCollision;
				eventOccurred = true;
				eventTimer = 0;
			}
		}
		
		
		// Find the best direction to go
		if (eventOccurred && eventTimer == 0 || isWall(direction)) {
			// Check surroundings for walls and determine all possible directions of movement
			List<int> validMoves = findValidMoves();
			Random random = new Random();
			changeDirection(validMoves[random.Next(validMoves.Count)]);
		}
		
		// Change random to hasPowerUp ? findPlayer(validMoves) : findEdible(validMoves)

		// Move the sprite
		move();
	}
	
	private void changeDirection(int dir) {
		direction = dir;
		
		switch (dir) {
			case 0:
				sprite.Play("right");
				break;
			case 1:
				sprite.Play("up");
				break;
			case 2:
				sprite.Play("left");
				break;
			case 3:
				sprite.Play("down");
				break;
			default:
				break;
		}
	}
	
	private List<int> findValidMoves() {
		// Returns a list of all possible directions to go
		List<int> directions = new List<int>();
		
		List<int> oppositeDirection = new List<int>();

		// Find walls
		for (var i = 0; i < 4; i++) {
			if (Math.Abs(direction-i) == 2) { // don't go backward
				oppositeDirection.Add(i);
				continue;
			}
			if (!isWall(i)) {
				directions.Add(i);
			}
		}
		
		return directions.Count > 0 ? directions : oppositeDirection;
	}
	
	private bool isWall(int dir) {
		return colliders[dir].GetOverlappingBodies().Count > 0;
	}
	
	private int findPlayer(List<int> validMoves) {
		// Returns the best direction to go to find the player
		return 0;
	}
	
	private int findEdible(List<int> validMoves) {
		// Returns the best direction to go to find the next edible
		return 0;
	}
	
	private void move() {
		// Right and down are positive, left and up are negative 
		Position += new Vector2((direction-1)*Speed*(direction%2-1), (direction-2)*Speed*(direction%2)); // (x, y)
	}
}
