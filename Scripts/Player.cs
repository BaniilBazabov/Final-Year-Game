using Godot;
using System;

public partial class Player : Area2D
{
	[Signal]
	public delegate void HitEventHandler();

	public int Speed { get; set; } = 300; // How fast the player will move (pixels/sec).
	private Timer attackCooldown;
	AnimatedSprite2D attackAnimation;

	public Vector2 ScreenSize; // Size of the game window.
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		attackCooldown = GetNode<Timer>("AttackCooldown");
	}
	
	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero; // The player's movement vector.

		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}

		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}

		if (Input.IsActionPressed("move_down"))
		{
			velocity.Y += 1;
		}

		if (Input.IsActionPressed("move_up"))
		{
			velocity.Y -= 1;
		}

		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			animatedSprite2D.Play();
			GD.Print(velocity);
		}
		else
		{
			animatedSprite2D.Play("Idle");
		}
		 
		if (velocity.X != 0 || velocity.Y != 0)
		{
			animatedSprite2D.Animation = "right";
			animatedSprite2D.FlipH = velocity.X < 0;
		}
		
		Position += velocity * (float)delta;

		Attack();
	}

	public void Attack()
	{
		if(attackCooldown.IsStopped())
		{
			GD.Print("I ran");
			PlayAttackAnimationAtMouse();
			attackCooldown.Start();
		}
	}

	private void PlayAttackAnimationAtMouse()
	{
		attackAnimation = GetNode<AnimatedSprite2D>("AttackAnimation");
		Vector2 mousePosition = GetGlobalMousePosition();

		// Set the AnimatedSprite position to the mouse location
		attackAnimation.GlobalPosition = mousePosition;

		// Play the attack effect animation
		attackAnimation.Play();
	}
	
}
