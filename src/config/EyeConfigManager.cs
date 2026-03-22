using GooglyEyes.config.persistence;
using GooglyEyes.config.persistence.models;

namespace GooglyEyes.config;

public class EyeConfigManager(IConfigWriteSource write, IConfigReadSource read)
{
    private EyeConfig? _cachedConfig;

    private EyeConfig? ReloadConfig()
    {
        _cachedConfig = read.Read();
        return _cachedConfig;
    }

    public void Save(EyeConfig config)
    {
        write.Write(config);
        ReloadConfig();
    }

    public EyeConfig LoadConfig()
    {
        var config = _cachedConfig ?? ReloadConfig();
        return config ?? EyeConfig.EMPTY;
    }
}