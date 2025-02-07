using Godot;
using System;

public partial class Audio : Node
{
	
	private int powerup_type = 0; // 1 = spray, 2 = clippers
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<AudioStreamPlayer>("MusicMain").Play();
	}
	
	public void EnemyPowerUp() {
		GetNode<AudioStreamPlayer>("MusicMain").Stop();
		GetNode<AudioStreamPlayer>("PlantPowerUp").Play();
		GetNode<Timer>("Timer").Start();
	}
	
	public void OnTimerTimeout() {
		GetNode<AudioStreamPlayer>("MusicAction").Play();
	}
	
	public void EnemyPowerDown() {
		GetNode<AudioStreamPlayer>("MusicAction").Stop();
		GetNode<AudioStreamPlayer>("MusicMain").Play();
	}
	
	public void PickUpSpray() {
		powerup_type = 1;
		GetNode<AudioStreamPlayer>("Spray").Play();
	}
	
	public void PickUpClippers() {
		powerup_type = 2;
		GetNode<AudioStreamPlayer>("Clippers").Play();
	}
	
	public void PowerupUsed() {
		switch (powerup_type) {
			case 1: GetNode<AudioStreamPlayer>("Spray").Play(); break;
			case 2: GetNode<AudioStreamPlayer>("Clippers").Play(); break;
			default: break;
		}
	}
	
	public void PowerupDropped() {
		PowerupUsed();
		powerup_type = 0;
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	//public override void _Process(double delta)
	//{
		//
	//}
}
