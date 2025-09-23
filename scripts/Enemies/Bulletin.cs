using Godot;
using System;
using System.Threading.Tasks;

public partial class Bulletin : CharacterBody2D
{
	[Export]
	private float speed = 25f;
	[Export]
	private AnimationPlayer animationPlayer;
	[Export]
	private AudioStreamPlayer audioPlayer;
	[Export]
	private AudioStream spin;
	[Export]
	private AudioStream hurt;

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
						animationPlayer.Play("attackDown");
						Attack(playerPos, "Down");
						didAttack = true;
					}
					else
					{
						animationPlayer.Play("attackUp");
						Attack(playerPos, "Up");
						didAttack = true;
					}
					Velocity = Vector2.Zero;
					attacking = true;
				}
				else if (playerPos.X > GlobalPosition.X - 0.5f && playerPos.X < GlobalPosition.X + 0.5f && !didAttack)
				{
					if (playerPos.Y > GlobalPosition.Y)
					{
						animationPlayer.Play("attackDown");
						Attack(playerPos, "Down");
						didAttack = true;
					}
					else
					{
						animationPlayer.Play("attackUp");
						Attack(playerPos, "Up");
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
					animationPlayer.Play("attackRight");
					Attack(playerPos, "Right");
					didAttack = true;
				}
				else
				{
					animationPlayer.Play("attackLeft");
					Attack(playerPos, "Left");
					didAttack = true;
				}
				Velocity = Vector2.Zero;
				attacking = true;
			}
			else if (GlobalPosition.X > playerPos.X - 0.5f && GlobalPosition.X < playerPos.X + 0.5f && !didAttack)
			{
				if (playerPos.Y > GlobalPosition.Y)
				{
					animationPlayer.Play("attackDown");
					Attack(playerPos, "Down");
					didAttack = true;
				}
				else
				{
					animationPlayer.Play("attackUp");
					Attack(playerPos, "Up");
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

		if (stuck && animationPlayer.CurrentAnimation != "attackStuck")
		{
			animationPlayer.Play("attackStuck");
			Velocity = Vector2.Zero;
		}

		MoveAndSlide();
	}

	private async void Attack(Vector2 position, string direction)
	{
		animationPlayer.Stop(false);
		animationPlayer.Play("attack" + direction);
		await Task.Delay(300);
		animationPlayer.Play("attack" + direction + "Loop");
		audioPlayer.Stream = spin;
		audioPlayer.Play();
		if (direction == "Left")
		{
			Velocity = Vector2.Left;
			speed = 50f;
		}
		else if (direction == "Right")
		{
			Velocity = Vector2.Right;
			speed = 50f;
		}
		else if (direction == "Up")
		{
			Velocity = Vector2.Up;
			speed = 50f;
		}
		else
		{
			Velocity = Vector2.Down;
			speed = 50f;
		}
		Velocity = Velocity *= speed;
		await Task.Delay(2000);
		attacking = false;
		didAttack = false;
		animationPlayer.Play("RESET");
	}

	private void directionV()
	{
		if (playerPos.Y < GlobalPosition.Y)
		{
			Velocity = Vector2.Up;
			speed = 12.5f;
			Velocity = Velocity *= speed;
			animationPlayer.Play("walkUp");
		}
		else
		{
			Velocity = Vector2.Down;
			speed = 12.5f;
			Velocity = Velocity *= speed;
			animationPlayer.Play("walkDown");
		}
	}

	private void directionH()
	{
		if (playerPos.X < GlobalPosition.X)
		{
			Velocity = Vector2.Left;
			speed = 12.5f;
			Velocity = Velocity *= speed;
			animationPlayer.Play("walkLeft");
		}
		else
		{
			Velocity = Vector2.Right;
			speed = 12.5f;
			Velocity = Velocity *= speed;
			animationPlayer.Play("walkRight");
		}
	}

	private void _on_bulletin_area_entered(Area2D area)
	{
		if (area.Name == "Tentacle" && !stuck)
		{
			audioPlayer.Stream = hurt;
			audioPlayer.Play();
			animationPlayer.Play("attackStuck");
			Velocity = Vector2.Zero;
			attacking = true;
			stuck = true;
		}
	}
}
