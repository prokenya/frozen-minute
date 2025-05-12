using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public partial class Gui : Control
{
	public Label time;
	public Button play;
	public HBoxContainer Menus;
	private Godot.Collections.Dictionary<int, string> levelsPaths = new Godot.Collections.Dictionary<int, string>
{
    { 0, "res://src/scenes/worlds/World1.tscn" },
    { 1, "res://src/scenes/worlds/World2.tscn" },
	{ 2, "res://src/scenes/worlds/World3.tscn"}

};
	private ColorRect transition;
	private Button exit;
	private Tween _tween;
	private PanelContainer menu;
	private PanelContainer settings;
	private Button settingsButton;
	private Button CreditsButton;
	private PanelContainer Credits;
	public SelctLevel selectlevel;
	private int SFXBusIndex;
	private int MUSICBusIndex;

	private SpinBox musicSpin;
	private SpinBox SFXSpin;

	private HBoxContainer MobileControls;
	private VBoxContainer MobileUI;

	private HBoxContainer MobileUIButons;
	public override void _Ready()
	{
		Global.gui = this;
		play = GetNode<Button>("%Play");
		exit = GetNode<Button>("%Exit");
		menu = GetNode<PanelContainer>("%MainMenu");
		Menus = GetNode<HBoxContainer>("%Menus");
		MobileControls = GetNode<HBoxContainer>("%MobileControls");
		MobileUIButons = GetNode<HBoxContainer>("%MobileUIButons");
		MobileUI = GetNode<VBoxContainer>("%MobileUI");
		transition = GetNode<ColorRect>("%Transition");
		settings = GetNode<PanelContainer>("%Settings");
		settingsButton = GetNode<Button>("%SettingButtons");
		CreditsButton = GetNode<Button>("%CreditsButton");
		Credits = GetNode<PanelContainer>("%Credits");
		selectlevel = GetNode<SelctLevel>("%Selct Level");

		time = GetNode<Label>("%Time");

		SFXSpin = GetNode<SpinBox>("%SFXSpin");
		musicSpin = GetNode<SpinBox>("%musicSpin");
		SFXBusIndex = AudioServer.GetBusIndex("SFX");
		MUSICBusIndex = AudioServer.GetBusIndex("MUSIC");
		LoadAudioSettings(Global.data.AudioBusVolume);
		if (Global.platformName == "PC"){
			MobileUI.Visible = false;
		}
	}
	public void _Play(){
		_Play(0);
	}
	public async void _Play(int Index){
		// Global.Main.SpawnPlayer();
		await SetTransition(true,1);
		Global.player.Camera.Enabled = true;
		Menus.Hide();
		play.Hide();
		Global.gui.Menus.Hide();
		MobileControls.Show();
		await Global.GlobalNode.ChangeWorld(levelsPaths[Index],false);
		await SetTransition(false,1);
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
			Menus.Visible = !Menus.Visible;
			MobileControls.Visible = !MobileControls.Visible;
			Global.Main.IsFreezed = Menus.Visible;
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

	private void _SettingsPressed(){
		settings.Visible = !settings.Visible;
		selectlevel.Visible = !selectlevel.Visible;
		MobileUIButons.Visible = !settings.Visible;
		if (selectlevel.Visible){
			settingsButton.Text = ">Settings<";

		}
		else{
		settingsButton.Text = "<Settings>";}
		
	}
	private void _MusicValue(float Value){
		SetAudioVolume(MUSICBusIndex,Value);
	}
	private void _SfxValue(float Value){
		SetAudioVolume(SFXBusIndex,Value);
		
	}

	private void _Credits(){
		menu.Hide();
		settings.Hide();
		selectlevel.Hide();
		Credits.Show();
	}
	private void _Back(){
		menu.Show();
		settings.Hide();
		selectlevel.Hide();
		Credits.Hide();
	}
	private void SetAudioVolume(int busID, float Value) {
	    AudioServer.SetBusVolumeDb(busID, Mathf.LinearToDb(Value/15));
		Global.data.AudioBusVolume[busID] = Value;
		Global.data.Save();
	}
	public void LoadAudioSettings(Godot.Collections.Dictionary<int, float> dict){

		foreach(int busID in dict.Keys)
		{
			AudioServer.SetBusVolumeDb(busID, Mathf.LinearToDb(dict[busID]/15));

		}
		musicSpin.Value = dict[MUSICBusIndex];
		SFXSpin.Value = dict[SFXBusIndex];
	}

	public void SetTime(double timesec){
		time.Text = $"TimeLeft: {timesec}";
	}
}
