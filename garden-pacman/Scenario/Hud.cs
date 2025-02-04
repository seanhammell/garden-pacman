using Godot;
using System;

public partial class Hud : CanvasLayer
{
	public void UpdateScore(int score)
	{
		GetNode<Label>("ScoreLabel").Text = score.ToString();
	}
	public void UpdateLives(int lives)
	{
		GetNode<Label>("LivesLabel").Text = "Lives: " + lives.ToString();
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
