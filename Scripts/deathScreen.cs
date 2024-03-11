using Godot;
using System;

public partial class deathScreen : Control //this file manages the screen that appears when the player dies.
{
	private void _on_restart_button_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scripts/map.tscn");
	}


	private void _on_main_menu_button_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scripts/mainMenu.tscn");
	}

	private void _on_player_base_button_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scripts/playerBase1.tscn");
	}

}


