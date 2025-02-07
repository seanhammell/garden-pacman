using Godot;
using System;

public partial class GameOverScreen : Control
{
	public override void _Ready()
	{
		var timer = new Timer();
		timer.Autostart = true;
		timer.WaitTime = 5.0;
		AddChild(timer);
		timer.Timeout += () => GetTree().ChangeSceneToFile("res://Scenario/TitleScreen.tscn");
	}
}
