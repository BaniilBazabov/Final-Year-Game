using Godot;
using System;

public partial class Health : Node2D
{
	[Export] public float max_health = 500f;
	float health;

	public override void _Ready()
	{
		health = max_health;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) 
	{
	}

	public void Damage(float damage) 
	{
		health -= damage;

		if(health <= 0)
		{
			GetParent().QueueFree();
		}
	}
}
