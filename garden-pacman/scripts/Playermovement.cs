using Godot;
using System;

public partial class Playermovement : CharacterBody2D
{
	[Export]
	public float Speed = 10000.0f;
	private float NormalSpeed;
	private bool Powerup = false;
	private Timer PowerupTimer;
	private Area2D KillArea;
	[Export]
	public int rayLength = 15;
	private RayCast2D topRightCast;
	private RayCast2D topLeftCast;
	private RayCast2D bottomRightCast;
	private RayCast2D bottomLeftCast;
	private RayCast2D rightTopCast;
	private RayCast2D rightBottomCast;
	private RayCast2D leftTopCast;
	private RayCast2D leftBottomCast;

	public void Reset()
	{
		Position = new Vector2(-1, 75);
		NormalSpeed = Speed;
		Velocity = new Vector2(0, 0);
	}

	public override void _Ready()
	{
		base._Ready();
		Reset();
		PowerupTimer = new Timer();
		PowerupTimer.WaitTime = 5.0f;
		PowerupTimer.OneShot = true;
		PowerupTimer.Timeout += OnPowerupTimeout;
		AddChild(PowerupTimer);
		KillArea = GetNode<Area2D>("KillArea");
		KillArea.BodyEntered += OnBodyEntered;
		topRightCast = GetNode<RayCast2D>("topRightCast");
		topLeftCast = GetNode<RayCast2D>("topLeftCast");
		bottomRightCast = GetNode<RayCast2D>("bottomRightCast");
		bottomLeftCast = GetNode<RayCast2D>("bottomLeftCast");
		rightTopCast = GetNode<RayCast2D>("rightTopCast");
		rightBottomCast = GetNode<RayCast2D>("rightBottomCast");
		leftTopCast = GetNode<RayCast2D>("leftTopCast");
		leftBottomCast = GetNode<RayCast2D>("leftBottomCast");

		topRightCast.TargetPosition = new Vector2(0, rayLength);
		topLeftCast.TargetPosition = new Vector2(0, rayLength);
		bottomRightCast.TargetPosition = new Vector2(0, rayLength);
		bottomLeftCast.TargetPosition = new Vector2(0, rayLength);
		rightTopCast.TargetPosition = new Vector2(0, rayLength);
		rightBottomCast.TargetPosition = new Vector2(0, rayLength);
		leftTopCast.TargetPosition = new Vector2(0, rayLength);
		leftBottomCast.TargetPosition = new Vector2(0, rayLength);
		GD.Print(topRightCast.TargetPosition);
		GD.Print(leftTopCast.TargetPosition);
		GD.Print(rightTopCast.TargetPosition);
		GD.Print(leftBottomCast.TargetPosition);


	}
	public void GetPowerup()
	{
		Speed *= 1.5f;
		Powerup = true;
		PowerupTimer.Start();
		GD.Print("powerup");
		GD.Print(Powerup);
	}

	private void OnPowerupTimeout()
	{
		Speed = NormalSpeed;
		Powerup = false;
	}
	
		private void OnBodyEntered(Node body)
	{
		if (body.HasMethod("die"))
		{
			if (Powerup == true)
				{
				body.Call("die");
				}
		}
	}
	
	public override void _PhysicsProcess(double delta)
	{
		GD.Print(Position);
		Vector2 velocity = Velocity;
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction.X!=0)
		{
			
			
			if (rightTopCast.IsColliding() || rightBottomCast.IsColliding())
			{
				if (direction.X > 0)
				{

					direction.X = 0;
				}
			}
			if (leftTopCast.IsColliding() || leftBottomCast.IsColliding())
			{
				if (direction.X < 0)
				{
					
					direction.X = 0;
				}
			}
			velocity.X = direction.X * Speed * (float)delta;
			
		}
		else
		{
			velocity.X = (float)Mathf.MoveToward(Velocity.X, Velocity.X, delta * Speed);
		}

		if(direction.Y!=0)
		{
			
			if (topRightCast.IsColliding() || topLeftCast.IsColliding())
			{
				if (direction.Y < 0)
				{
					direction.Y = 0;
				}
			}
			if (bottomRightCast.IsColliding() || bottomLeftCast.IsColliding())
			{
				if (direction.Y > 0)
				{
					direction.Y = 0;
				}
			}
			velocity.Y = direction.Y * Speed * (float)delta;

		}
		else
		{
			velocity.Y = (float)Mathf.MoveToward(Velocity.Y, Velocity.Y, delta * Speed);
		}


		Velocity = velocity;
		MoveAndSlide();
	}

}
