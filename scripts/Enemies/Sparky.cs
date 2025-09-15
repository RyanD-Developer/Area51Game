using Godot;
using System;

public partial class Sparky : CharacterBody2D
{
	[Export]
	private float speed = 25f;

	public Player player;
	public RoomArea roomArea;
	private Vector2 playerPos;
	public Vector2[] boundaries;
	public bool isOn = false;

	public override void _Ready()
	{
		base._Ready();
		if(player == null)
		{
			foreach (Node child in GetTree().GetRoot().GetChildren())
		{
			if (child is Player playerNode)
			{
				player = playerNode;
			}
		}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		playerPos = player.GlobalPosition;
		float differenceX = Math.Abs(playerPos.X) - Math.Abs(GlobalPosition.X);
		float differenceY = Math.Abs(playerPos.Y) - Math.Abs(GlobalPosition.Y);
		if(differenceX< 1f || differenceY < 1f)
		{
			if(differenceX - differenceY > 0.5f)
		{
			if (differenceX < differenceY)
			{
			if (Geometry2D.IsPointInPolygon(new Vector2(playerPos.X, GlobalPosition.Y), boundaries))
				{
					directionH();
				}
				else
				{
					directionV();
				}
			}
			else
			{
				if (Geometry2D.IsPointInPolygon(new Vector2(GlobalPosition.X, playerPos.Y), boundaries))
				{
					directionV();
				}
				else
				{
					directionH();
				}
			}
		}
		else
		{
			if (Geometry2D.IsPointInPolygon(new Vector2(GlobalPosition.X, playerPos.Y), boundaries))
				{
					directionV();
				}
				else
				{
					directionH();
				}
		}
		}
		else if(playerPos.X > GlobalPosition.X && playerPos.Y > GlobalPosition.Y)
		{
			if (Geometry2D.IsPointInPolygon(new Vector2(playerPos.X, GlobalPosition.Y), boundaries))
				{
					directionH();
				}
				else
				{
					directionV();
				}
		}
		else
		{
			Velocity = Vector2.Zero;
			GD.Print("Should not be called");
			//attack goes here
		}
		
		Velocity = Velocity *= speed;

		MoveAndSlide();
	}

	private void directionV()
	{
		if (playerPos.Y < GlobalPosition.Y)
		{
			Velocity = Vector2.Up;
		}
		else
		{
			Velocity = Vector2.Down;
		}
	}

	private void directionH()
	{
		if (playerPos.X < GlobalPosition.X)
		{
			Velocity = Vector2.Left;
		}
		else
		{
			Velocity = Vector2.Right;
		}
	}

}
