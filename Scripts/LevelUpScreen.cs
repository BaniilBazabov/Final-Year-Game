using Godot;
using System;
using System.Collections.Generic;

public partial class LevelUpScreen : Control
{
	Button upgradeButton1;
	Button upgradeButton2;
	Button upgradeButton3;
	Button upgradeButton4;

	private Player player;

	public void Initialize(Player player)
	{
		this.player = player;
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();
		upgradeButton1 = GetNode<Button>("/root/Game/Player/LevelUpMenu/LevelUpScreen/Panel/VBoxContainer/Skill1");
		upgradeButton2 = GetNode<Button>("/root/Game/Player/LevelUpMenu/LevelUpScreen/Panel/VBoxContainer/Skill2");
		upgradeButton3 = GetNode<Button>("/root/Game/Player/LevelUpMenu/LevelUpScreen/Panel/VBoxContainer/Skill3");
		upgradeButton4 = GetNode<Button>("/root/Game/Player/LevelUpMenu/LevelUpScreen/Panel/VBoxContainer/Skill4");

		upgradeButton1.Connect(Button.SignalName.Pressed, new Callable(this, MethodName._onUpgradeButtonPressed1));
		upgradeButton2.Connect(Button.SignalName.Pressed, new Callable(this, MethodName._onUpgradeButtonPressed2));
		upgradeButton3.Connect(Button.SignalName.Pressed, new Callable(this, MethodName._onUpgradeButtonPressed3));
		upgradeButton4.Connect(Button.SignalName.Pressed, new Callable(this, MethodName._onUpgradeButtonPressed4));

		player = GetTree().Root.GetNode<Player>("Game/Player");
		
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
		new Upgrade("res://Art/Skills/BonkHammer.png", "Bonking Hammer", "Increases your Damage from all sources by 10%", 0, 5),
		new Upgrade("res://Art/Skills/hairPreservedSprinkles.png", "Hair Preserved", "Increases your HP by 10%", 0, 5),
		new Upgrade("res://Art/Skills/BonkHammerAS.png", "Bonking Speed", "Increases your Attack Speed by 10%", 0, 5),
		new Upgrade("res://Art/Skills/GoldenHands.png", "Golden Hands", "Increases your Pick Up Range by 10%", 0, 5),
		new Upgrade("res://Art/Skills/SpeedyFit.png", "Fashion Statement", "Increases your Movement Speed by 10%", 0, 5),
		new Upgrade("res://Art/Skills/HairPreservedRegen.png", "Hair Regeneration", "Increases your HP Regeneration by 5", 0, 5),
		new Upgrade("res://Art/Skills/AreaOfAttack.png", "Bonking Area", "Increases your Attack Area of effect by 10%", 0, 5),
		new Upgrade("res://Art/Skills/AttackRange.png", "Bonking Range", "Increases your Attack Range by 15%", 0, 5),
	};

	private List<Upgrade> availableUpgrades = new List<Upgrade>();

	public void ShowMenu()
	{
		GetTree().Paused = true;
		allUpgrades = Randomize(allUpgrades);
		availableUpgrades.Clear();
		// Assign upgrades to buttons based on tier
		foreach (Upgrade upgrade in allUpgrades)
		{
			// Check if the upgrade's tier is less than or equal to its max tier
			if (upgrade.Tier < upgrade.MaxTier)
			{
				// Assign the upgrade to the list of available upgrades
				availableUpgrades.Add(upgrade);
			}
		}

		// Assign upgrades to buttons
		for (int i = 0; i < 4; i++)
		{
			Button button = GetNode<Button>($"/root/Game/Player/LevelUpMenu/LevelUpScreen/Panel/VBoxContainer/Skill{i + 1}");
			button.Text = $"{availableUpgrades[i].Name}\nTier: {availableUpgrades[i].Tier+1}\n{availableUpgrades[i].Description}";
			button.Icon = (Texture2D)GD.Load(availableUpgrades[i].IconPath);
		}

		Show();
	}

	public void HideMenu()
	{
		Hide();
	}

	private void _onUpgradeButtonPressed1()
	{
		Upgrade prevUpgrade = availableUpgrades[0];
		updateStats(prevUpgrade.Name);
		allUpgrades.Remove(prevUpgrade);
		prevUpgrade.Tier++;
		allUpgrades.Add(prevUpgrade);
		HideMenu();
		GetTree().Paused = false;
	}

	private void _onUpgradeButtonPressed2()
	{
		Upgrade prevUpgrade = availableUpgrades[1];
		updateStats(prevUpgrade.Name);
		allUpgrades.Remove(prevUpgrade);
		prevUpgrade.Tier++;
		allUpgrades.Add(prevUpgrade);
		HideMenu();
		GetTree().Paused = false;
	}

	private void _onUpgradeButtonPressed3()
	{
		Upgrade prevUpgrade = availableUpgrades[2];
		updateStats(prevUpgrade.Name);
		allUpgrades.Remove(prevUpgrade);
		prevUpgrade.Tier++;
		allUpgrades.Add(prevUpgrade);
		HideMenu();
		GetTree().Paused = false;
	}

	private void _onUpgradeButtonPressed4()
	{
		Upgrade prevUpgrade = availableUpgrades[3];
		updateStats(prevUpgrade.Name);
		allUpgrades.Remove(prevUpgrade);
		prevUpgrade.Tier++;
		allUpgrades.Add(prevUpgrade);
		HideMenu();
		GetTree().Paused = false;
	}

	private void updateStats(string nameOfUpgrade)
	{
		switch(nameOfUpgrade)
		{
			case "Bonking Hammer":
			player.damage *= 1.1f;
			break;

			case "Hair Preserved":
			player.IncreaseHP();
			break;

			case "Bonking Speed":
			player.DecreaseAttackCooldown(.25f);
			break;

			case "Fashion Statement":
			player.Speed *= 1.1f;
			break;

			case "Hair Regeneration":
			player.RegenAmount += 5f;
			break;

			case "Golden Hands":
			player.IncreasePickUpRange();
			break;

			case "Bonking Area":
			player.IncreaseAttackRadius();
			break;

			case "Bonking Range":
			player.AttackRange *= 1.15f;
			break;


			default:
			break;
		}
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
