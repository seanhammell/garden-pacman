using Godot;
using System;

public partial class EnemyPowerup : Area2D
{
	// Get enemy node
	private CharacterBody2D enemy;
	
	private double timer; 
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		enemy = GetNode<CharacterBody2D>("/root/Scenario/Enemy");
		timer = 0.0;
		respawn();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!this.Visible) {
			timer += delta;
			if (timer >= (int)enemy.Get("PowerupDuration")) {
				// Stop the chase & respawn the thang
				Show();
				timer = 0.0;
			}
		}
	}
	
	private void respawn() {
		// Respawn the powerup at a random valid place
		Random random = new Random();
		float newPosX = random.Next(-250, 250);
		float newPosY = random.Next(-200, 300);
		//figure out actual dimensions
		while ((25 < newPosX  && newPosX < 25) && (50 < newPosY && newPosY < 100)) { // Don't respawn in the greenhouse
			newPosX = random.Next(-250, 250);
			newPosY = random.Next(-200, 300);
		}
		Position = new Vector2(newPosX, newPosY);
	}
	
	private void OnBodyEntered(Node2D body)
	{
		if (body != GetNode<CharacterBody2D>("/root/Scenario/Player") && body != enemy) { // Make sure that the powerup doesn't respawn on top of something
			respawn();
		}
		
		foreach (Node child in GetNode<Node>("/root/Scenario/Items").GetChildren()) { // If it respawns on top of another item, respawn again
			if (child == body)
			{
				respawn();
				break;
			}
		}
		
		// If the enemy eats it and it has been the proper amount of time since the last powerup
		if (body == enemy && timer <= 0.0) {
			// Hide powerup for timer duration
			Hide();
			respawn();
			enemy.Call("Powerup");
			startTimer();
		}
	}
	
	private void startTimer() {
		timer = 0;
	}
}
