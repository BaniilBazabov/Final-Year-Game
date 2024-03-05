using Godot;
using System;

public partial class Xpdrop : RigidBody2D
{
	[Export]
	public Texture2D blueXpTexture;
	[Export]
	public Texture2D redXpTexture;
	[Export]
	public Texture2D yellowXpTexture;
	public float xpValue = 0f;

	public void SetXpType(string type)
	{
		Sprite2D sprite = GetNode<Sprite2D>("DropSkin");
		switch (type)
		{
			case "Skeleton":
				sprite.Texture = blueXpTexture;
				xpValue = 10f;
				break;
			case "Zombie":
				sprite.Texture = blueXpTexture;
				xpValue = 50f;
				break;
			case "BossZombie":
				sprite.Texture = yellowXpTexture;
				xpValue = 100f;
				break;
			default:
				sprite.Texture = blueXpTexture;
				xpValue = 10f;
				break;
		}
	}

	public void Despawn()
	{
		QueueFree();
	}
}
