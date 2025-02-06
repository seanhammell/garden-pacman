using Godot;
using System;

public partial class Playermovement : CharacterBody2D
{
	[Signal]
	public delegate void DeathEventHandler();

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
	
	private Vector2 PreviousPosition;
	private double powerupTimer;
	public double PowerupDuration { get; set; } = 10.0;
	private Scenario gameManager;
	private AnimatedSprite2D animatedSprite;

	public void Reset()
	{
		Position = new Vector2(778, 640);
		PreviousPosition = new Vector2(Position.X, Position.Y);
		NormalSpeed = Speed;
		Velocity = new Vector2(0, 0);
		GD.Print("Player Reset");
		GD.Print(Position);
	}

	public override void _Ready()
	{
		base._Ready();
		Reset();
		gameManager = GetNode<Scenario>("/root/Scenario");
		PowerupTimer = new Timer();
		PowerupTimer.WaitTime = 30.0f;
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
		
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");


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
		
		if(body.Name=="Enemy")
		{
			GD.Print("Player Collided enemy");
		}
		if (body.HasMethod("die"))
		{
			GD.Print("Value of powerup: ",Powerup);
			if (Powerup == true)
			{
				body.Call("die");
			}
		}
	}
	
	public void die()
	{
		
		gameManager.OnPlayerDeath();
		if(gameManager.gameOver)
		{
			GD.Print("Player Died");
			QueueFree();
		}
		
	}
	
	public override void _PhysicsProcess(double delta)
	{

		Vector2 velocity = Velocity;
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		
		
		if (direction.X!=0)
		{
			
			
			if (rightTopCast.IsColliding() && rightBottomCast.IsColliding())
			{
				if (direction.X > 0)
				{

					direction.X = 0;
				}
			}
			if (leftTopCast.IsColliding() && leftBottomCast.IsColliding())
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
			if (topRightCast.IsColliding() && topLeftCast.IsColliding())
			{
				if (direction.Y < 0)
				{
					direction.Y = 0;
				}
			}
			if (bottomRightCast.IsColliding() && bottomLeftCast.IsColliding())
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
		if(velocity.X>0)
		{
			animatedSprite.Play("right");
		}
		else if(velocity.X<0)
		{
			animatedSprite.Play("left");
		}
		else if(velocity.Y>0)
		{
			animatedSprite.Play("down");
		}
		else if(velocity.Y<0)
		{
			animatedSprite.Play("up");
		}
		var footstep_timer = GetNode<Timer>("FootstepSound/Timer");
		if ((Position - PreviousPosition).Length() > 1.0 && footstep_timer.IsStopped()) {
			GetNode<AudioStreamPlayer>("FootstepSound").Play();
			footstep_timer.Start();
		}
		
		PreviousPosition = new Vector2(Position.X, Position.Y);
	}
	
}
