using Godot;
using System;

public enum SpawningState
	{
		SkeletonOnly,
		ZombieOnly,
		Both
	}
public partial class Game : Node
{
	[Export]
	public PackedScene SkeletonScene { get; set; }

	[Export]
	public PackedScene ZombieScene { get; set; }
	private SpawningState currentSpawningState = SpawningState.SkeletonOnly;
	private int _score;
	Player player;
	private bool paused =  false;
	Hud hud;
	PauseMenu pauseMenu;
	Timer scoreTimer, mobTimer, startTimer;

	public override void _Ready()
	{
		player = GetNode<Player>("Player");
		pauseMenu = GetNode<PauseMenu>("PauseMenu");
		pauseMenu.Hide();

		hud = GetNode<Hud>("HUD");
		mobTimer = GetNode<Timer>("MobTimer");
		scoreTimer = GetNode<Timer>("ScoreTimer");
		startTimer = GetNode<Timer>("StartTimer");

		NewGame();
		
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
		mobTimer.Stop();
		scoreTimer.Stop();
		player.Hide();
		hud.ShowGameOver();
	}
	public void NewGame()
	{
		_score = 0;
		hud.UpdateScore(_score);
		

		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);
		player.health = player.Max_health;
		player.Show();
		
		startTimer.Start();
	}
	private void _on_mob_timer_timeout()
	{
		Vector2 playerPosition = GetNode<Player>("Player").Position;

		Camera2D camera = GetNode<Camera2D>("Player/Camera2D");
		Rect2 viewportRect = camera.GetViewportRect();

		Vector2 SpawnPosition = CalculateRandomSpawnPosition(viewportRect, playerPosition);

		SpawnMobs(SpawnPosition);
	}

	private void SpawnMobs(Vector2 spawnPosition)
	{
		switch (currentSpawningState)
		{
			case SpawningState.SkeletonOnly:
				SpawnSkeleton(spawnPosition);
				break;
			case SpawningState.ZombieOnly:
				SpawnZombie(spawnPosition);
				break;
			case SpawningState.Both:
				SpawnSkeleton(spawnPosition);
				SpawnZombie(spawnPosition);
				break;
		}
	}

	private void SpawnSkeleton(Vector2 spawnPosition)
	{
		Skeleton skeletonInstance = SkeletonScene.Instantiate<Skeleton>();
		skeletonInstance.Position = spawnPosition;
		skeletonInstance.Visible = true;
		AddChild(skeletonInstance);
	}

	private void SpawnZombie(Vector2 spawnPosition)
	{
		Zombie zombieInstance = ZombieScene.Instantiate<Zombie>();
		zombieInstance.Position = spawnPosition;
		zombieInstance.Visible = true;
		AddChild(zombieInstance);
	}

	private Vector2 CalculateRandomSpawnPosition(Rect2 viewportRect, Vector2 playerPosition)
	{
		float bufferDistance = 500.0f;

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
		hud.UpdateScore(_score);
	}


	private void _on_start_timer_timeout()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();

		GetNode<Timer>("OneMinuteTimer").Start();
	}

	private void _on_one_minute_timer_timeout()
	{
		currentSpawningState = SpawningState.ZombieOnly;
	}

	private void _on_two_minute_timer_timeout()
	{
		currentSpawningState = SpawningState.Both;
	}

	private void _on_three_minute_timer_timeout()
	{	// Stop further state changes beyond 3 minutes
		GetNode<Timer>("OneMinuteTimer").Stop();
		GetNode<Timer>("TwoMinuteTimer").Stop();
		GetNode<Timer>("ThreeMinuteTimer").Stop();
	}
	
}

