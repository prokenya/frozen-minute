using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public partial class Gui : Control
{
	public Button play;
	public ColorRect transition;
	private Tween _tween;
	private PanelContainer menu;

	public Button exit;
	public override void _Ready()
	{
		Global.gui = this;
		play = GetNode<Button>("%Play");
		exit = GetNode<Button>("%Exit");
		menu = GetNode<PanelContainer>("%MainMenu");
		transition = GetNode<ColorRect>("%Transition");
	}

	public async void _Play(){
		play.Hide();
		exit.Show();
		menu.Hide();
		Global.Main.SpawnPlayer();
		await Global.GlobalNode.ChangeWorld("res://src/scenes/World1.tscn");
	}
	public async Task SetTransition(bool Start,double duration = 1){
		if (_tween != null){
        	_tween.Kill();
		}
   		_tween = CreateTween();
		if (Start){
			transition.Show();
			_tween.TweenProperty(transition,"color",new Color("#000000"),duration);
			await ToSignal(_tween, Tween.SignalName.Finished);
		}
		else{
			_tween.TweenProperty(transition,"color",new Color("#00000000"),duration);
			await ToSignal(_tween, Tween.SignalName.Finished);
			transition.Hide();
		}
	}

	public override void _Input(InputEvent inputEvent)
	{
	    if (Input.IsActionJustPressed("ui_cancel")){
			menu.Visible = !menu.Visible;
			Global.player.IsFreezed = menu.Visible;
		}
	}
	public async void _Exit(){
		await SetTransition(true);
		if (Global.IsGameOver){
			GetTree().Quit();
		}
		GetTree().ChangeSceneToFile("res://src/scenes/Main.tscn");
		Global.IsGameOver = true;
		await SetTransition(false);

	}

}
