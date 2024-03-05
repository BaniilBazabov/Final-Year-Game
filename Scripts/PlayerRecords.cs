using Godot;

public static class PlayerRecords
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
