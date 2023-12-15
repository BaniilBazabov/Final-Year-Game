using Godot;
using System;
using System.Collections.Generic;

public partial class LevelUpScreen : Control
{
	Button upgradeButton1;
	Button upgradeButton2;
	Button upgradeButton3;
	Button upgradeButton4;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();
		upgradeButton1 = GetNode<Button>("/root/Game/Player/LevelUpMenu/LevelUpScreen/Panel/VBoxContainer/Skill1");
		upgradeButton2 = GetNode<Button>("/root/Game/Player/LevelUpMenu/LevelUpScreen/Panel/VBoxContainer/Skill2");
		upgradeButton3 = GetNode<Button>("/root/Game/Player/LevelUpMenu/LevelUpScreen/Panel/VBoxContainer/Skill3");
		upgradeButton4 = GetNode<Button>("/root/Game/Player/LevelUpMenu/LevelUpScreen/Panel/VBoxContainer/Skill4");

		upgradeButton1.Connect(Button.SignalName.Pressed, new Callable(this, "_onUpgradeButtonPressed"));
		upgradeButton2.Connect(Button.SignalName.Pressed, new Callable(this, "_onUpgradeButtonPressed"));
		upgradeButton3.Connect(Button.SignalName.Pressed, new Callable(this, "_onUpgradeButtonPressed"));
		upgradeButton4.Connect(Button.SignalName.Pressed, new Callable(this, "_onUpgradeButtonPressed"));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public class Upgrade
	{
		public string IconPath { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Tier { get; set; }
		public int MaxTier { get; set; }

		public Upgrade(string iconPath, string name, string description, int tier, int maxTier)
		{
			IconPath = iconPath;
			Name = name;
			Description = description;
			Tier = tier;
			MaxTier = maxTier;
		}
	}

	private List<Upgrade> allUpgrades = new List<Upgrade>
	{
		new Upgrade("res://Art/Skills/BonkHammer.png", "Bonking Hammer ", "Increases your damage from all sources by 10%", 1, 5),
		new Upgrade("res://Art/Skills/hairPreservedSprinkles.png", "Hair Preserved ", "Increases your HP by 10%", 1, 5),
		new Upgrade("res://icon.svg", "Placeholder 1", "ToBeReplaced 1", 1, 5),
		new Upgrade("res://icon.svg", "Placeholder 2", "ToBeReplaced 2", 1, 5),
		new Upgrade("res://icon.svg", "Placeholder 3", "ToBeReplaced 3", 1, 5),
		new Upgrade("res://icon.svg", "Placeholder 4", "ToBeReplaced 4", 1, 5),
	};

	private List<Upgrade> availableUpgrades = new List<Upgrade>();

	public void ShowMenu()
	{
		Randomize(allUpgrades);
		foreach (Upgrade upgrade in allUpgrades)
		{
			GD.Print(upgrade.Name);
		}
		
		// availableUpgrades = allUpgrades.GetRange(0, 3);
		// // Assign upgrades to buttons based on tier
		// foreach (Upgrade upgrade in allUpgrades)
		// {
		// 	// Check if the upgrade's tier is less than or equal to its max tier
		// 	if (upgrade.Tier <= upgrade.MaxTier)
		// 	{
		// 		// Assign the upgrade to the list of available upgrades
		// 		availableUpgrades.Add(upgrade);

		// 		// Increment the tier for the next availability
		// 		upgrade.Tier++;
		// 	}
		// }

		// // Assign upgrades to buttons
		// for (int i = 0; i < availableUpgrades.Count; i++)
		// {
		// 	Button button = GetNode<Button>($"UpgradeButton{i + 1}");
		// 	button.Text = availableUpgrades[i].Name;
		// 	// Set the icon and explanation text as needed
		// 	// button.Icon = LoadIcon(availableUpgrades[i].IconPath);
		// 	// button.ExplanationText = availableUpgrades[i].Description;
		// }

		Show();
	}

	public void HideMenu()
	{
		Hide();
	}

	private void _onUpgradeButtonPressed()
	{
		HideMenu();
	}

	public static List<T> Randomize<T>(List<T> list)
	{
		List<T> randomizedList = new List<T>();
		Random rnd = new Random();
		while (list.Count > 0)
		{
			int index = rnd.Next(0, list.Count); //pick a random item from the master list
			randomizedList.Add(list[index]); //place it at the end of the randomized list
			list.RemoveAt(index);
		}
		return randomizedList;
	}
}
