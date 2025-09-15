using Godot;
using System;
using Godot.Collections;

public partial class Hp : Node
{
	public int lives = 3;

	public Control[] Images = new Control[6];
	
	private void _on_player_area_entered(Area2D Area){
		if(Area.Name == "Flame"){
			if(lives > 0){
				Images[lives - 1].Visible = false;
				lives -= 1;
			}
		}
	}
	
}
