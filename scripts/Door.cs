using Godot;
using System;

public partial class Door : Area2D
{
	[Export]
	private AnimationPlayer animationPlayerTop;
	[Export]
	private AnimationPlayer animationPlayerLeft;

	public void _on_area_2d_2_body_entered(Node body)
	{
		if(body.Name == "Player")
		{
			GD.Print("success");
			animationPlayerTop.Play("Open");
		}
	}
	
	public void _on_area_2d_2_body_exited(Node body)
	{
		if(body.Name == "Player")
		{
			GD.Print("success");
			animationPlayerTop.Play("Close");
		}
	}
	
	public void _on_area_2d_body_entered(Node body)
	{
		if(body.Name == "Player")
		{
			GD.Print("success");
			animationPlayerLeft.Play("Open");
		}
	}
	
	public void _on_area_2d_body_exited(Node body)
	{
		if(body.Name == "Player")
		{
			GD.Print("success");
			animationPlayerLeft.Play("Close");
		}
	}
}
