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
		Vector2 playerPosition = GetNode<Player>("Player").Position;

		Camera2D camera = GetNode<Camera2D>("Player/Camera2D");
		Rect2 viewportRect = camera.GetViewportRect();

		Vector2 SpawnPosition = CalculateRandomSpawnPosition(viewportRect, playerPosition);

		Mob mob = MobScene.Instantiate<Mob>();
		mob.Position = SpawnPosition;
		AddChild(mob);
	}

	private Vector2 CalculateRandomSpawnPosition(Rect2 viewportRect, Vector2 playerPosition)
	{
		float bufferDistance = 150.0f;

		Rect2 spawnArea = new Rect2(
			viewportRect.Position - new Vector2(bufferDistance, bufferDistance),
			viewportRect.Size + new Vector2(bufferDistance * 2, bufferDistance * 2)
		);

		Vector2 spawnPosition;
		do
		{
			spawnPosition = new Vector2(
				(float)GD.Randf() * spawnArea.Size[0] + spawnArea.Position[0],
				(float)GD.Randf() * spawnArea.Size[1] + spawnArea.Position[1]
			);
		} while (spawnPosition.DistanceTo(playerPosition) < bufferDistance);

		return spawnPosition;
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
