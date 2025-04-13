using System.Text.Json;
using WAMFF.Core.Models;

namespace WAMFF.Core.Services;

public static class ConfigurationProvider
{
#if DEBUG
    private static string path = "AppSettings.json";
#else
    private static string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WAMFF", "AppSettings.json");
#endif
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
            config = JsonSerializer.Deserialize<ConfigModel>(stream, ConfigModelContext.Default.ConfigModel);
        }

        if (config is null) {
            config = ConfigModel.GetDefault();
            using FileStream stream = File.Create(path);
            stream.SetLength(0);
            JsonSerializer.Serialize(stream, config, ConfigModelContext.Default.ConfigModel);
            stream.Flush();
        }

        return config;
    }

    private static void SaveConfig(ConfigModel config) {
        using FileStream stream = File.Create(path);
        stream.SetLength(0);
        JsonSerializer.Serialize(stream, config, ConfigModelContext.Default.ConfigModel);
        stream.Flush();
    }
}