using Godot;
using System;


public partial class Mob : RigidBody2D 
{
	[Export]
	public PackedScene XpScene { get; set; }
	[Export]
	public PackedScene coinScene { get; set; }
	private Player player;
	private double speed = 50.0;
	private float damage = 50f;
	private Timer attackCooldown;
	[Export] public float maxHealth = 100f;
	public float health;
	Area2D mobHitbox;
	ProgressBar bar;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var animatedSprite2D = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
		string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
		animatedSprite2D.Play(mobTypes[GD.Randi()% mobTypes.Length]);

		player = GetTree().Root.GetNode<Player>("Game/Player");

		mobHitbox = GetNode<Area2D>("MobHitbox");
		attackCooldown = GetNode<Timer>("AttackCooldown");
		health = maxHealth;
		bar = GetNode<ProgressBar>("MobHealthBar");
		UpdateMobHealthBar();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		UpdateMobHealthBar();

		Vector2 direction = player.GlobalPosition - GlobalPosition;
		
		// If not attacking, move the mob towards the player
		direction = direction.Normalized();
		Vector2 velocity = direction * (float)(speed * delta);
		GlobalPosition += velocity;

		foreach (Node2D body in mobHitbox.GetOverlappingBodies())
		{
			CharacterBody2D characterBody = body as CharacterBody2D;
			if (characterBody != null)
				{
					if (characterBody is Player)
					{
						Attack();
					}
				}
		}

	}

	public void Attack()
	{
		if(attackCooldown.IsStopped())
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
	private bool xpdropped = false;
	private void UpdateMobHealthBar()
	{
		float healthPercentage = Mathf.Clamp(health / maxHealth * 100, 0, 100);

		bar.Value = healthPercentage;
		if (healthPercentage == 0f && !xpdropped)
		{
			float coinDropRate = (float)GD.RandRange(1,100);

			if(coinDropRate==1)
			{
				coin coin = coinScene.Instantiate<coin>();
				GetNode<Game>("../").AddChild(coin);
				coin.GlobalPosition = GlobalPosition;
			}
			//xp drop handled with death logic
			Xpdrop xpdrop = XpScene.Instantiate<Xpdrop>();
			GetNode<Game>("../").AddChild(xpdrop);
			xpdrop.GlobalPosition = GlobalPosition;
			Despawn();
			player.IncreaseKillCount();
			xpdropped = true;
		}
	}

	private void Despawn()
	{
		QueueFree();
	}	


}
