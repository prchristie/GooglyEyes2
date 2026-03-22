using System.Text.Json;
using Godot;
using GooglyEyes.config.persistence.models;

namespace GooglyEyes.config.persistence;

public class GodotUserConfigWriteSource(string path) : IConfigWriteSource
{
    public void Write(EyeConfig content)
    {
        var realPath = path.StartsWith("user://")
            ? ProjectSettings.GlobalizePath(path)
            : path;

        var dir = Path.GetDirectoryName(realPath);

        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        var json = JsonSerializer.Serialize(content);

        File.WriteAllText(realPath, json);
    }
}