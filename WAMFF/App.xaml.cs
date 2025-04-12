using CommunityToolkit.Mvvm.DependencyInjection;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using System;
using System.IO;
using WAMFF.Core.Services;
using WAMFF.Utilities;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WAMFF
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private readonly IFileService f_FileService;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App() {
            this.InitializeComponent();
            Ioc.Default.ConfigureServices(new ServiceCollection()
                .AddSingleton<BackDropController>()
                .AddSingleton<ILiteDatabase>(CreateDatabase)
                .AddSingleton<IFileService, FileService>()
                .AddTransient<ICategoryRepository, CategoryRepository>()
                .AddTransient<IFileRepository, FileRepository>()
                .AddTransient<ITagsRepository, TagsRespository>()
                .BuildServiceProvider());

            f_FileService = Ioc.Default.GetRequiredService<IFileService>()!;
            f_FileService.AttachDispatcherQueue(DispatcherQueue.GetForCurrentThread());

            f_FileService.StartCleanUp();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args) {
            m_window = new MainWindow();
            m_window.Activate();
        }

        private Window? m_window;

        private static ILiteDatabase CreateDatabase(IServiceProvider provider) {
            string directory_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WAMF");

            if (!Directory.Exists(directory_path)) {
                Directory.CreateDirectory(directory_path);
            }

            string path = Path.Combine(directory_path, "datastore.db");
            return new LiteDatabase(path);
        }
    }
}