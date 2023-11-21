using Godot;
using System;

public partial class Player : Area2D
{
	[Signal]
	public delegate void HitEventHandler();

	[Export] public float max_health = 500f;
	[Export] public float health;
	ProgressBar bar;
	public int Speed { get; set; } = 300; // How fast the player will move (pixels/sec).
	private Timer attackCooldown;
	AnimatedSprite2D attackAnimation;
	private float damage = 50;

	public Vector2 ScreenSize; // Size of the game window.
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		health = max_health;
		bar = GetNode<ProgressBar>("HealthBar");
		UpdateHealthBar();

		ScreenSize = GetViewportRect().Size;
		attackCooldown = GetNode<Timer>("AttackCooldown");
	}
	
	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
		UpdateHealthBar();
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
		if (attackCooldown.IsStopped())
		{
			AnimatedSprite2D attackAnimation = GetNode<AnimatedSprite2D>("AttackAnimation");
			Vector2 mousePosition = GetGlobalMousePosition();
			Vector2 playerPosition = GlobalPosition;

			Vector2 direction = (mousePosition - playerPosition).Normalized();
			float maxAttackRange = 150.0f;
			Area2D attackZone = GetNode<Area2D>("AttackZone");
			Vector2 targetPosition = playerPosition + direction * Mathf.Min(playerPosition.DistanceTo(mousePosition), maxAttackRange);
			attackAnimation.GlobalPosition = targetPosition;
			attackZone.GlobalPosition = attackAnimation.GlobalPosition;

			foreach (Node2D mob in attackZone.GetOverlappingBodies())
			{
				if (mob is Mob)
				{
					GD.Print("Mob is actually detected");
					Mob mobInstance = (Mob)mob;
					mobInstance.Damage(damage);
				}
			}

			attackAnimation.Play("oneshot");
			attackCooldown.Start();
		}
	}

	public void Damage(float damage) 
	{
		health -= damage;

		if(health <= 0)
		{
			GetTree().Root.GetNode<Game>("Game").GameOver();			
		}

		UpdateHealthBar();
	}

	private void UpdateHealthBar()
	{
		float healthPercentage = health / max_health*100;

		bar.Value = healthPercentage;
	}
}

