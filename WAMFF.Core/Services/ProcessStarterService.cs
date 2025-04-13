using System.Diagnostics;
using WAMFF.Core.Models;

namespace WAMFF.Core.Services;

public static class ProcessStarter
{
    public static void WithDefault(FileDetails file) => WithDefault(file.FullPath);
    public static void WithDefault(string file) {
        ProcessStartInfo info = new(file) { UseShellExecute = true };
        Process.Start(info);
    }

    public static void WithOpenWith(FileDetails file) => WithOpenWith(file.FullPath);
    public static void WithOpenWith(string file) {
        ProcessStartInfo info = new("C:\\WINDOWS\\system32\\OpenWith.exe") { UseShellExecute = true, Arguments = $"\"{file}\"" };
        Process.Start(info);
    }

    public static void WithVsCode(FileDetails file) => WithVsCode(file.FullPath);
    public static void WithVsCode(string file) {
        ProcessStartInfo info = new(ConfigurationProvider.CurrentConfig.VSCodePath) { Arguments = $"\"{file}\"" };
        Process.Start(info);
    }

    public static void ShowInExplorer(FileDetails file) => ShowInExplorer(file.FullPath);
    public static void ShowInExplorer(string file) {
        ProcessStartInfo info = new("explorer.exe") { Arguments = $"/select,\"{file}\"" };
        Process.Start(info);
    }
}