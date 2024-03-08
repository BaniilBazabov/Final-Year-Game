using Godot;
using System;

public partial class Hud : CanvasLayer
{	
	//controls the hud.
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



