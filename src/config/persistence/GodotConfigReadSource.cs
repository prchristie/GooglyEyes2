using System.Text.Json;
using GooglyEyes.config.persistence.models;

namespace GooglyEyes.config.persistence;

/*
 * Works for res:// and user:// paths
 **/
public class GodotConfigReadSource(string path) : IConfigReadSource
{
    private static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(nameof(GodotConfigReadSource), MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public EyeConfig Read()
    {
        using var file = Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.Read);
        Logger.Info($"Reading config from '{path}'");
        return JsonSerializer.Deserialize<EyeConfig>(file.GetAsText()) ?? new EyeConfig();
    }
}