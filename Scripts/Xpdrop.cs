using Godot;
using System;

public partial class Xpdrop : RigidBody2D
{
	public float xpValue = 0f;

	public void SetXpType(string type)
	{
		AnimatedSprite2D sprite = GetNode<AnimatedSprite2D>("DropSkin");
		switch (type)
		{
			case "Skeleton":
				sprite.Play("Blue");
				xpValue = 10f;
				break;
			case "Zombie":
				sprite.Play("Red");
				xpValue = 50f;
				break;
			case "BossZombie":
				sprite.Play("Yellow");
				xpValue = 100f;
				break;
			default:
				sprite.Play("Blue");
				xpValue = 10f;
				break;
		}
	}

	public void Despawn()
	{
		QueueFree();
	}
}
