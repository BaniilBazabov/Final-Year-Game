using Godot;
using System;

public enum SpawningState
	{
		SkeletonOnly,
		ZombieOnly,
		BossZombieOnly
	}
public partial class Game : Node //this file manages all the events that happen during the gameplay. Decided the position of enemies spawning, the time they spawn at and what time and which enemy appears.
{                                //Also, contains logic regarding starting/ending/pausing the game and the timers
	public static Game Instance;
	[Export]
	public PackedScene SkeletonScene { get; set; }

	[Export]
	public PackedScene ZombieScene { get; set; }
	[Export]
	public PackedScene BossZombieScene { get; set; }
	private SpawningState currentSpawningState = SpawningState.SkeletonOnly;
	private int _score;
	public float playerGold = 0;
	Player player;
	private bool paused =  false;
	Hud hud;
	PauseMenu pauseMenu;
	Timer scoreTimer, mobTimer, startTimer, oneMinuteTimer, twoMinuteTimer, threeMinuteTimer;
	AudioStreamPlayer gameAudio;

	public override void _Ready()
	{
		if(Instance == null)
		{
			Instance = this;
		} 
		
		player = GetNode<Player>("Player");
		pauseMenu = GetNode<PauseMenu>("PauseMenu");
		pauseMenu.Hide();

		hud = GetNode<Hud>("HUD");
		mobTimer = GetNode<Timer>("MobTimer");
		scoreTimer = GetNode<Timer>("ScoreTimer");
		startTimer = GetNode<Timer>("StartTimer");
		oneMinuteTimer = GetNode<Timer>("OneMinuteTimer");
		twoMinuteTimer = GetNode<Timer>("TwoMinuteTimer");
		threeMinuteTimer = GetNode<Timer>("ThreeMinuteTimer");

		gameAudio = GetNode<AudioStreamPlayer>("GameMusic");

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

	public void ReturnToMainMenu()
	{
		_UnpauseGame();
		GetTree().ChangeSceneToFile("res://Scripts/playerBase1.tscn");
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
			case SpawningState.BossZombieOnly:
				SpawnBossZombie(CalculateBossSpawnPosition(spawnPosition));
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

	private void SpawnBossZombie(Vector2 spawnPosition)
	{
		BossZombie bossZombie = BossZombieScene.Instantiate<BossZombie>();
		bossZombie.Position = spawnPosition;
		bossZombie.Visible = true;
		AddChild(bossZombie);
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

	private Vector2 CalculateBossSpawnPosition(Vector2 playerPosition)
	{
		float spawnDistance = 400.0f; // Distance from the player to spawn the boss

		// Calculate a random angle around the player
		float angle = (float)GD.RandRange(0, Mathf.Pi * 2);

		// Calculate the spawn position based on the angle and spawn distance
		Vector2 spawnPosition = playerPosition + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * spawnDistance;

		return spawnPosition;
	}


	private void _on_score_timer_timeout()
	{
		_score++;
		hud.UpdateScore(_score);
	}


	private void _on_start_timer_timeout()
	{
		mobTimer.Start();
		scoreTimer.Start();
		oneMinuteTimer.Start();
	}

	private void _on_one_minute_timer_timeout()
	{
		// currentSpawningState = SpawningState.BossZombieOnly;
		// DespawnEnemies();
		currentSpawningState = SpawningState.ZombieOnly;
		twoMinuteTimer.Start();
	}

	private void _on_two_minute_timer_timeout()
	{
		currentSpawningState = SpawningState.BossZombieOnly;
		DespawnEnemies();
		Vector2 playerPosition = GetNode<Player>("Player").Position;
		SpawnMobs(CalculateBossSpawnPosition(playerPosition));
		
	}

	private void _on_audio_stream_player_finished()
	{
		gameAudio.Play();
	}
	private void DespawnEnemies()
	{
		// Iterate through the children of the node
		foreach (Node node in GetChildren())
		{
			// Check if the child node is an enemy
			if (node is Skeleton || node is Zombie)
			{
				// Remove the enemy from the scene
				node.QueueFree();
			}
		}
		mobTimer.Stop();
	}

	private void _on_three_minute_timer_timeout()
	{	// Stop further state changes beyond 3 minutes
		oneMinuteTimer.Stop();
		twoMinuteTimer.Stop();
		threeMinuteTimer.Stop();
	}
	
}
