using Godot;
using System;

public partial class Game : Node
{
	[Export]
	public PackedScene MobScene { get; set; }
	private int _score;

	public override void _Ready()
	{
		NewGame();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void GameOver()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
	}
	public void NewGame()
	{
		_score = 0;
		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);
	}
	private void _on_mob_timer_timeout()
	{
		var player = GetNode<Player>("Player");
		Mob mob = MobScene.Instantiate<Mob>();
		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();

		float direction = mobSpawnLocation.Rotation;
		mob.Position = mobSpawnLocation.Position;

		var velocity = new Vector2(200, 0);
		mob.LinearVelocity = velocity.Rotated(direction);

		AddChild(mob);
	}


	private void _on_score_timer_timeout()
	{
		_score++;
	}


	private void _on_start_timer_timeout()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}
	
}




