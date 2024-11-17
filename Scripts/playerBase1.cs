using Godot;
using System;

public partial class playerBase1 : Node2D //This file manages the player base and all of its mechanics.
{
	Area2D teleportArea;
	Area2D potionMasterArea;
	PauseMenu pauseMenu;
	private Sprite2D e;
	private float playerGold;
	private Label goldLabel;
	private AudioStreamPlayer audio;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		e = GetNode<Sprite2D>("E");
		e.Visible = false;
		potionMasterArea = GetNode<Area2D>("PotionMaster");
		teleportArea = GetNode<Area2D>("TeleportArea");
		pauseMenu = GetNode<PauseMenu>("PauseMenu");
		pauseMenu.Hide();
		goldLabel = GetNode<Label>("PlayerGoldLabel");
		audio = GetNode<AudioStreamPlayer>("PlayerBase1Audio");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("pause_menu"))
		{
			_PauseGame();
		}

		bool isInTeleportArea = false;
		bool isInPotionMasterArea = false;
		goldLabel.Text = PlayerRecords.PlayerTotalGold.ToString() + " gold";

		foreach (Node2D body in teleportArea.GetOverlappingBodies())
		{
			if (body is Player)
			{
				isInTeleportArea = true;
				break;
			}
		}

		foreach (Node2D body in potionMasterArea.GetOverlappingBodies())
		{
			if (body is Player)
			{
				isInPotionMasterArea = true;
				break;
			}
		}

		// Set the visibility of the "E" sprite based on player's position
		if(isInTeleportArea)
		{
			e.Position = new Vector2(863, 1227);
			e.Visible = true;
		}
		else if(isInPotionMasterArea)
		{
			e.Position = new Vector2(783, 419);
			e.Visible = true;
		}
		else {e.Visible = false;}

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

	private void _on_player_base_1_audio_finished()
	{
		audio.Play();
	}
}
