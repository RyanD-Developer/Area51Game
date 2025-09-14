using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	private Control[] Images = new Control[6];

	[Export]
	private int speed = 50;

	private Vector2 currentVelocity;
	private bool isAttacking;
	private bool mouseDown;
	private bool isOverlapped = false;
	[Export]
	private AnimationPlayer animationPlayer;

	private string direction = "Down";

	[Export]
	private Node2D attackParent;
	[Export]
	private AnimationPlayer attackAnimator;

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

		if (!isAttacking)
		{
			Velocity = currentVelocity;
			MoveAndSlide();
			updateAnimation();
		}
		else
		{
			handleAttacking();
		}
	}

	private void handleAttacking()
	{
		if (mouseDown)
		{
			if (!isOverlapped)
			{
				Tween tween;
				tween.TweenProperty(attackParent, "position", GetViewport().GetMousePosition(), 0.75);
			}
			else
			{
				attackParent.position = new Vector2(0f,0f);
				//GetViewport().GetMousePosition();
			}
			if (attackParent.position == GetViewport().GetMousePosition())
			{
				isOverlapped = false;
			}
		}
		else
		{

		}
	}

	private void handleInput()
	{
		currentVelocity = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		currentVelocity *= speed;
		mouseDown = Input.IsActionPressed("ui_accept");
		if (mouseDown)
		{
			isAttacking = true;
		}
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
