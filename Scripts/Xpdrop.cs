using Godot;
using System;

public partial class Xpdrop : RigidBody2D
{
	[Export]
	public float red_xp = 10f;
	public override void _Ready()
	{
		
	}

	public override void _Process(double delta)
	{
		
	}

	public void Despawn()
	{
		QueueFree();
	}
}
