using Godot;
using System;
using System.Threading.Tasks;

public partial class Sparky : CharacterBody2D
{
	[Export]
	private float speed = 25f;
	private AnimationPlayer animationPlayer;
	private AnimationPlayer flameAnimator;
	[Export]
	private Node2D flamePos;
	private AudioStreamPlayer audioPlayer;
	[Export]
	private AudioStream flamethrower;
	[Export]
	private AudioStream hurt;


	private string direction;
	public Player player;
	public RoomArea roomArea;
	private Vector2 playerPos;
	public Vector2[] boundaries;
	public bool isOn = false;
	private bool attacking = false;
	private bool didAttack = false;
	private bool stuck = false;

	public override void _Ready()
	{
		base._Ready();
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		flameAnimator = GetNode<AnimationPlayer>("FireAnimator");
		audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		if (player == null)
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

		if (isOn && !stuck)
		{
			playerPos = player.GlobalPosition;
			float differenceX = Math.Abs(playerPos.X) - Math.Abs(GlobalPosition.X);
			float differenceY = Math.Abs(playerPos.Y) - Math.Abs(GlobalPosition.Y);
			if (!attacking)
			{
				if (differenceX < 5f || differenceY < 5f)
				{

					if (differenceX - differenceY > 0.5f)
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
				else if (playerPos.X > GlobalPosition.X && playerPos.Y > GlobalPosition.Y)
				{
					directionH();
				}
				else if (GlobalPosition.X > playerPos.X - 5f && GlobalPosition.X < playerPos.X + 5f && !didAttack)
				{
					if (playerPos.Y > GlobalPosition.Y)
					{
						CallDeferred("PlayAttackAnimation", "Down");
						direction = "Down";
						Attack();
						didAttack = true;
					}
					else
					{
						CallDeferred("PlayAttackAnimation", "Up");
						direction = "Up";
						Attack();
						didAttack = true;
					}
					Velocity = Vector2.Zero;
					attacking = true;
				}
				else if (playerPos.X > GlobalPosition.X - 0.5f && playerPos.X < GlobalPosition.X + 0.5f && !didAttack)
				{
					if (playerPos.Y > GlobalPosition.Y)
					{
						CallDeferred("PlayAttackAnimation", "Down");
						direction = "Down";
						Attack();
						didAttack = true;
					}
					else
					{
						CallDeferred("PlayAttackAnimation", "Up");
						direction = "Up";
						Attack();
						didAttack = true;
					}
					Velocity = Vector2.Zero;
					attacking = true;
				}
				else if (playerPos.X > GlobalPosition.X && playerPos.Y > GlobalPosition.Y)
				{
					directionH();
					if (GlobalPosition.X > playerPos.X - 5f && GlobalPosition.X < playerPos.X + 5f && !didAttack)
					{
						animationPlayer.Stop(true);
						Velocity = Vector2.Zero;
						attacking = true;
					}
					else if (playerPos.X > GlobalPosition.X - 0.5f && playerPos.X < GlobalPosition.X + 0.5f && !didAttack)
					{
						animationPlayer.Stop(true);
						Velocity = Vector2.Zero;
						attacking = true;
					}
				}
			}

			if (GlobalPosition.Y > playerPos.Y - 0.5f && GlobalPosition.Y < playerPos.Y + 0.5f && !didAttack)
			{
				if (playerPos.X > GlobalPosition.X)
				{
					CallDeferred("PlayAttackAnimation", "Right");
					direction = "Right";
					Attack();
					didAttack = true;
				}
				else
				{
					CallDeferred("PlayAttackAnimation", "Left");
					direction = "Left";
					Attack();
					didAttack = true;
				}
				Velocity = Vector2.Zero;
				attacking = true;
			}
			else if (GlobalPosition.X > playerPos.X - 0.5f && GlobalPosition.X < playerPos.X + 0.5f && !didAttack)
			{
				if (playerPos.Y > GlobalPosition.Y)
				{
					CallDeferred("PlayAttackAnimation", "Down");
					direction = "Down";
					Attack();
					didAttack = true;
				}
				else
				{
					CallDeferred("PlayAttackAnimation", "Up");
					direction = "Up";
					Attack();
					didAttack = true;
				}
				Velocity = Vector2.Zero;
				attacking = true;
			}
			else if (playerPos.X > GlobalPosition.X && playerPos.Y > GlobalPosition.Y && !attacking && !didAttack)
			{
				directionH();
			}
			else if (GlobalPosition.Y > playerPos.Y - 0.5f && GlobalPosition.Y < playerPos.Y + 0.5f && didAttack && !attacking)
			{
				animationPlayer.Stop(true);
				Velocity = Vector2.Zero;
			}
			else if (GlobalPosition.X > playerPos.X - 0.5f && GlobalPosition.X < playerPos.X + 0.5f && didAttack && !attacking)
			{
				animationPlayer.Stop(true);
				Velocity = Vector2.Zero;
			}

		}

		Velocity = Velocity *= speed;

		MoveAndSlide();
	}

	private async void Attack()
	{
		await Task.Delay(200);
		audioPlayer.Stream = flamethrower;
		audioPlayer.Play();
		CallDeferred("PlayFlameAttackAnimation", direction);
		flameAnimator.Play(animationPlayer.CurrentAnimation);
		await Task.Delay(600);
		if (flameAnimator.CurrentAnimation.Length > 10)
		{
			CallDeferred("PlayFlameLoopAnimation", direction);
		}
		await Task.Delay(1400);
		CallDeferred("PlayFlameResetAnimation");
		attacking = false;
		await Task.Delay(2500);
		didAttack = false;
	}

	private void directionV()
	{
		if (playerPos.Y < GlobalPosition.Y)
		{
			Velocity = Vector2.Up;
			animationPlayer.Play("walkUp");
		}
		else
		{
			Velocity = Vector2.Down;
			animationPlayer.Play("walkDown");
		}
	}

	private void directionH()
	{
		if (playerPos.X < GlobalPosition.X)
		{
			Velocity = Vector2.Left;
			animationPlayer.Play("walkLeft");
		}
		else
		{
			Velocity = Vector2.Right;
			animationPlayer.Play("walkRight");
		}
	}

	private void _on_sparky_area_entered(Area2D area)
	{
		if (area.Name == "Tentacle" && !stuck)
		{
			audioPlayer.Stream = hurt;
			audioPlayer.Play();
			flamePos.Position = new Vector2(flamePos.Position.X, flamePos.Position.Y + 7.5f);
			Velocity = Vector2.Zero;
			animationPlayer.Stop(true);
			stuck = true;
			float differenceX = Math.Abs(playerPos.X) - Math.Abs(GlobalPosition.X);
			float differenceY = Math.Abs(playerPos.Y) - Math.Abs(GlobalPosition.Y);
			if (differenceX < differenceY)
			{
				if (playerPos.X > GlobalPosition.X)
				{
					CallDeferred(nameof(PlayStuckAnimation), "Right");

					AttackStuck("Right");
				}
				else
				{
					CallDeferred(nameof(PlayStuckAnimation), "Left");
					AttackStuck("Left");
				}
				Velocity = Vector2.Zero;
			}
			else
			{
				Velocity = Vector2.Zero;
				animationPlayer.Stop(true);
				stuck = true;
				if (playerPos.Y > GlobalPosition.Y)
				{
					CallDeferred(nameof(PlayStuckAnimation), "Down");
					AttackStuck("Down");
				}
				else
				{
					CallDeferred(nameof(PlayStuckAnimation), "Up");
					AttackStuck("Up");
				}
				Velocity = Vector2.Zero;
			}
		}
	}
	private async void AttackStuck(string direction)
	{
		while (stuck)
		{
			if (isOn)
			{
				audioPlayer.Stream = flamethrower;
				audioPlayer.Play();
				if (!IsInsideTree()) break;
				CallDeferred(nameof(PlayFlameAttackAnimation), direction);
				await Task.Delay(600);
				if (!IsInsideTree()) break;
				CallDeferred(nameof(PlayFlameLoopAnimation), direction);

				await Task.Delay(2500);
				if (!IsInsideTree()) break;
				CallDeferred(nameof(PlayFlameResetAnimation));
				await Task.Delay(2500);
			}
		}
	}

	private void PlayAttackAnimation(string direction)
	{
		animationPlayer.Play("attack" + direction);
	}

	private void PlayStuckAnimation(string direction)
	{
		animationPlayer.Play("stuck" + direction);
	}

	private void PlayLoopAnimation(string direction)
	{
		animationPlayer.Play("attack" + direction + "Loop");
	}

	private void PlayResetAnimation()
	{
		animationPlayer.Play("RESET");
	}

	private void PlayFlameAttackAnimation(string direction)
	{
		flameAnimator.Play("attack" + direction);
	}

	private void PlayFlameLoopAnimation(string direction)
	{
		flameAnimator.Play("attack" + direction + "Loop");
	}

	private void PlayFlameResetAnimation()
	{
		flameAnimator.Play("RESET");
	}
}
