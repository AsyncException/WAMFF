using System.Diagnostics;
using WAMFF.Core.Models;

namespace WAMFF.Core.Services;

public static class ProcessStarter
{
    public static void WithDefault(FileDetails file) {
        ProcessStartInfo info = new(file.FullPath) { UseShellExecute = true };
        Process.Start(info);
    }

    public static void WithOpenWith(FileDetails file) {
        ProcessStartInfo info = new("C:\\WINDOWS\\system32\\OpenWith.exe") { UseShellExecute = true, Arguments = $"\"{file.FullPath}\"" };
        Process.Start(info);
    }

    public static void WithVsCode(FileDetails file, ConfigModel config) {
        ProcessStartInfo info = new(config.VSCodePath) { Arguments = $"\"{file.FullPath}\"" };
        Process.Start(info);
    }

    public static void ShowInExplorer(FileDetails file) {
        ProcessStartInfo info = new("explorer.exe") { Arguments = $"/select,\"{file.FullPath}\"" };
        Process.Start(info);
    }
}