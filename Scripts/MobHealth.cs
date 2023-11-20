using Godot;
using System;

public partial class MobHealth : Node2D
{
	[Export] public float maxHealth = 100f;
	public float health;

	ProgressBar bar;

	public override void _Ready()
	{
		health = maxHealth;
		bar = GetNode<ProgressBar>("MobHealthBar");
		UpdateMobHealthBar();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) 
	{
		UpdateMobHealthBar();
	}

	public void Damage(float damage) 
	{
		health -= damage;

		if (health <= 0)
		{
			Dispawn();
		}

		UpdateMobHealthBar();
	}

	private void Dispawn()
	{
		QueueFree(); // This is a simple example; you might want to handle this differently based on your game logic.
	}

	private void UpdateMobHealthBar()
	{
		float healthPercentage = Mathf.Clamp(health / maxHealth * 100, 0, 100);

		bar.Value = healthPercentage;
	}
}
