using Godot;
using System;

public partial class playerBase1 : Node2D
{
	PauseMenu pauseMenu;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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
