using Godot;

public partial class World : Node2D
{
	[Export]
	private bool StartTimerOnReady = true;
	private PackedScene icicleScene = ResourceLoader.Load<PackedScene>("res://src/scenes/Icicle.tscn");
	private PackedScene particleScene = ResourceLoader.Load<PackedScene>("res://src/scenes/particle.tscn");

	public override void _Ready()
	{
		Global.CurrentWorld = this;
		if (StartTimerOnReady)
		{
			Global.Main.StartTimer();
		}
		
	}
	public async void SpawnOBJ(int id,Vector2I Pos,int frame = -1){
		await ToSignal(GetTree(), "process_frame");
		if (id == 0){
			Icicle instace = (Icicle)icicleScene.Instantiate();
			instace.GlobalPosition = (Vector2)Pos;
			AddChild(instace);
			instace.setFrame(frame);
		}
		if (id == 1){
			Particle instace = (Particle)particleScene.Instantiate();
			instace.GlobalPosition = (Vector2)Pos;
			AddChild(instace);
		}
	}
}
