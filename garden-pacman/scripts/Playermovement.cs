using Godot;
using System;

public partial class Playermovement : CharacterBody2D
{
	[Export]
	public float Speed = 10000.0f;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		
		velocity.X = direction.X * Speed * (float)delta;
		velocity.Y = direction.Y * Speed * (float)delta;
		
		Velocity = velocity;
		MoveAndSlide();
	}
}
