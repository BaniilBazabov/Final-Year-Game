using Godot;
using System;

public partial class Skeleton : RigidBody2D, IEnemy
{
	private Player player;
	private double speed = 75.0;
	private float damage = 50f;
	private Timer attackCooldown;
	private float maxHealth = 100f;
	private float health;
	private Area2D skeletonHitbox;
	private ProgressBar bar;
	private AnimatedSprite2D animatedSprite;

	[Export]
	public PackedScene XpScene { get; set; }
	[Export]
	public PackedScene coinScene { get; set; }

	public override void _Ready()
	{
		player = GetTree().Root.GetNode<Player>("Game/Player");
		attackCooldown = GetNode<Timer>("AttackCooldown");
		health = maxHealth;
		skeletonHitbox = GetNode<Area2D>("SkeletonHitbox");
		bar = GetNode<ProgressBar>("SkeletonHealthBar");
		animatedSprite = GetNode<AnimatedSprite2D>("SkeletonWalk");
		UpdateMobHealthBar();

		// Play Zombie animation
		animatedSprite.Play();
	}

	public override void _Process(double delta)
	{
		UpdateMobHealthBar();

		Vector2 direction = player.GlobalPosition - GlobalPosition;
		direction = direction.Normalized();
		Vector2 velocity = direction * (float)(speed * delta);
		GlobalPosition += velocity;

		if(velocity.X > 0)
		{
			animatedSprite.FlipH = false;
		}
		else if(velocity.X < 0)
		{
			animatedSprite.FlipH = true;
		}

		foreach (Node2D body in skeletonHitbox.GetOverlappingBodies())
		{
			CharacterBody2D characterBody = body as CharacterBody2D;
			if (characterBody != null && characterBody is Player)
			{
				Attack();
			}
		}
	}

	public void Attack()
	{
		if (attackCooldown.IsStopped())
		{
			player.Damage(damage);
			attackCooldown.Start();
		}
	}

	public void Damage(float damage) 
	{
		health -= damage;
		UpdateMobHealthBar();
	}

	private bool xpDropped = false;
	private void UpdateMobHealthBar()
	{
		float healthPercentage = Mathf.Clamp(health / maxHealth * 100, 0, 100);
		bar.Value = healthPercentage;

		if (healthPercentage == 0f && !xpDropped)
		{
			float coinDropRate = (float)GD.RandRange(1, 100);
			if (coinDropRate == 1)
			{
				coin coin = coinScene.Instantiate<coin>();
				GetNode<Game>("../").AddChild(coin);
				coin.GlobalPosition = GlobalPosition;
			}

			// XP drop handled with death logic
			Xpdrop xpdrop = XpScene.Instantiate<Xpdrop>();
			GetNode<Game>("../").AddChild(xpdrop);
			xpdrop.GlobalPosition = GlobalPosition;

			Despawn();
			player.IncreaseKillCount();
			xpDropped = true;
		}
	}

	private void Despawn()
	{
		QueueFree();
	}	
}
