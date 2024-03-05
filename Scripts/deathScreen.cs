using Godot;
using System;

public partial class deathScreen : Control
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


