using Godot;
using System;

public partial class VictoryScreen : Control //this file manages the screen which appears when the player kills the last boss.
{
	private void _on_player_base_button_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scripts/playerBase1.tscn");
	}


	private void _on_exit_button_pressed()
	{
		GetTree().Quit();
	}
}
