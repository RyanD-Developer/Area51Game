using Godot;
using System.Collections.Generic;
using System;

public partial class RoomArea : Area2D
{
	[Export]
	private Vector2 cameraTargetPosition;
	[Export]
	private float transitionSpeed = 2.0f;
	[Export]
	private CollisionPolygon2D areaShape;
	[Export]
	private Player playerNode;

	private static RoomArea current_collision;
	private RoomArea previous_collision;
	private Camera2D camera;
	private List<Sparky> sparkies = new List<Sparky>();

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
		foreach (Node child in GetParent().GetChildren())
		{
			if (child is Sparky sparkyNode)
			{
				sparkies.Add(sparkyNode);
			}
		}
		foreach (Sparky sparky in sparkies)
		{
			sparky.boundaries = areaShape.GetPolygon();
			sparky.player = playerNode;
		}
		cameraTargetPosition = new Vector2(this.GlobalPosition.X, this.GlobalPosition.Y - 20f);
		camera = GetViewport().GetCamera2D();
	}


	public void _on_body_entered(Node body)
	{
		if (body.Name == "Player")
		{
			previous_collision = current_collision;
			current_collision = this;
			if (previous_collision != null)
			{
				//previous_collision.GetParent<Sprite2D>().Modulate = Color.Color8(255, 255, 255, 0);
				Tween tweenColorPrevious = GetTree().CreateTween();
				tweenColorPrevious.TweenProperty(previous_collision.GetParent<Sprite2D>(), "modulate", Color.Color8(255, 255, 255, 0), transitionSpeed);
			}
			//current_collision.GetParent<Sprite2D>().Modulate = Color.Color8(255, 255, 255, 255);
			Tween tweenColor = GetTree().CreateTween();
			tweenColor.TweenProperty(current_collision.GetParent<Sprite2D>(), "modulate", Color.Color8(255, 255, 255, 255), transitionSpeed);
			//camera.Position = new Vector2(current_collision.GlobalPosition.X, current_collision.GlobalPosition.Y - 20f);
			Tween tween = GetTree().CreateTween();
			tween.TweenProperty(camera, "position", cameraTargetPosition, transitionSpeed);
			foreach(Sparky sparky in sparkies)
			{
				sparky.isOn = true;
			}
		}
	}
	public void _on_body_exited(Node body)
	{
		if (current_collision == this && body.Name == "Player")
		{
			if (previous_collision != null)
			{
				current_collision = previous_collision;
			}
			previous_collision = this;
			if (previous_collision != null)
			{
				Tween tweenColorPrevious = GetTree().CreateTween();
				tweenColorPrevious.TweenProperty(previous_collision.GetParent<Sprite2D>(), "modulate", Color.Color8(255, 255, 255, 0), transitionSpeed);
			}
			Tween tweenColor = GetTree().CreateTween();
			tweenColor.TweenProperty(current_collision.GetParent<Sprite2D>(), "modulate", Color.Color8(255, 255, 255, 255), transitionSpeed);
			Tween tween = GetTree().CreateTween();
			tween.TweenProperty(camera, "position", cameraTargetPosition, transitionSpeed);
		}
		if(body.Name == "Player"){
		foreach(Sparky sparky in sparkies)
		{
			sparky.isOn = false;
		}
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
				//previous_collision.GetParent<Sprite2D>().Modulate = Color.Color8(255, 255, 255, 0);
				Tween tweenColorPrevious = GetTree().CreateTween();
				tweenColorPrevious.TweenProperty(previous_collision.GetParent<Sprite2D>(), "modulate", Color.Color8(255, 255, 255, 0), transitionSpeed);
			}
			//current_collision.GetParent<Sprite2D>().Modulate = Color.Color8(255, 255, 255, 255);
			Tween tweenColor = GetTree().CreateTween();
			tweenColor.TweenProperty(current_collision.GetParent<Sprite2D>(), "modulate", Color.Color8(255, 255, 255, 255), transitionSpeed);
			//camera.Position = new Vector2(current_collision.GlobalPosition.X, current_collision.GlobalPosition.Y - 20f);
			Tween tween = GetTree().CreateTween();
			tween.TweenProperty(camera, "position", cameraTargetPosition, transitionSpeed);
		}
	}
}
