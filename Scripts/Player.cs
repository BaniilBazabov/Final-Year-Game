using Godot;
using System;

public partial class Player : Area2D
{
	[Signal]
	public delegate void HitEventHandler();

	public float Max_health { get; set; } = 1000f;
	public Label HpLabel;
	public float health;
	public float RegenAmount { get; set; } = 5f;
	public float AttackRange { get; set; } = 220.0f;
	public Vector2 pickUpRangeScale = new(1.15f, 1.15f);
	public float Speed { get; set; } = 300; // How fast the player will move (pixels/sec).
	ProgressBar bar;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	private LevelUpMenu levelUpMenu;
	private LevelUpScreen levelUpScreen;
	private Area2D pickUpZone;
	

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	private float level = 1f;
	float experience = 0f;
	private float experienceForNextLevel = 100f;
	private float experienceScalingFactor = 1.15f;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	private Timer AttackCooldown { get; set; }
	private Timer RegenCooldown { get; set; }
	AnimatedSprite2D attackAnimation;
	public float damage { get; set; } = 50;

	public Vector2 ScreenSize; // Size of the game window.
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		health = Max_health;
		bar = GetNode<ProgressBar>("HealthBar");
		UpdateHealthBar();

		ScreenSize = GetViewportRect().Size;
		AttackCooldown = GetNode<Timer>("AttackCooldown");
		RegenCooldown = GetNode<Timer>("RegenCooldown");
		HpLabel = GetNode<Label>("HealthLabel");
		levelUpMenu = GetNode<LevelUpMenu>("LevelUpMenu");
		levelUpScreen = levelUpMenu.GetNode<LevelUpScreen>("LevelUpScreen");
		pickUpZone = GetNode<Area2D>("PickUpZone");
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
		
		GetXp();
		MoveAttackZone();
		Attack();
		RegenHealth();
	}

	private void MoveAttackZone()
	{
		AnimatedSprite2D attackAnimation = GetNode<AnimatedSprite2D>("AttackAnimation");
		Area2D attackZone = GetNode<Area2D>("AttackZone");

		Vector2 mousePosition = GetGlobalMousePosition();
		Vector2 playerPosition = GlobalPosition;

		Vector2 direction = (mousePosition - playerPosition).Normalized();
		Vector2 targetPosition = playerPosition + direction * Mathf.Min(playerPosition.DistanceTo(mousePosition), AttackRange);

		attackAnimation.GlobalPosition = targetPosition;
		attackZone.GlobalPosition = targetPosition;

	}

	public void Attack()
	{
		if (AttackCooldown.IsStopped())
		{
			AnimatedSprite2D attackAnimation = GetNode<AnimatedSprite2D>("AttackAnimation");
			Area2D attackZone = GetNode<Area2D>("AttackZone");
			foreach (Node2D mob in attackZone.GetOverlappingBodies())
			{
				if (mob is Mob)
				{
					Mob mobInstance = (Mob)mob;
					mobInstance.Damage(damage);
				}
			}

			attackAnimation.Play("oneshot");
			AttackCooldown.Start();
		}
	}

	public float DecreaseAttackCooldown(float amount)
	{
		AttackCooldown.WaitTime -= amount;
		return (float)AttackCooldown.WaitTime;
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
		float healthPercentage = health / Max_health * 100;

		bar.Value = healthPercentage;
	}

	private void RegenHealth()
	{
		if (RegenCooldown.IsStopped() && health != Max_health && health < Max_health)
		{
			health += RegenAmount;
			UpdateHealthBar();
			HpLabel.Text = $"{health}/{Max_health}";
			RegenCooldown.Start();
		}
		
	}

	public void IncreasePickUpRange()
	{
		pickUpZone.Scale *= pickUpRangeScale;
		

	}

	public void GetXp()
	{
		foreach (RigidBody2D xpdrop in pickUpZone.GetOverlappingBodies())
		{
			if(xpdrop is Xpdrop)
			{
				Xpdrop expdrop = (Xpdrop)xpdrop;
				experience += expdrop.red_xp;
				if (experience >= experienceForNextLevel)
				{
					LevelUp();
				}
				expdrop.Despawn();
				GD.Print(experience);
			}
		}
	}

	private void LevelUp()
	{
		level++;
		ShowLevelUpMenu();
		GD.Print($"Level Up! New Level: {level}");

		// Adjust experience threshold for the next level
		experienceForNextLevel = experienceScalingFactor * experienceForNextLevel;
		GD.Print($"Experience Required for Next Level: {experienceForNextLevel}");
		GD.Print($"Damage:{damage} and Health: {health}");
	}

	private void ShowLevelUpMenu()
	{
		levelUpScreen.Initialize(this);
		levelUpScreen.ShowMenu();
	}
}

