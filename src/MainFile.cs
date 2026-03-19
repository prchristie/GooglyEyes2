using Godot;
using GooglyEyes.config;
using GooglyEyes.config.persistence;
using GooglyEyes.config.persistence.models;
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
        string baseDir = Path.GetDirectoryName(OS.GetExecutablePath());
        string configPath = Path.Combine(baseDir, "mods", "GooglyEyes", "config", "config.json");

        JsonEyeConfigRepo jsonEyeConfigRepo = new JsonEyeConfigRepo(configPath);
        EyeConfigManager ecm = new EyeConfigManager(jsonEyeConfigRepo);
        jsonEyeConfigRepo.Save(new EyeConfig
        {
            CardDefinitions = new Dictionary<string, CardEyeConfig>
            {
                {
                    "BodySlam",
                    new CardEyeConfig
                    {
                        Name = "BodySlam",
                        EyeAnchors =
                        [
                            new()
                            {
                                OffsetX = 0,
                                OffsetY = 0,
                                RadiusPx = 20
                            }
                        ]
                    }
                }
            }
        });
        CardGooglyEyesPatch.Config = ecm.GetConfig();

        Logger.Info("Initializing GooglyEyes");

        Harmony harmony = new Harmony(ModId);
        harmony.PatchAll();
    }
}