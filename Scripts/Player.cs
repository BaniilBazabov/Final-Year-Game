using Godot;
using System;

public partial class Player : Area2D
{
	[Signal]
	public delegate void HitEventHandler();

	[Export] public float max_health = 1000f;
	[Export] public float health;
	[Export] float attackRange = 220.0f;
	public int Speed { get; set; } = 300; // How fast the player will move (pixels/sec).
	ProgressBar bar;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	private LevelUpMenu levelUpMenu;
	private LevelUpScreen levelUpScreen;
	Button upgradeButton1;
	Button upgradeButton2;
	Button upgradeButton3;
	Button upgradeButton4;
	

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	private float level = 1f;
	[Export] float experience = 0f;
	private float experienceForNextLevel = 100f;
	private float experienceScalingFactor = 1.15f;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	private Timer attackCooldown;
	AnimatedSprite2D attackAnimation;
	private float damage = 100;

	public Vector2 ScreenSize; // Size of the game window.
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		health = max_health;
		bar = GetNode<ProgressBar>("HealthBar");
		UpdateHealthBar();

		ScreenSize = GetViewportRect().Size;
		attackCooldown = GetNode<Timer>("AttackCooldown");
		levelUpMenu = GetNode<LevelUpMenu>("LevelUpMenu");
		levelUpScreen = levelUpMenu.GetNode<LevelUpScreen>("LevelUpScreen");

		upgradeButton1 = GetNode<Button>("/root/Game/Player/LevelUpMenu/LevelUpScreen/Panel/VBoxContainer/SkillOne");
		upgradeButton2 = GetNode<Button>("/root/Game/Player/LevelUpMenu/LevelUpScreen/Panel/VBoxContainer/SkillTwo");
		upgradeButton3 = GetNode<Button>("/root/Game/Player/LevelUpMenu/LevelUpScreen/Panel/VBoxContainer/SkillThree");
		upgradeButton4 = GetNode<Button>("/root/Game/Player/LevelUpMenu/LevelUpScreen/Panel/VBoxContainer/SkillFour");

		upgradeButton1.Connect(Button.SignalName.Pressed, new Callable(this, "_onUpgradeButtonPressed"));
		upgradeButton2.Connect(Button.SignalName.Pressed, new Callable(this, "_onUpgradeButtonPressed"));
		upgradeButton3.Connect(Button.SignalName.Pressed, new Callable(this, "_onUpgradeButtonPressed"));
		upgradeButton4.Connect(Button.SignalName.Pressed, new Callable(this, "_onUpgradeButtonPressed"));
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
	}

	private void MoveAttackZone()
	{
		AnimatedSprite2D attackAnimation = GetNode<AnimatedSprite2D>("AttackAnimation");
		Area2D attackZone = GetNode<Area2D>("AttackZone");

		Vector2 mousePosition = GetGlobalMousePosition();
		Vector2 playerPosition = GlobalPosition;

		Vector2 direction = (mousePosition - playerPosition).Normalized();
		Vector2 targetPosition = playerPosition + direction * Mathf.Min(playerPosition.DistanceTo(mousePosition), attackRange);

		attackAnimation.GlobalPosition = targetPosition;
		attackZone.GlobalPosition = targetPosition;

	}

	public void Attack()
	{
		if (attackCooldown.IsStopped())
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
		float healthPercentage = health / max_health * 100;

		bar.Value = healthPercentage;
	}

	public void GetXp()
	{
		Area2D pickUpZone = GetNode<Area2D>("PickUpZone");
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
		experienceForNextLevel = (experienceScalingFactor * experienceForNextLevel) + (experienceForNextLevel * 0.5f);
		GD.Print($"Experience Required for Next Level: {experienceForNextLevel}");
	}

	private void ShowLevelUpMenu()
	{
		
		levelUpScreen.ShowMenu();
	}

	private void _onUpgradeButtonPressed()
	{
		levelUpScreen.HideMenu();

	}


}

