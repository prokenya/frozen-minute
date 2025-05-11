using Godot;
using System;

public partial class End : Area2D
{
	[Export]
	private int LevelIndex = 0;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void _Entered(Node2D body){
		if (body.IsInGroup("player")){
		GD.Print("next level");
		if (LevelIndex == -1){
			Global.gui._Exit();
		}
		else
		{
			Global.data.OpenedLevels[LevelIndex] = true;
			Global.data.Save();
			Global.gui._Play(LevelIndex);
			Global.gui.selectlevel.UpdateButtons();
		}
		}
	}

}
