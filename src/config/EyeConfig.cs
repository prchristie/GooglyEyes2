using MegaCrit.Sts2.Core.Models;

namespace GooglyEyes.config.persistence.models;

public class EyeConfig
{
    public static EyeConfig EMPTY = new EyeConfig();

    public Dictionary<string, CardEyeConfig> CardDefinitions { get; set; } = new();
    // public Dictionary<string, RelicEyeConfig> RelicDefinitions { get; set; }

    public CardEyeConfig? GetCardConfig(CardModel card)
    {
        return CardDefinitions.GetValueOrDefault(card.GetType().Name);
    }

    public bool ContainsCard(CardModel card)
    {
        return CardDefinitions.ContainsKey(card.GetType().Name);
    }
}

public class CardEyeConfig
{
    public string Name { get; set; }
    public List<EyeAnchor> EyeAnchors { get; set; }
}

// public class RelicEyeConfig
// {
//     public string Id { get; set; }
//     public List<EyeAnchor> EyeAnchors { get; set; }
// }

public class EyeAnchor
{
    public float OffsetX { get; set; }
    public float OffsetY { get; set; }
    public float RadiusPx { get; set; }
}