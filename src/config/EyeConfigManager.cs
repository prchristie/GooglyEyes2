using GooglyEyes.config.persistence;
using GooglyEyes.config.persistence.models;

namespace GooglyEyes.config;

public class EyeConfigManager(JsonEyeConfigRepo repo)
{
    private EyeConfig? _cachedConfig;

    private EyeConfig? LoadConfig()
    {
        _cachedConfig = repo.Load();
        return _cachedConfig;
    }

    public EyeConfig GetConfig()
    {
        var config = _cachedConfig ?? LoadConfig();
        return config ?? EyeConfig.EMPTY;
    }
}