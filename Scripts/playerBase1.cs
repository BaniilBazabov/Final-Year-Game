using Godot;
using System;

public partial class playerBase1 : Node2D
{
	Area2D teleportArea;
	PauseMenu pauseMenu;
	private Sprite2D e;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		e = GetNode<Sprite2D>("E");
		e.Visible = false;
		teleportArea = GetNode<Area2D>("TeleportArea");
		pauseMenu = GetNode<PauseMenu>("PauseMenu");
		pauseMenu.Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("pause_menu"))
		{
			_PauseGame();
		}

		bool isInTeleportArea = false;

		foreach (Node2D body in teleportArea.GetOverlappingBodies())
		{
			if (body is Player)
			{
				isInTeleportArea = true;
				break;
			}
		}

		// Set the visibility of the "E" sprite based on player's position
		e.Visible = isInTeleportArea;

		// Teleportation logic
		if (isInTeleportArea && Input.IsActionJustPressed("interact"))
		{
			
			GetTree().ChangeSceneToFile("res://Scripts/map.tscn");
		}
	}
	private void _PauseGame()
	{
		GetTree().Paused = true;
		pauseMenu.Show();
	}
	private void _UnpauseGame()
	{
		GetTree().Paused = false;
		pauseMenu.Hide();
	}

	private void _CloseGame()
	{
		GetTree().Quit();
	}
}
