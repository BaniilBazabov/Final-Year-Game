using Godot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public static class SaveLoadManager 
{
	public static void SaveGame(string name)
	{
		DirAccess dir = DirAccess.Open("user://");
		if(!dir.DirExists("SavedGames"))
		{
			dir.MakeDir("SavedGames");
		}

		dir = DirAccess.Open("user://SavedGames");

		Dictionary<string, string> saveGameData = new Dictionary<string, string>();
		saveGameData.Add("PlayerTotalGold", PlayerRecords.PlayerTotalGold.ToString());
		saveGameData.Add("PlayerTotalKills", PlayerRecords.PlayerTotalKills.ToString());

		string savedDataJson = JsonConvert.SerializeObject(saveGameData);

		FileAccess file = FileAccess.Open($"user://SavedGames/{name}.json", FileAccess.ModeFlags.Write);
		file.StoreString(savedDataJson);
		file.Close();
	}

	public static void LoadGame(string name)
	{
		string filePath = $"user://SavedGames/{name}.json";
		if(FileAccess.FileExists(filePath))
		{
			FileAccess file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
			string content = file.GetAsText();
			Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
			PlayerRecords.PlayerTotalGold = float.Parse(data["PlayerTotalGold"]);
			PlayerRecords.PlayerTotalKills = float.Parse(data["PlayerTotalKills"]);
			file.Close();
		}
		else
		{
		SaveGame("TheOneAndOnlySave");
		LoadGame("TheOneAndOnlySave");
		}
	}
}
