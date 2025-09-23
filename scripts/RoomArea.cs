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
	[Export]
	private bool winningRoom;

	private static RoomArea current_collision;
	private RoomArea previous_collision;
	[Export]
	public Sprite2D sprite;
	private Camera2D camera;
	private List<Sparky> sparkies = new List<Sparky>();
	private List<Bulletin> bulletins = new List<Bulletin>();

	public override void _Ready()
	{
		base._Ready();

		var overlappingBodies = GetOverlappingBodies();

		if(playerNode.currentRoom == this)
		{
			current_collision = this;
			current_collision.Modulate = Color.Color8(255, 255, 255, 255);
		}
		foreach (Node child in GetChildren())
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
		foreach (Node child in GetChildren())
		{
			if (child is Bulletin bulletinNode)
			{
				bulletins.Add(bulletinNode);
			}
		}
		foreach (Bulletin bulletin in bulletins)
		{
			bulletin.boundaries = areaShape.GetPolygon();
			bulletin.player = playerNode;
		}
		cameraTargetPosition = new Vector2(this.GlobalPosition.X + 160, this.GlobalPosition.Y + 85f);
		camera = GetViewport().GetCamera2D();
	}


	public void _on_body_entered(Node body)
	{
		if (body.Name == "Player")
		{
			if(winningRoom)
			{
				GetTree().ChangeSceneToFile("res://scenes//win_screen.tscn");
			}
			previous_collision = current_collision;
			current_collision = this;
			if (previous_collision != null)
			{
				//previous_collision.GetParent<Sprite2D>().Modulate = Color.Color8(255, 255, 255, 0);
				Tween tweenColorPrevious = GetTree().CreateTween();
				tweenColorPrevious.TweenProperty(previous_collision, "modulate", Color.Color8(255, 255, 255, 0), transitionSpeed);
			}
			//current_collision.GetParent<Sprite2D>().Modulate = Color.Color8(255, 255, 255, 255);
			Tween tweenColor = GetTree().CreateTween();
			tweenColor.TweenProperty(current_collision, "modulate", Color.Color8(255, 255, 255, 255), transitionSpeed);
			//camera.Position = new Vector2(current_collision.GlobalPosition.X, current_collision.GlobalPosition.Y - 20f);
			Tween tween = GetTree().CreateTween();
			tween.TweenProperty(camera, "position", cameraTargetPosition, transitionSpeed);
			foreach (Sparky sparky in sparkies)
			{
				sparky.isOn = true;
			}
			foreach (Bulletin bulletin in bulletins)
			{
				bulletin.isOn = true;
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
				tweenColorPrevious.TweenProperty(previous_collision, "modulate", Color.Color8(255, 255, 255, 0), transitionSpeed);
			}
			Tween tweenColor = GetTree().CreateTween();
			tweenColor.TweenProperty(current_collision, "modulate", Color.Color8(255, 255, 255, 255), transitionSpeed);
			Tween tween = GetTree().CreateTween();
			tween.TweenProperty(camera, "position", cameraTargetPosition, transitionSpeed);
		}
		if (body.Name == "Player")
		{
			foreach (Sparky sparky in sparkies)
			{
				sparky.isOn = false;
			}
			foreach (Bulletin bulletin in bulletins)
			{
				bulletin.isOn = false;
			}
		}
	}
}
