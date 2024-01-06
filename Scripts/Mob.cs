using Godot;
using System;
using System.Diagnostics.Metrics;
using System.Xml.Linq;

public partial class Mob : RigidBody2D 
{
	[Export]
	public PackedScene XpScene { get; set; }
	private Player player;
	private double speed = 50.0;
	private float damage = 50f;
	private Timer attackCooldown;
	[Export] public float maxHealth = 100f;
	public float health;
	ProgressBar bar;

	private void _on_visible_on_screen_enabler_2d_screen_exited()
	{
		QueueFree();
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var animatedSprite2D = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
		string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
		animatedSprite2D.Play(mobTypes[GD.Randi()% mobTypes.Length]);

		player = GetTree().Root.GetNode<Player>("Game/Player");

		attackCooldown = GetNode<Timer>("AttackCooldown");
		health = maxHealth;
		bar = GetNode<ProgressBar>("MobHealthBar");
		UpdateMobHealthBar();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		UpdateMobHealthBar();

		if (player != null)
		{
			Vector2 direction = player.GlobalPosition - GlobalPosition;
			float distanceToPlayer = direction.Length();

			if (distanceToPlayer <= 7)
			{
				Attack();
			} 
			else 
			{
				// If not attacking, move the mob towards the player
				direction = direction.Normalized();
				Vector2 velocity = direction * (float)(speed * delta);
				GlobalPosition += velocity;
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
			Xpdrop xpdrop = XpScene.Instantiate<Xpdrop>();
			GetNode<Game>("../").AddChild(xpdrop);
			xpdrop.GlobalPosition = GlobalPosition;
			Despawn();
			xpdropped = true;
		}
	}

	private void Despawn()
	{
		QueueFree();
	}	


}
