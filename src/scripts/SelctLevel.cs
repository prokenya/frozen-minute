using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class SelctLevel : PanelContainer
{	
	public int LevelCount = 3;
	private GridContainer levels;

	private Godot.Collections.Array<Button> buttons = new Godot.Collections.Array<Button>();
	public override void _Ready()
	{
		levels = GetNode<GridContainer>("%levels");
        for (int i = 0; i < LevelCount; i++)
        {
			Button button = new Button();
			button.Text = $"Lvel: {i+1}";
			button.SizeFlagsHorizontal = SizeFlags.Expand | SizeFlags.ShrinkCenter;

			button.SetMeta("level_id", i);

			button.Pressed += () => OnLevelButtonPressed(button);
			levels.AddChild(button);
			buttons.Add(button);
		}
		UpdateButtons();
	}

	public void OnLevelButtonPressed(Button button){
		int levelId = (int)button.GetMeta("level_id");
		GD.Print($"Pressed level {levelId}");
		Global.gui._Play(levelId);

	}
	public void UpdateButtons()
	{
		foreach (Button button in buttons)
		{
			int levelId = (int)button.GetMeta("level_id");

        	if (!Global.data.OpenedLevels.ContainsKey(levelId))
        	{
        	    button.Disabled = true;
        	}
        	else
        	{
        	    button.Disabled = false;
        	}
		}
	}

}
