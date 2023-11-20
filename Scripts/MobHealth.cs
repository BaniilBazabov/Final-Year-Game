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
		UpdateMobHealthBar();
	}

	private async void UpdateMobHealthBar()
	{
		float healthPercentage = Mathf.Clamp(health / maxHealth * 100, 0, 100);

		bar.Value = healthPercentage;
		if (healthPercentage == 0f)
		{
			await ToSignal(GetTree().CreateTimer(1.0), "timeout"); // Delay for 1 second
			Despawn();
		}
	}

	private void Despawn()
	{
		QueueFree();
	}	
}
