using Godot;
using System;
using System.Runtime.InteropServices;

public partial class World : Node2D
{
	[Export]
	private bool StartTimerOnReady = true;
	private PackedScene icicleScene = ResourceLoader.Load<PackedScene>("res://src/scenes/Icicle.tscn");
	public override void _Ready()
	{
		Global.CurrentWorld = this;
		if (StartTimerOnReady)
		{
			Global.Main.StartTimer();
		}
		
	}
	public async void SpawnIcicle(Vector2I Pos,int frame){
		await ToSignal(GetTree(), "process_frame");
		Icicle instace = (Icicle)icicleScene.Instantiate();
		instace.GlobalPosition = (Vector2)Pos;
		AddChild(instace);
		instace.setFrame(frame);
	}
}
