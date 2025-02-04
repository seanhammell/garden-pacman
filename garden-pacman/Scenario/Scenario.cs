using Godot;
using System;

public partial class Scenario : Node2D
{
	private int _score;
	public void NewGame()
	{
		GetNode<Timer>("ScoreTimer").Start();
		GetNode<Timer>("PelletsTimer").Start();
		_score = 0;
		var hud = GetNode<Hud>("HUD");
		hud.UpdateScore(_score);
		var player = GetNode<Playermovement>("Player");
		player.Reset();
	}
	
	public void OnScoreTimerTimeout()
	{
		++_score;
		GetNode<Hud>("HUD").UpdateScore(_score);
	}
	
		public void OnPelletsTimerTimeout()
	{
		var PlayerNode = GetNode<CharacterBody2D>("Player");
		var PelletsInstance = GetNode<Area2D>("Pellets");
		PelletsInstance.GlobalPosition = PlayerNode.GlobalPosition;
		GetTree().CurrentScene.AddChild(PelletsInstance);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		NewGame();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("reset"))
		{
			NewGame();
		}
	}
}
