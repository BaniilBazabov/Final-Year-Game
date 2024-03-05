using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Signal]
	public delegate void HitEventHandler();

	public float Max_health { get; set; } = 1000f;
	public Label HpLabel;
	public Label killLabel;
	public float health;
	public float RegenAmount { get; set; } = 5f;
	public float AttackRange { get; set; } = 220.0f;
	private Vector2 pickUpRangeScale = new(1.15f, 1.15f);
	private Vector2 attackZoneScale = new(1.15f, 1.15f);
	private Vector2 attackAnimationScale = new(1.15f, 1.15f);
	public float Speed { get; set; } = 300; // How fast the player will move (pixels/sec).
	ProgressBar bar;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	private LevelUpMenu levelUpMenu;
	private LevelUpScreen levelUpScreen;
	private Area2D pickUpZone;
	private Area2D attackZone;
	private  Vector2 currentVelocity;
	

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	private float level = 1f;
	private float experience = 0f;
	public float gold = 0f;
	public float kills = 0f;
	private float experienceForNextLevel = 100f;
	private float experienceScalingFactor = 1.15f;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	private Timer AttackCooldown { get; set; }
	private Timer RegenCooldown { get; set; }
	AnimatedSprite2D attackAnimation;
	public float damage { get; set; } = 100;
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
		killLabel = GetNode<Label>("KillLabel");
		levelUpMenu = GetNode<LevelUpMenu>("LevelUpMenu");
		levelUpScreen = levelUpMenu.GetNode<LevelUpScreen>("LevelUpScreen");
		pickUpZone = GetNode<Area2D>("PickUpZone");
		attackZone = GetNode<Area2D>("AttackZone");
		attackAnimation = GetNode<AnimatedSprite2D>("AttackAnimation");

		killLabel.Text = "Kills: 0";

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
		base._Process(delta);
		handleInput();
		Velocity = currentVelocity;
		MoveAndSlide();

		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		if (Velocity.Length() > 0)
		{
			Velocity = Velocity.Normalized() * Speed;
			animatedSprite2D.Play();
		}
		else
		{
			animatedSprite2D.Play("Idle");
		}
		 
		if (Velocity.X != 0 || Velocity.Y != 0)
		{
			animatedSprite2D.Animation = "right";
			animatedSprite2D.FlipH = Velocity.X < 0;
		}
		
		GetXpAndCoins();
		MoveAttackZone();
		Attack();
		RegenHealth();
	}

	private void handleInput()
	{
		currentVelocity = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		currentVelocity *= Speed;
	}

	private void MoveAttackZone()
	{
		AnimatedSprite2D attackAnimation = GetNode<AnimatedSprite2D>("AttackAnimation");

		Vector2 mousePosition = GetGlobalMousePosition();
		Vector2 playerPosition = GlobalPosition;

		Vector2 direction = (mousePosition - playerPosition).Normalized();
		Vector2 targetPosition = playerPosition + direction * Mathf.Min(playerPosition.DistanceTo(mousePosition), AttackRange);

		attackAnimation.GlobalPosition = targetPosition;
		attackZone.GlobalPosition = targetPosition;

	}

	public void Attack()
	{
		if (AttackCooldown.IsStopped()&& GetTree().CurrentScene is Game)
		{
			foreach (Node2D mobNode in attackZone.GetOverlappingBodies())
			{
				if (mobNode is IEnemy)
				{
					IEnemy enemy = (IEnemy)mobNode;
					enemy.Damage(damage);
				}
			}

			// Play attack animation
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

	public void IncreaseHP()
	{
		Max_health *= 1.1f;
		HpLabel.Text = "Hair Preserved: " + $"{health}/{Max_health}";
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

	public void IncreaseAttackRadius()
	{
		attackZone.Scale *= attackZoneScale;
		attackAnimation.Scale *= attackAnimationScale;
	}

	public void GetXpAndCoins()
	{
		foreach (Node2D body in pickUpZone.GetOverlappingBodies())
		{
			if (body is Xpdrop xpdrop)
			{
				// Use the safe cast and check for null
				RigidBody2D rigidBody = xpdrop as RigidBody2D;
				if (rigidBody != null)
				{
					experience += xpdrop.red_xp;
					if (experience >= experienceForNextLevel)
					{
						LevelUp();
					}

					xpdrop.Despawn();
					
				}
			}
			else if (body is coin coin)
			{
				RigidBody2D rigidBody = coin as RigidBody2D;
				if(rigidBody!= null)
				{
					gold += coin.amount;
					coin.Despawn();
				}
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

	public void IncreaseKillCount()
	{
		killLabel.Text = "Kills: " + $"{kills++}";
	}

	private void ShowLevelUpMenu()
	{
		levelUpScreen.Initialize(this);
		levelUpScreen.ShowMenu();
	}
}

