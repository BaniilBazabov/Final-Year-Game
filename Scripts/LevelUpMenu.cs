using Godot;
using System;

public partial class LevelUpMenu : CanvasLayer
{
	[Export]
	public PackedScene levelUpScreenScene{ get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		levelUpScreenScene = GD.Load<PackedScene>("res://Scripts/LevelUpScreen.tscn");
	}

	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
