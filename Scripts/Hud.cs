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

	async public void ShowGameOver()
	{
		ShowMessage("Game Over");

		var roundTime = GetNode<Timer>("MessageTimer");
		await ToSignal(roundTime, Timer.SignalName.Timeout);

		var message = GetNode<Label>("Message");
		message.Text = "Press the Start button to begin!";
		message.Show();

		await ToSignal(GetTree().CreateTimer(0.5), SceneTreeTimer.SignalName.Timeout);
		GetNode<Button>("Start").Show();
	}

	public void UpdateScore(int score)
	{
		GetNode<Label>("RoundTime").Text = score.ToString();
	}
	
	private void _on_message_timer_timeout()
	{
		GetNode<Label>("Message").Hide();
	}


	private void _on_start_pressed()
	{
		GetNode<Button>("Start").Hide();
		GetTree().Root.GetNode<Game>("Game").NewGame();
		
	}
}



