using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	private Control[] Images = new Control[6];
	
	[Export]
	private int speed = 50;

	private Vector2 currentVelocity;

	[Export]
	private AnimationPlayer animationPlayer;

	private string direction = "Down";

	public override void _Ready()
	{
		base._Ready();
		GetNode<Hp>("Sprite2D").Images = Images;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (currentVelocity.X == 0 && currentVelocity.Y == 0)
		{
			animationPlayer.Stop(true);
		}

		handleInput();

		Velocity = currentVelocity;
		MoveAndSlide();
		updateAnimation();
	}

	private void handleInput()
	{
		currentVelocity = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		currentVelocity *= speed;
	}

	private void updateAnimation()
	{
		if (Velocity.X != 0 || Velocity.Y != 0)
		{
			if (Velocity.X < 0)
			{
				if (Velocity.Y != 0)
				{
					if (Velocity.Y > 0)
					{
						direction = "Left_Down";
					}
					else
					{
						direction = "Left_Up";
					}
				}
				else
				{
					direction = "Left";
				}

			}
			else if (Velocity.X > 0)
			{
				if (Velocity.Y != 0)
				{
					if (Velocity.Y > 0)
					{
						direction = "Right_Down";
					}
					else
					{
						direction = "Right_Up";
					}
				}
				else
				{
					direction = "Right";
				}
			}
			else if (Velocity.X == 0 && Velocity.Y > 0)
			{
				direction = "Down";
			}
			else
			{
				direction = "Up";
			}
		}

		animationPlayer.Play("walk" + direction);
	}
}
