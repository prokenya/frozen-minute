using Godot;
using System;
using System.Collections.Generic;

public partial class Data : Node
{
    public Godot.Collections.Dictionary<int, bool> OpenedLevels = new Godot.Collections.Dictionary<int, bool>
{
    { 0, true }
};
    public Godot.Collections.Dictionary<int, float> AudioBusVolume = new Godot.Collections.Dictionary<int, float>
{
    { 1, 30.0f },
    { 2, 30.0f }

};
    private const string SavePath = "user://save.cfg";

    public void Save()
    {
        var config = new ConfigFile();

        foreach (var pair in OpenedLevels)
        {
                config.SetValue("OpenedLevels", pair.Key.ToString(), pair.Value);
        }
         foreach (var pair in AudioBusVolume)
        {
            config.SetValue("AudioBus", pair.Key.ToString(), pair.Value);
        }
        Error err = config.Save(SavePath);
        GD.Print(err == Error.Ok ? "Saved OK" : $"Save Error: {err}");
    }
    public void Load()
    {
        var config = new ConfigFile();

        Error err = config.Load(SavePath);
        if (err != Error.Ok)
        {
            GD.Print("Save file not found or load failed.");
            return;
        }
        // OpenedLevels.Clear();
        foreach (var key in config.GetSectionKeys("OpenedLevels"))
        {
            if (int.TryParse(key, out int level))
            {
                OpenedLevels[level] = (bool)config.GetValue("OpenedLevels", key);
            }
        }
        // AudioBusVolume.Clear();
        foreach (var key in config.GetSectionKeys("AudioBus"))
        {
            if (int.TryParse(key, out int busId))
            {
                AudioBusVolume[busId] = (float)config.GetValue("AudioBus", key);
            }
        }
        GD.Print("Loaded config:", AudioBusVolume, " AudioBus");
    }
}
