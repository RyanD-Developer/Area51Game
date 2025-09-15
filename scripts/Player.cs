using Godot;
using System;
using System.Threading.Tasks;

public partial class Player : CharacterBody2D
{
	[Export]
	private Control[] Images = new Control[6];

	[Export]
	private int speed = 50;
	[Export]
	private AudioStreamPlayer audioPlayer;
	[Export]
	private AudioStream attack;

	private Vector2 currentVelocity;
	private bool isAttacking = false;
	private bool isInGround = false;
	private bool attackAnim = false;
	private bool didAttack = false;
	private bool mouseDown = false;
	[Export]
	private AnimationPlayer animationPlayer;
	[Export]
	public Hp health;

	private string direction = "Down";

	[Export]
	private Node2D attackParent;
	[Export]
	private AnimationPlayer attackAnimator;
	[Export]
	public RoomArea currentRoom;
	private Vector2 attackPos;
	private Vector2 mousePos;

	public override void _Ready()
	{
		base._Ready();
		GetNode<Hp>("Sprite2D").Images = Images;
		attackPos = attackParent.Position;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		handleInput();

		if (!isAttacking)
		{
			Velocity = currentVelocity;
			MoveAndSlide();
			updateAnimation();
		}
		else
		{
			if (isInGround)
			{
				handleAttacking();
			}
			else
			{
				if (!attackAnim)
				{
					AttackAnimation();
				}
			}
		}
	}

	private void handleAttacking()
	{
		if (mouseDown)
		{
			mousePos = ToGlobal(GetGlobalMousePosition()) - this.GlobalPosition;
			attackAnimator.Play("InMoving");
			Tween tween = GetTree().CreateTween();
			tween.TweenProperty(attackParent, "global_position", mousePos, 0.5);
		}
		else
		{
			AttackDelay();
		}
	}

	private async void AttackAnimation()
	{
		attackAnim = true;
		animationPlayer.Play("attack" + direction);
		audioPlayer.Stream = attack;
		audioPlayer.Play();
		await Task.Delay(850);
		mousePos = ToGlobal(GetViewport().GetMousePosition()) - this.GlobalPosition;
		attackAnimator.Play("InMoving");
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(attackParent, "global_position", mousePos, 0.5);
		animationPlayer.PlaybackActive = false;
		isInGround = true;
	}

	private async void AttackDelay()
	{
		if (!didAttack)
		{
			while (attackParent.GlobalPosition.X > mousePos.X + 1f && attackParent.GlobalPosition.X < mousePos.X - 1f && attackParent.GlobalPosition.Y > mousePos.Y + 1f && attackParent.GlobalPosition.Y < mousePos.Y - 1f)
			{
				attackAnimator.Play("InMoving");
				Tween tween = GetTree().CreateTween();
				tween.TweenProperty(attackParent, "global_position", mousePos, 0.5);
				await Task.Delay(750);
				if (attackParent.GlobalPosition.X < mousePos.X + 1f && attackParent.GlobalPosition.X > mousePos.X - 1f && attackParent.GlobalPosition.Y < mousePos.Y + 1f && attackParent.GlobalPosition.Y > mousePos.Y - 1f)
				{
					tween.Kill();
				}
			}
			if (attackParent.GlobalPosition.X < mousePos.X + 1f && attackParent.GlobalPosition.X > mousePos.X - 1f && attackParent.GlobalPosition.Y < mousePos.Y + 1f && attackParent.GlobalPosition.Y > mousePos.Y - 1f)
			{
				attackAnimator.Play("Attacking");
				attackParent.GlobalPosition = mousePos;
				await Task.Delay(800);
				didAttack = true;
			}
		}
		if (didAttack)
		{
			attackParent.Position = attackPos;
			attackAnimator.Play("Gone");
			animationPlayer.PlaybackActive = true;
			await Task.Delay(600);
			isInGround = false;
			isAttacking = false;
			didAttack = false;
			attackAnim = false;
		}
	}

	private void handleInput()
	{
		currentVelocity = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		currentVelocity *= speed;
		mouseDown = Input.IsActionPressed("ui_accept");
		if (!isAttacking)
		{
			if (mouseDown)
			{
				isAttacking = true;
			}
		}
	}

	private void updateAnimation()
	{
		if (currentVelocity.X == 0 && currentVelocity.Y == 0)
		{
			animationPlayer.Stop(true);
		}
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
