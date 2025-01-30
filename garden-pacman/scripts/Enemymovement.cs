using Godot;
using System;

public partial class Enemymovement : Area2D
{
	[Export]
	public float Speed { get; set; } = 0.5f;
	
	private bool hasPowerUp = false;
	
	private int direction = 0; // Direction the enemy is facing; right=0, up=1; left=2; down=3
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Position = new Vector2(300, 350); //Initially place the enemy at a certain spot
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Check surroundings for either a wall or a new path to change direction
		checkSurroundings();
		
		// Move the sprite
		move();
	}
	
	private void checkSurroundings() {
		// Function that checks the surroundings by the enemy to know if there's a wall or a free path to go down
		
		// If the enemy is powered-up, choose to go to the best direction to get to player (without turning around)
		
		// If next grid space is a wall, move in a different direction
		
		// If next grid space is free, randomly choose to continue in the same direction or go down the new path
		
		// Include probability of going to the next powerup
	}
	
	private void changeDirection(string dir) {
		switch (dir) {
			case "right":
				direction = 0;
				break;
			case "left":
				direction = 2;
				break;
			case "up":
				direction = 1;
				break;
			case "down":
				direction = 3;
				break;
			default:
				break;
		}
	}
	
	private void move() {
		// Right and down are positive, left and up are negative 
		Position += new Vector2((direction-1)*Speed*(direction%2-1), (direction-2)*Speed*(direction%2)); // (x, y)
	}
	
	private void moveRight() {
		Position += new Vector2(1*Speed, 0);
	}
	
	private void moveLeft() {
		Position += new Vector2(-1*Speed, 0);
	}
	
	private void moveUp() {
		Position += new Vector2(0, -1*Speed);
	}
	
	private void moveDown() {
		Position += new Vector2(0, 1*Speed);
	}
}
