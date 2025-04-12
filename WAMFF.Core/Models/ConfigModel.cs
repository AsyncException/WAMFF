namespace WAMFF.Core.Models;

public class ConfigModel
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
}