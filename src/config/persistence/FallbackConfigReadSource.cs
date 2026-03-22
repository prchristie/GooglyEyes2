using GooglyEyes.config.persistence.models;

namespace GooglyEyes.config.persistence;

public class FallbackConfigReadSource(IConfigReadSource primaryDelegate, IConfigReadSource secondaryDelegate)
    : IConfigReadSource
{
    static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(nameof(FallbackConfigReadSource), MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public EyeConfig Read()
    {
        try
        {
            return primaryDelegate.Read();
        }
        catch (Exception e)
        {
            Logger.Warn($"Primary eye config source not available: {e}");
            return secondaryDelegate.Read();
        }
    }
}