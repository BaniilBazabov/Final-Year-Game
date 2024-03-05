using Godot;
using System;

public partial class BossZombie : RigidBody2D, IEnemy
{
	private enum BossState
	{
		Spawning,
		Fighting,
		Dying
	}
	private BossState state = BossState.Spawning;
	private Player player;
	private double speed = 100.0;
	private float damage = 200f;
	private Timer attackCooldown;
	private Timer roundEndCountdown;
	private float maxHealth = 1500f;
	private float health;
	private Area2D bossZombieHitbox;
	private ProgressBar bar;
	private AnimatedSprite2D BossZombieAnimation;

	[Export]
	public PackedScene XpScene { get; set; }
	[Export]
	public PackedScene coinScene { get; set; }

	public override void _Ready()
	{
		player = GetTree().Root.GetNode<Player>("Game/Player");
		attackCooldown = GetNode<Timer>("AttackCooldown");
		roundEndCountdown = GetNode<Timer>("RoundEndCountdown");
		health = maxHealth;
		bossZombieHitbox = GetNode<Area2D>("BossZombieHitbox");
		bar = GetNode<ProgressBar>("BossZombieHealthBar");
		BossZombieAnimation = GetNode<AnimatedSprite2D>("BossZombieWalk");
		UpdateMobHealthBar();

	}

	public override void _Process(double delta)
	{
		UpdateMobHealthBar();

		switch(state)
		{
			case BossState.Spawning:
				BossZombieAnimation.Play("Arising");
				break;

			case BossState.Fighting:
				BossZombieAnimation.Play("Walking");

				Vector2 direction = player.GlobalPosition - GlobalPosition;
				direction = direction.Normalized();
				Vector2 velocity = direction * (float)(speed * delta);
				GlobalPosition += velocity;

				BossZombieAnimation.FlipH = velocity.X < 0;

				foreach (Node2D body in bossZombieHitbox.GetOverlappingBodies())
				{
					CharacterBody2D characterBody = body as CharacterBody2D;
					if (characterBody != null && characterBody is Player)
					{
						Attack();
					}
				}
				break;
			case BossState.Dying:
				BossZombieAnimation.Play("Death");
				break;
			default: 
				state = BossState.Fighting;
				BossZombieAnimation.Play("Walking");
				break;
		}
	}

	private void _on_boss_zombie_walk_animation_finished()
	{
		if (BossZombieAnimation.Animation == "Arising")
		{
			state = BossState.Fighting;
		}
		else if (BossZombieAnimation.Animation == "Death")
		{
			dropLootAndDespawn();
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
			state = BossState.Dying;
			player.IncreaseKillCount();
			xpDropped = true;
		}
	}
	private void dropLootAndDespawn()
	{
		coin coin = coinScene.Instantiate<coin>();
		GetNode<Game>("../").AddChild(coin);
		coin.GlobalPosition = GlobalPosition;
		

		// XP drop handled with death logic + saving data at the round end.
		Xpdrop xpdrop = XpScene.Instantiate<Xpdrop>();
		GetNode<Game>("../").AddChild(xpdrop);
		xpdrop.GlobalPosition = GlobalPosition;
		xpdrop.SetXpType("BossZombie");
		PlayerRecords.UpdatePlayerRecords(player.gold, player.kills);
		player.gold = 0f; player.kills = 0f;
		Despawn();
	}

	private void Despawn()
	{
		QueueFree();
		GetTree().ChangeSceneToFile("res://Scripts/victoryScreen.tscn");
	}	
}

