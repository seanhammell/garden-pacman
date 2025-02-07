using Godot;
using System;

public partial class Greenhouse : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CharacterBody2D player = GetNode<CharacterBody2D>("/root/Scenario/Player");
		player.SetDeferred("disabled", true);  // Disable collisions
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
