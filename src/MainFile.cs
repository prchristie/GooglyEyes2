using Godot;
using GooglyEyes.config;
using GooglyEyes.config.persistence;
using GooglyEyes.config.persistence.models;
using HarmonyLib;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;

namespace GooglyEyes;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    private const string ModId = "GooglyEyes";

    private static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(nameof(MainFile), MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        var pckConfigPath = "res://config/config.json";
        var userConfigPath = "user://GooglyEyes/config/config.json";

        GodotConfigReadSource primaryConfigSource = new(pckConfigPath);
        GodotConfigReadSource secondaryConfigSource = new(userConfigPath);
        FallbackConfigReadSource fallbackConfigReadSource = new(primaryConfigSource, secondaryConfigSource);
        GodotUserConfigWriteSource write = new(userConfigPath);
        EyeConfigManager ecm = new EyeConfigManager(write, fallbackConfigReadSource);
        CardGooglyEyesPatch.Config = ecm.LoadConfig();

        Harmony harmony = new Harmony(ModId);
        harmony.PatchAll();
    }
}