using Godot;
using System;

public partial class Main : Node
{
	private string playerpat = "res://src/scenes/Player.tscn";
	public Player player;
	public override void _Ready()
	{
		Global.Main = this;
	}
	public void SpawnPlayer(){
		player = (Player)ResourceLoader.Load<PackedScene>(playerpat).Instantiate();
		AddChild(player);
	}
}
