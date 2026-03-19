using GooglyEyes.config.persistence.models;
using GooglyEyes.GooglyEyes;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace GooglyEyes;

using Godot;
using HarmonyLib;

[HarmonyPatch(typeof(NCard), "_Ready")]
public static class CardGooglyEyesPatch
{
    public static EyeConfig? Config { get; set; }

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(nameof(GetType), MegaCrit.Sts2.Core.Logging.LogType.Generic);

    static void Postfix(NCard __instance)
    {
        Logger.Info("Postfix triggered for NCard");

        if (Config == null)
        {
            Logger.Info("Config is null");
            return;
        }

        AddGooglyEyes(__instance);
    }

    private static void AddGooglyEyes(NCard card)
    {
        Logger.Info("Entering AddGooglyEyes");

        if (card.Model == null)
        {
            Logger.Info("Card model is null");
            return;
        }

        Logger.Info($"Card model detected: {card.Model.GetType().Name}");

        var cardConfig = Config!.GetCardConfig(card.Model);

        if (cardConfig == null)
        {
            Logger.Info("No config found for this card");
            return;
        }

        Logger.Info($"Config found for card: {cardConfig.Name}");

        var container = new Node2D { Name = "GooglyEyesContainer" };
        Logger.Info("Created GooglyEyes container node");

        var eyes = CreateEyes(cardConfig).ToList();
        Logger.Info($"Created {eyes.Count} eyes");

        foreach (var eye in eyes)
        {
            container.AddChild(eye);
        }

        card.AddChild(container);
        Logger.Info("Added GooglyEyes container to card");
    }

    private static IEnumerable<GooglyEye> CreateEyes(CardEyeConfig cardConfig)
    {
        foreach (var ea in cardConfig.EyeAnchors)
        {
            Logger.Info($"Creating eye with OffsetX={ea.OffsetX}, OffsetY={ea.OffsetY}, Radius={ea.RadiusPx}");
            yield return new GooglyEye(new Vector2(ea.OffsetX, ea.OffsetY), (int)ea.RadiusPx);
        }
    }
}