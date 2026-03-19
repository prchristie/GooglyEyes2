using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;

namespace GooglyEyes;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "GooglyEyes"; //At the moment, this is used only for the Logger and harmony names.

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(nameof(MainFile), MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        Harmony harmony = new Harmony(ModId);
        
        Logger.Info("Initializing GooglyEyes");

        harmony.PatchAll();
    }
}