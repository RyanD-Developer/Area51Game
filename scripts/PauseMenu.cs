using Godot;

public partial class PauseMenu : CanvasLayer
{
	[Export]
	private CanvasLayer _pauseMenu;

	public override void _Ready()
	{
		_pauseMenu.Visible = false;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel"))
		{
			GetTree().Paused = !GetTree().Paused;
			_pauseMenu.Visible = !_pauseMenu.Visible;
		}
	}

	private void OnResumeButtonPressed()
	{
		GetTree().Paused = !GetTree().Paused;
		_pauseMenu.Visible = !_pauseMenu.Visible;
	}

	private void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
}
