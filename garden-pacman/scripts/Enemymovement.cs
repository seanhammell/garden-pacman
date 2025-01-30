using Godot;
using System;
using System.Collections.Generic;


public partial class Enemymovement : CharacterBody2D
{
	[Export]
	public float Speed { get; set; } = 0.5f;
	
	private bool hasPowerUp = false;
	
	private AnimatedSprite2D sprite;
	
	private int direction; // Direction the enemy is facing; right=0, up=1; left=2; down=3
	
	private RayCast2D topCollider;
	private RayCast2D bottomCollider;
	private RayCast2D leftCollider;
	private RayCast2D rightCollider;
	private RayCast2D[] colliders;
	
	private bool previousRightCollision;
	private bool previousLeftCollision;
	private bool previousBottomCollision;
	private bool previousTopCollision;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Set all the colliders
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		topCollider = GetNode<RayCast2D>("TopCollider");
		bottomCollider = GetNode<RayCast2D>("BottomCollider");
		leftCollider = GetNode<RayCast2D>("LeftCollider");
		rightCollider = GetNode<RayCast2D>("RightCollider");
		previousRightCollision = rightCollider.IsColliding();
		previousLeftCollision = leftCollider.IsColliding();
		previousTopCollision = topCollider.IsColliding();
		previousBottomCollision = bottomCollider.IsColliding();
		colliders = new RayCast2D[] {rightCollider, topCollider, leftCollider, bottomCollider};
		changeDirection(1);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Check to see if something changed to cause us to change direction
		bool eventOccurred = false;
		if (previousRightCollision != isWall(0)) {
			previousRightCollision = !previousRightCollision;
			eventOccurred = true;
		}
		if (previousTopCollision != isWall(1)) {
			previousTopCollision = !previousTopCollision;
			eventOccurred = true;
		}
		if (previousLeftCollision != isWall(2)) {
			previousLeftCollision = !previousLeftCollision;
			eventOccurred = true;
		}
		if (previousBottomCollision != isWall(3)) {
			previousBottomCollision = !previousBottomCollision;
			eventOccurred = true;
		}
		
		// maybe take out being able to turn around?
		
		// Find the best direction to go
		if (eventOccurred) {
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

		// Find walls
		for (var i = 0; i < 4; i++) {
			if (!isWall(i)) {
				directions.Add(i);
			}
		}
		
		return directions;
	}
	
	private bool isWall(int dir) {
		return colliders[dir].IsColliding();
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
