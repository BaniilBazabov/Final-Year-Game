using Godot;
using System;

public partial class Mob : RigidBody2D 
{
	private Node2D player;
	private double speed = 50.0;
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
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		player = GetTree().Root.GetNode<Node2D>("Game/Player");

		if(player != null)
		{
			Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
			Vector2 velocity = direction * (float)(speed * delta);

			// Move the mob towards the player
			GlobalPosition += velocity;
		}
	}


}
