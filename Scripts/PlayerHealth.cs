using Godot;
using System;
using System.Threading;

public partial class PlayerHealth : Node2D
{
	
	[Export] public float max_health = 500f;
	[Export] public float health;
	ProgressBar bar;
	public override void _Ready()
	{
		health = max_health;
		bar = GetNode<ProgressBar>("HealthBar");
		UpdateHealthBar();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) 
	{
		UpdateHealthBar();
	}

	public void Damage(float damage) 
	{
		health -= damage;

		if(health <= 0)
		{
			GetTree().Root.GetNode<Game>("Game").GameOver();			
		}

		UpdateHealthBar();
	}

	private void UpdateHealthBar()
	{
		float healthPercentage = health / max_health*100;

		bar.Value = healthPercentage;
	}

}
