using Godot;
using System;

public partial class Scenario : Node2D
{
	private int _score;
	private int _playerLives = 4;
	public bool gameOver = false;
	private Hud hud;

	public void NewGame()
	{
		GetNode<Timer>("ScoreTimer").Start();
		GetNode<Timer>("PelletsTimer").Start();
		_score = 10000;
		
		hud.UpdateScore(_score);
		
		var player = GetNode<Playermovement>("Player");
		player.Reset();
	}

	public bool IsGameOver()
	{
		return gameOver;
	}

	public void GameOver(bool playerWon)
	{
		gameOver = true;
		if(playerWon)
		{
			GD.Print("Player Won!");
		}
		else
		{
			GD.Print("Player Lost!");
		}
		GetTree().ChangeSceneToFile("res://Scenario/GameOverScreen.tscn");
	}
	
	public void OnScoreTimerTimeout()
	{
		--_score;
		hud.UpdateScore(_score);
		if (_score == 0) {
			const bool playerWon = false;
			GameOver(playerWon);
		}
	}

	public void OnPelletsTimerTimeout()
	{
		var PlayerNode = GetNode<CharacterBody2D>("Player");
		 PackedScene pelletScene = (PackedScene)ResourceLoader.Load("res://Scenario/pellets.tscn");
		Area2D pelletInstance = (Area2D)pelletScene.Instantiate();
		pelletInstance.GlobalPosition = PlayerNode.GlobalPosition;
		GetTree().CurrentScene.AddChild(pelletInstance);
	}
	
	public void OnPlayerDeath()
	{
		const bool playerWon = false;
		--_playerLives;
		if(_playerLives>0)
		{
			NewGame();
		}
		else
		{
			GameOver(playerWon);
		}
		hud.UpdateLives(_playerLives);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hud = GetNode<Hud>("HUD");
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
