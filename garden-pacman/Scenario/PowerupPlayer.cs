using Godot;
using System;

public partial class PowerupPlayer : Area2D
{
	// Get enemy node
	private CharacterBody2D player;
	
	private bool isRespawning = false;
	private double timer; 
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode<CharacterBody2D>("/root/Scenario/Player");
		timer = 100.0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!this.Visible && !isRespawning) {
			timer += delta;
			if (timer >= (int)player.Get("PowerupDuration")) {
				// Stop the chase & respawn the thang
				Show();
			}
		}
	}
	
	private void respawn() {
		// Respawn the powerup at a random valid place
		Hide();
		Random random = new Random();
		float newPosX = random.Next(45, 1500);
		float newPosY = random.Next(45, 1330);
		bool validPosition = false;
		
		// Get the parent node containing all items
		Node items = GetNode<Node>("/root/Scenario/Items");

		while (!validPosition) {
			newPosX = random.Next(32, 480);
			newPosY = random.Next(32, 480);
			validPosition = true; // Assume position is valid unless we find a conflict

			// Ensure it doesn't spawn in the greenhouse
			if ((224 < newPosX && newPosX < 288) && (-224 < newPosY && newPosY < 288)) { // NEED TO UPDATE GREENHOUSE COORDS
				validPosition = false;
				continue;
			}
			
		}

		Position = new Vector2(newPosX, newPosY);
		isRespawning = false;
	}

	
	private void OnBodyEntered(Node2D body)
	{
		if (body != GetNode<CharacterBody2D>("/root/Scenario/Enemy") && body != player) { // Make sure that the powerup doesn't respawn on top of something
			isRespawning = true;
			Hide();
			respawn();
		}
		
		if (body == player) {
			// Hide powerup for timer duration
			GD.Print("Powerup collected! - Called from powerup object");
			isRespawning = true;
			Hide();
			respawn();
			player.Call("GetPowerup");
			startTimer();
		}
	}
	
	private void startTimer() {
		timer = 0.0;
	}
}
