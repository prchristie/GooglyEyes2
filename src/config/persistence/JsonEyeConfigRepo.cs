using System.Text.Json;
using GooglyEyes.config.persistence.models;

namespace GooglyEyes.config.persistence;

public class JsonEyeConfigRepo(String fileName)
{
    public EyeConfig? Load()
    {
        using (StreamReader r = new StreamReader(fileName))
        {
            string json = r.ReadToEnd();
            return JsonSerializer.Deserialize<EyeConfig>(json);
        }
    }

    public void Save(EyeConfig config)
    {
        using (StreamWriter w = new StreamWriter(fileName))
        {
            string json = JsonSerializer.Serialize(config);
            w.Write(json);
        }
    }
}