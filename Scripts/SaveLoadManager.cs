using Godot;
using System;

public partial class SaveLoadManager : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SaveGame(string name)
	{
		DirAccess dir = DirAccess.Open("user://");
		if(!dir.DirExists("SavedGames"))
		{
			dir.MakeDir("SavedGames");
		}

		dir = DirAccess.Open("user://SavedGames");
		if(!dir.DirExists(name))
		{
			dir.MakeDir(name);
		}
	}
}
