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

    public override void _Ready()
    {
        Tree = GetTree();
        GlobalNode = this;
    }
    public async Task ChangeWorld(string scenePath)
    {

		IsGameOver = false;
        player.IsFreezed = true;
        await gui.SetTransition(true);
        await ToSignal(GetTree(), "process_frame");
        PackedScene scene = ResourceLoader.Load<PackedScene>(scenePath);
        World instance = (World)scene.Instantiate();
        if (CurrentWorld != null){CurrentWorld.QueueFree();}
        Main.AddChild(instance);
        player.IsFreezed = false;
        await gui.SetTransition(false);


    }


}  
