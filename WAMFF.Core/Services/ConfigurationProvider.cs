using System.Text.Json;
using WAMFF.Core.Models;

namespace WAMFF.Core.Services;

public static class ConfigurationProvider
{
    private static readonly string path = "AppSettings.json";
    private static readonly JsonSerializerOptions options = new() { WriteIndented = true };

    private static ConfigModel? _currentConfig = null;

    public static ConfigModel CurrentConfig {
        get {
            return _currentConfig ??= GetConfig();
        }
        set {
            if (!value.Equals(_currentConfig)) {
                SaveConfig(value);
                _currentConfig = value;
            }
        }
    }

    private static ConfigModel GetConfig() {
        ConfigModel? config = null;
        if (File.Exists(path)) {
            using FileStream stream = File.OpenRead(path);
            config = JsonSerializer.Deserialize<ConfigModel>(stream);
        }

        if (config is null) {
            config = ConfigModel.GetDefault();
            using FileStream stream = File.Create(path);
            stream.SetLength(0);
            JsonSerializer.Serialize(stream, config, options);
            stream.Flush();
        }

        return config;
    }

    private static void SaveConfig(ConfigModel config) {
        using FileStream stream = File.Create(path);
        stream.SetLength(0);
        JsonSerializer.Serialize(stream, config, options);
        stream.Flush();
    }
}