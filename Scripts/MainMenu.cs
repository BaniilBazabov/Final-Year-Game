using Godot;
using System;

public partial class MainMenu : Control
{
	AudioStreamPlayer audio;
	public override void _Ready()
	{
		SaveLoadManager.LoadGame("TheOneAndOnlySave");
		audio = GetNode<AudioStreamPlayer>("MainMenuAudio");
	}

	private void _on_play_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scripts/playerBase1.tscn");
	}


	private void _on_settings_pressed()
	{
		
	}


	private void _on_quit_game_pressed()
	{
		GetTree().Quit();
	}

	private void _on_main_menu_audio_finished()
	{
		audio.Play();
	}
}

