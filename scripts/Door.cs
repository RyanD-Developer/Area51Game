using Godot;
using System;

public partial class Door : Area2D
{
	[Export]
	private AnimationPlayer animationPlayerTop;
	[Export]
	private AnimationPlayer animationPlayerLeft;
	[Export]
	private AudioStreamPlayer audioPlayer;
	[Export]
	private AudioStream open;
	[Export]
	private AudioStream close;

	public void _on_area_2d_2_body_entered(Node body)
	{
		if (body.Name == "Player")
		{
			audioPlayer.Stream = open;
			audioPlayer.Play();
			animationPlayerTop.Play("Open");
		}
	}

	public void _on_area_2d_2_body_exited(Node body)
	{
		if (body.Name == "Player")
		{
			audioPlayer.Stream = close;
			audioPlayer.Play();
			animationPlayerTop.Play("Close");
		}
	}

	public void _on_area_2d_body_entered(Node body)
	{
		if (body.Name == "Player")
		{
			audioPlayer.Stream = open;
			audioPlayer.Play();
			animationPlayerLeft.Play("Open");
		}
	}

	public void _on_area_2d_body_exited(Node body)
	{
		if (body.Name == "Player")
		{
			audioPlayer.Stream = close;
			audioPlayer.Play();
			animationPlayerLeft.Play("Close");
		}
	}
}
