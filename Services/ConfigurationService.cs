using YamlDotNet.Serialization;
using q_clipboard_server.Models;

namespace q_clipboard_server.Services;


public static class ConfigurationService
{
    public static ConfigurationModel Configuration { get; }

    static ConfigurationService()
    {
        var configPath = "config/config.yaml";
        if (File.Exists(configPath))
        {
            var config = File.ReadAllText(configPath);
            Configuration = new Deserializer().Deserialize<ConfigurationModel>(config);
        }
        else
        {
            Configuration = new ConfigurationModel();
            var serializer = new Serializer();
            var config = serializer.Serialize(Configuration);
            var configDir = Path.GetDirectoryName(configPath);
            if (configDir != null) {
                Directory.CreateDirectory(configDir);
            }
            File.WriteAllText(configPath, config);
        }
    }
}