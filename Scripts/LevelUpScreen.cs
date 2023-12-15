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

	public void ShowMenu()
	{
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
	}
}
