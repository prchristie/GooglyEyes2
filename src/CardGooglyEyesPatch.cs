using System.Collections.Generic;
using System.Linq;
using GooglyEyes.config.persistence.models;
using GooglyEyes.GooglyEyes;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace GooglyEyes;

using Godot;
using HarmonyLib;

[HarmonyPatch(typeof(NCard), "UpdateVisuals")]
public static class CardGooglyEyesPatch
{
    public static EyeConfig? Config { get; set; }

    // private static Dictionary<NCard, List<GooglyEye>> eyes = new();
    private static HashSet<NCard> blah = new();

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(nameof(GetType), MegaCrit.Sts2.Core.Logging.LogType.Generic);

    static void Postfix(NCard __instance)
    {
        if (Config == null)
        {
            return;
        }

        if (!blah.Add(__instance))
        {
            return;
        }

        AddGooglyEyes(__instance);
    }

    private static void AddGooglyEyes(NCard card)
    {
        if (card.Model == null)
        {
            return;
        }

        var cardConfig = Config!.GetCardConfig(card.Model);

        if (cardConfig == null)
        {
            return;
        }

        var container = new Node2D { Name = "GooglyEyesContainer" };

        var eyes = CreateEyes(cardConfig).ToList();

        foreach (var eye in eyes)
        {
            container.AddChild(eye);
        }

        card.AddChild(container);
    }

    private static IEnumerable<GooglyEye> CreateEyes(CardEyeConfig cardConfig)
    {
        foreach (var ea in cardConfig.EyeAnchors)
        {
            yield return new GooglyEye(new Vector2(ea.OffsetX, ea.OffsetY), (int)ea.RadiusPx);
        }
    }
}