using Godot;
using System;
using System.Threading.Tasks;

public partial class MainLevel : Node2D
{
	[Export]
	private Player player;
	
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		
		if(player.health.lives <= 0)
		{
			Tween tweenColorPrevious = GetTree().CreateTween();
			tweenColorPrevious.TweenProperty(this, "modulate", Color.Color8(255, 255, 255, 0), 1f);
			reloadScene();
		}
	}
	
	private async void reloadScene()
	{
		await Task.Delay(2500);
		GetTree().ReloadCurrentScene();
	}
}
