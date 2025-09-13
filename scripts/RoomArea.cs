using Godot;
using System.Collections.Generic;
using System;

public partial class RoomArea : Area2D
{
	[Export]
	private Vector2 cameraTargetPosition;

	private static RoomArea current_collision;
	private RoomArea previous_collision;

	public override void _Ready()
	{
		base._Ready();

		var overlappingBodies = GetOverlappingBodies();

		foreach (Node body in overlappingBodies)
		{
			if (body.Name == "Player")
			{
				current_collision = this;
				current_collision.GetParent<Sprite2D>().Modulate = Color.Color8(255, 255, 255, 255);
			}
		}
	}


	public void _on_body_entered(Node body)
	{
		if (body.Name == "Player")
		{
			previous_collision = current_collision;
			current_collision = this;
			if (previous_collision != null)
			{
				previous_collision.GetParent<Sprite2D>().Modulate = Color.Color8(255, 255, 255, 0);
			}
			current_collision.GetParent<Sprite2D>().Modulate = Color.Color8(255, 255, 255, 255);
			GetViewport().GetCamera2D().Position = new Vector2(current_collision.GlobalPosition.X, current_collision.GlobalPosition.Y - 20f);
		}
	}
	public void _on_body_exited(Node Body)
	{
		if (current_collision == this)
		{
			if (previous_collision != null)
			{
				current_collision = previous_collision;
			}
			previous_collision = this;
			if (previous_collision != null)
			{
				previous_collision.GetParent<Sprite2D>().Modulate = Color.Color8(255, 255, 255, 0);
			}
			current_collision.GetParent<Sprite2D>().Modulate = Color.Color8(255, 255, 255, 255);
			GetViewport().GetCamera2D().Position = new Vector2(current_collision.GlobalPosition.X, current_collision.GlobalPosition.Y - 20f);
		}
	}

	public void _on_area_entered(Area2D body)
	{
		if (body.Name == "Player")
		{
			previous_collision = current_collision;
			current_collision = this;
			if (previous_collision != null)
			{
				previous_collision.GetParent<Sprite2D>().Modulate = Color.Color8(255, 255, 255, 0);
			}
			current_collision.GetParent<Sprite2D>().Modulate = Color.Color8(255, 255, 255, 255);
			GetViewport().GetCamera2D().Position = new Vector2(current_collision.GlobalPosition.X, current_collision.GlobalPosition.Y - 20f);
		}
	}
}
