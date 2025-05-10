using Godot;
using System;

public partial class Main : Node
{
	public Player player;
	private string playerpat = "res://src/scenes/Player.tscn";
	private bool _isFreezed;
	public bool IsFreezed
	{
	    get { return _isFreezed; }
	    set
	    {
	        _isFreezed = value;
			if (_isFreezed){ProcessMode = ProcessModeEnum.Disabled;}
			else{ProcessMode = ProcessModeEnum.Inherit;}
	    }
	}
	private Timer timer;
	public override void _Ready()
	{
		Global.Main = this;
		timer = GetNode<Timer>("%Timer");
		// SpawnPlayer();
	}
	public override void _PhysicsProcess(double delta)
    {
        Global.gui.SetTime(Math.Round(timer.TimeLeft));
    }
	public void SpawnPlayer(){
		player = (Player)ResourceLoader.Load<PackedScene>(playerpat).Instantiate();
		AddChild(player);
	}
	public void StartTimer(){
		timer.Start();
	}
	public void StopTimer(){
		timer.Stop();
	}

	private void _TimerTimeout(){
		Global.gui._Exit();
	}
}
