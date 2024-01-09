using Godot;
using System;

public partial class Game : Node
{
	[Export]
	public PackedScene MobScene { get; set; }
	private int _score;
	Player player;
	private bool paused =  false;
	PauseMenu pauseMenu;

	public override void _Ready()
	{
		player = GetNode<Player>("Player");
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
	public void _UnpauseGame()
	{
		GetTree().Paused = false;
		pauseMenu.Hide();
	}

	public void CloseGame()
	{
		GetTree().Quit();
	}
	public void GameOver()
	{
		GetNode<Hud>("HUD").ShowGameOver();
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		player.Hide();
	}
	public void NewGame()
	{
		var hud = GetNode<Hud>("HUD");
		hud.UpdateScore(_score);
		hud.ShowMessage("Get Ready!");
		_score = 0;

		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);
		player.health = player.Max_health;
		player.Show();
		
		GetNode<Timer>("StartTimer").Start();
	}
	private void _on_mob_timer_timeout()
	{
		Mob mob = MobScene.Instantiate<Mob>();
		AddChild(mob);
	}


	private void _on_score_timer_timeout()
	{
		_score++;
		GetNode<Hud>("HUD").UpdateScore(_score);
	}


	private void _on_start_timer_timeout()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}
	
}
