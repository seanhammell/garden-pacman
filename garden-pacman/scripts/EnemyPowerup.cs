using Godot;
using System;

public partial class EnemyPowerup : Area2D
{
	// Get enemy node
	private CharacterBody2D enemy;
	
	private bool isRespawning = true;
	private double timer; 
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		enemy = GetNode<CharacterBody2D>("/root/Scenario/Enemy");
		timer = 100.0;
		respawn();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!this.Visible && !isRespawning) {
			timer += delta;
			if (timer >= (int)enemy.Get("PowerupDuration")) {
				// Stop the chase & respawn the thang
				Show();
			}
		}
	}
	
	private void respawn() {
		// Respawn the powerup at a random valid place
		Random random = new Random();
		float newPosX = random.Next(-250, 250);
		float newPosY = random.Next(-200, 300);
		bool validPosition = false;
		
		// Get the parent node containing all items
		Node items = GetNode<Node>("/root/Scenario/Items");

		while (!validPosition) {
			newPosX = random.Next(-250, 250);
			newPosY = random.Next(-200, 300);
			validPosition = true; // Assume position is valid unless we find a conflict

			// Ensure it doesn't spawn in the greenhouse
			if ((-25 < newPosX && newPosX < 25) && (-100 < newPosY && newPosY < 100)) {
				validPosition = false;
				continue;
			}
			
		}

		Position = new Vector2(newPosX, newPosY);
		isRespawning = false;
	}

	
	private void OnBodyEntered(Node2D body)
	{
		if (body != GetNode<CharacterBody2D>("/root/Scenario/Player") && body != enemy) { // Make sure that the powerup doesn't respawn on top of something
			Hide();
			isRespawning = true;
			respawn();
		}
		
		// If the enemy eats it and it has been the proper amount of time since the last powerup
		if (body == enemy && timer >= (int)enemy.Get("PowerupDuration")) {
			// Hide powerup for timer duration
			Hide();
			isRespawning = true;
			respawn();
			enemy.Call("Powerup");
			startTimer();
		}
	}
	
	private void startTimer() {
		timer = 0.0;
	}
}
