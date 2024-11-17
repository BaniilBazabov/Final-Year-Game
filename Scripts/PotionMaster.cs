using Godot;
using System;

public partial class PotionMaster : Area2D

{
	AnimatedSprite2D chilling;
	
	public override void _Ready()
	{
		chilling = GetNode<AnimatedSprite2D>("Animation");
	}

	
	public override void _Process(double delta)
	{
		chilling.Play();
	}
}
