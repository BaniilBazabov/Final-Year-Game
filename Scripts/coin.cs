using Godot;
using System;

public partial class coin : RigidBody2D //this file manages the gold coins that drop from enemies on death.
{
	[Export]
	public int amount = 5;
	// Called when the node enters the scene tree for the first time.

	public void Despawn()
	{
		QueueFree();
	}
}
