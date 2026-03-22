using System.Text.Json;
using Godot;
using GooglyEyes.config.persistence.models;

namespace GooglyEyes.config.persistence;

/*
 * Works for res:// and user:// paths
**/
public class GodotConfigReadSource(string path) : IConfigReadSource
{
    public EyeConfig Read()
    {
        using var file = Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.Read);
        return JsonSerializer.Deserialize<EyeConfig>(file.GetAsText()) ?? new EyeConfig();
    }
}