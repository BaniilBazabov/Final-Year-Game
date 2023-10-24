using Godot;
using System;

public partial class Mob : RigidBody2D
{
	private Node2D player;
	private float speed = 100f;
	private void _on_visible_on_screen_enabler_2d_screen_exited()
	{
		QueueFree();
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
		animatedSprite2D.Play(mobTypes[GD.Randi()% mobTypes.Length]);

		player = GetNode<Player>("Player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(player != null)
		{
			Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();

			// Move the mob towards the player
			ApplyCentralImpulse(direction * speed);
		}
	}


}
