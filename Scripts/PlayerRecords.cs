using Godot;

public static class PlayerRecords //this file is used to keep track of player's gold and kills when loading/saving the game
{
	// Properties to track player statistics
	public static float PlayerTotalGold =0f;
	public static float PlayerTotalKills = 0f;

	public static void UpdatePlayerRecords(float gold, float kills)
	{
		PlayerTotalGold += gold;
		PlayerTotalKills += kills;
		SaveLoadManager.SaveGame("TheOneAndOnlySave");
	}
}
