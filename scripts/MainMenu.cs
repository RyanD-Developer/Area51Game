using Godot;
using System;

public partial class MainMenu : Control
{
	[Export]
	private string pathToScene;
	[Export]
	private AudioStream audioStream;
	[Export]
	private AudioStreamPlayer streamPlayer;
	public override void _Ready()
	{
		base._Ready();
		Input.MouseMode = Input.MouseModeEnum.Confined;
		if(pathToScene == null)
		{
			pathToScene = "res://scenes//main_level.tscn";
		}
	}
	
	private void _on_start_pressed()
	{
		streamPlayer.Stop();
		streamPlayer.Stream = audioStream;
		GetTree().ChangeSceneToFile(pathToScene);
	}

	private void _on_settings_pressed()
	{

	}

	private void _on_exit_pressed()
	{
		GetTree().Quit();
	}
}
