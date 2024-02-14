using Godot;
using System;

public partial class deathScreen : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_restart_button_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scripts/map.tscn");
	}


private void _on_main_menu_button_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scripts/mainMenu.tscn");
	}
}
