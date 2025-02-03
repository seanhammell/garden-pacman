using Godot;
using System;

public partial class EnemyPowerup : Area2D
{
	// Get enemy node
	private CharacterBody2D enemy;
	
	private double timer;
	private int timerDuration;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		enemy = GetNode<CharacterBody2D>("/root/Scenario/Enemy");
		timerDuration = 30;// Duration of time enemy will have powerup
		timer = 0.0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!this.Visible) {
			timer += delta;
			if (timer >= timerDuration) {
				// Stop the chase
				enemy.Call("TogglePowerup");
				// move piece to random VALID position
				Show();
			}
		}
	}
	
	private void OnBodyEntered(Node2D body)
	{
		if (body == enemy) {
			// Hide powerup for timer duration
			Hide();
			enemy.Call("TogglePowerup");
			startTimer();
		}
	}
	
	private void startTimer() {
		timer = 0;
	}
}
