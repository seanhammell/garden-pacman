using Godot;
using System;

public partial class Audio : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<AudioStreamPlayer>("MusicMain").Play();
	}
	
	public void EnemyPowerUp() {
		GetNode<AudioStreamPlayer>("MusicMain").Stop();
		GetNode<Timer>("Timer").Start();
	}
	
	public void OnTimerTimeout() {
		GetNode<AudioStreamPlayer>("MusicAction").Play();
	}
	
	public void EnemyPowerDown() {
		GetNode<AudioStreamPlayer>("MusicAction").Stop();
		GetNode<AudioStreamPlayer>("MusicMain").Play();
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
