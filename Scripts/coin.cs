using Godot;
using System;

public partial class coin : RigidBody2D
{
	[Export]
	public int amount = 5;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Despawn()
	{
		QueueFree();
	}
}