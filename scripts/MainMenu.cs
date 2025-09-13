using Godot;
using System;

public partial class MainMenu : Control
{
	private void _on_start_pressed()
	{
		GetTree().ChangeSceneToFile("res://scenes//main_level.tscn");
	}

	private void _on_settings_pressed()
	{

	}

	private void _on_exit_pressed()
	{
		GetTree().Quit();
	}
}
