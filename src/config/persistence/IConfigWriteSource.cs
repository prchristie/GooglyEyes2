using GooglyEyes.config.persistence.models;

namespace GooglyEyes.config.persistence;

public interface IConfigWriteSource
{
    void Write(EyeConfig content);
}