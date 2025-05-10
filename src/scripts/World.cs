using Godot;
using System;

public partial class World : Node2D
{
	public override void _Ready()
	{
		Global.CurrentWorld = this;
		Global.Main.StartTimer();
		
	}
}
