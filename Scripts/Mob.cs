using Godot;
using System;
using System.Diagnostics.Metrics;

public partial class Mob : RigidBody2D 
{
	private Node2D player;
	private double speed = 50.0;
	private float damage = 50f;
	private MobHealth mobHealth;

	private Timer attackCooldown;

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
		attackCooldown = GetNode<Timer>("AttackCooldown");
		mobHealth = GetNodeOrNull<MobHealth>("MobHealth");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		player = GetTree().Root.GetNode<Node2D>("Game/Player");

		if (player != null)
		{
			Vector2 direction = player.GlobalPosition - GlobalPosition;
			float distanceToPlayer = direction.Length();

			if (distanceToPlayer <= 10)
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
			player = GetTree().Root.GetNode<Node2D>("Game/Player");
			player.GetNode<PlayerHealth>("PlayerHealth").Damage(damage);
			attackCooldown.Start();
		}
	}

	public void takeDamage(float damage)
	{
		mobHealth?.Damage(damage);
	}


}
