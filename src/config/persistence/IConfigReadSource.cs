using GooglyEyes.config.persistence.models;

namespace GooglyEyes.config.persistence;

public interface IConfigReadSource
{
    EyeConfig Read();
}