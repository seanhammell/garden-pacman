using Godot;
using System;

public partial class Node2d : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Console.WriteLine("hi");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
