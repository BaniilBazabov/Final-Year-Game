using Godot;
using System;

public partial class Hud : CanvasLayer
{	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void ShowMessage(string text)
	{
		var message = GetNode<Label>("Message");
		message.Text = text;
		message.Show();

		GetNode<Timer>("MessageTimer").Start();
	}

	public void ShowGameOver()
	{
		GetTree().ChangeSceneToFile("res://Scripts/deathScreen.tscn");
	}

	public void UpdateScore(int score)
	{
		GetNode<Label>("RoundTime").Text = score.ToString();
	}
	
	private void _on_message_timer_timeout()
	{
		GetNode<Label>("Message").Hide();
	}
}



