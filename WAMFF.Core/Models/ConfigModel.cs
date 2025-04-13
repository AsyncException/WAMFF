using System.Text.Json.Serialization;

namespace WAMFF.Core.Models;

public class ConfigModel : IEquatable<ConfigModel>
{
    public List<string> DirectoryPath { get; set; } = [];
    public bool IsVSCodeInstalled { get; set; } = false;
    public string VSCodePath { get; set; } = string.Empty;

    public static ConfigModel GetDefault() {
        string[] paths = Environment.GetEnvironmentVariable("PATH")?.Split(';') ?? [];
        string? vspath = paths.Where(e => e.EndsWith("Microsoft VS Code\\bin")).FirstOrDefault();
        bool is_vs_code_installed = !string.IsNullOrEmpty(vspath);
        string vs_code_path = is_vs_code_installed ? string.Concat(vspath![..^3], "code.exe") : string.Empty;

        return new ConfigModel {
            DirectoryPath = [Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "WAMF")],
            IsVSCodeInstalled = is_vs_code_installed,
            VSCodePath = vs_code_path
        };
    }

    public bool Equals(ConfigModel? other) {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return IsVSCodeInstalled == other.IsVSCodeInstalled &&
               VSCodePath == other.VSCodePath &&
               DirectoryPath.SequenceEqual(other.DirectoryPath);
    }

    public override bool Equals(object? obj) {
        return Equals(obj as ConfigModel);
    }

    public override int GetHashCode() {
        HashCode hash = new();
        hash.Add(IsVSCodeInstalled);
        hash.Add(VSCodePath);
        foreach (string path in DirectoryPath) {
            hash.Add(path);
        }
        return hash.ToHashCode();
    }
}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(ConfigModel))]
public partial class ConfigModelContext : JsonSerializerContext;