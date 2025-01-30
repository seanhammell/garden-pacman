using Godot;
using System;
using System.Collections.Generic;


public partial class Enemymovement : Area2D
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
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		topCollider = GetNode<RayCast2D>("TopCollider");
		bottomCollider = GetNode<RayCast2D>("BottomCollider");
		leftCollider = GetNode<RayCast2D>("LeftCollider");
		rightCollider = GetNode<RayCast2D>("RightCollider");
		changeDirection(3);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Check surroundings for walls and determine all possible directions of movement
		List<int> validMoves = findValidMoves();
		
		// Find the best direction to go
		//changeDirection(hasPowerUp ? findPlayer(validMoves) : findEdible(validMoves));

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
		if (!isWallRight()) {
			directions.Add(0);
		}
		if (!isWallUp()) {
			directions.Add(1);
		}
		if (!isWallLeft()) {
			directions.Add(2);
		}
		if (!isWallDown()) {
			directions.Add(3);
		}
		
		return directions;
	}
	
	private bool isWallRight() {
		return rightCollider.IsColliding();
	}
	
	private bool isWallUp() {
		return topCollider.IsColliding();
	}
	
	private bool isWallLeft() {
		return leftCollider.IsColliding();
	}
	
	private bool isWallDown() {
		return bottomCollider.IsColliding();
	}
	
	private int findPlayer(List<int> validMoves) {
		// Returns the best direction to go to find the player
		return 0;
		
	}
	
	private int findEdible(List<int> validMoves) {
		// Returns the best direction to go to find the next edible
		return 1;
	}
	
	private void move() {
		// Right and down are positive, left and up are negative 
		Position += new Vector2((direction-1)*Speed*(direction%2-1), (direction-2)*Speed*(direction%2)); // (x, y)
	}
}
