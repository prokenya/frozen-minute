using Godot;
using System;
using System.Threading.Tasks;

[GlobalClass]
public partial class Global : Node
{
    public static Global GlobalNode;
    public static bool IsGameOver = true;
    public SceneTree Tree;
    public static Main Main;

    public static World CurrentWorld;
    public static Player player;

    public static Gui gui;

    public static Data data = new Data();

    public static string platformName;

    public override void _Ready()
    {
        Tree = GetTree();
        GlobalNode = this;
        data.Load();
        platformName = getOSName();
    }
    public async Task ChangeWorld(string scenePath,bool WithTransition = true)
    {
        Main.StopTimer();
		IsGameOver = false;
        Main.IsFreezed = true;
        if (WithTransition){await gui.SetTransition(true);}
        await ToSignal(GetTree(), "process_frame");
        PackedScene scene = ResourceLoader.Load<PackedScene>(scenePath);
        World instance = (World)scene.Instantiate();
        if (CurrentWorld != null){CurrentWorld.QueueFree();}
        Main.AddChild(instance);
        player.Position = new Vector2(0,0);
        Main.IsFreezed = false;
        if (WithTransition){await gui.SetTransition(false);}



    }
    public string getOSName(){
    string platform = OS.GetName();
	if (platform == "Android"){
		GD.Print(platform);
    }
	else
        {
		platform = "PC";
		GD.Print("PC");
        }
    // platform = "Android";
    return platform;
    }

}  
