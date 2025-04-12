using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using System.Runtime.InteropServices;
using Windows.UI;
using WinRT;

namespace WAMFF.Utilities;

public class BackDropController
{
    private static readonly Color tint_color = Color.FromArgb(100, 0, 0, 0);

    private readonly DesktopAcrylicController m_backdropController = new() { TintColor = tint_color };
    private readonly WindowsSystemDispatcherQueueHelper m_wsdqHelper = new();
    private readonly SystemBackdropConfiguration m_configurationSource = new() { IsInputActive = true, Theme = SystemBackdropTheme.Dark };

    private Window? m_window;

    public void SetAcrylicBackdrop(Window window) {
        if (!DesktopAcrylicController.IsSupported()) {
            return;
        }

        m_window = window;

        m_wsdqHelper.EnsureWindowsSystemDispatcherQueueController();

        m_window.Activated += Window_Activated;
        m_window.Closed += Window_Closed;
        ((FrameworkElement)m_window.Content).ActualThemeChanged += Window_ThemeChanged;

        m_backdropController.SetSystemBackdropConfiguration(m_configurationSource);
        m_backdropController.AddSystemBackdropTarget(m_window.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
    }

    private void Window_Activated(object sender, WindowActivatedEventArgs args) {
        m_configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;
    }

    private void Window_Closed(object sender, WindowEventArgs args) {
        m_backdropController?.Dispose();

        if (m_window != null) {
            m_window!.Activated -= Window_Activated;
            m_window.Closed -= Window_Closed;
            m_window = null;
        }
    }

    private void Window_ThemeChanged(FrameworkElement sender, object args) {
        if (m_configurationSource != null) {
            SetConfigurationSourceTheme();
        }
    }

    private void SetConfigurationSourceTheme() {
        m_configurationSource.Theme = ((FrameworkElement)m_window!.Content).ActualTheme switch {
            ElementTheme.Dark => SystemBackdropTheme.Dark,
            ElementTheme.Light => SystemBackdropTheme.Light,
            ElementTheme.Default => SystemBackdropTheme.Default
        };
    }
}

public class WindowsSystemDispatcherQueueHelper
{
    [StructLayout(LayoutKind.Sequential)]
    private struct DispatcherQueueOptions
    {
        internal int dwSize;
        internal int threadType;
        internal int apartmentType;
    }

    [DllImport("CoreMessaging.dll")]
    private static extern int CreateDispatcherQueueController([In] DispatcherQueueOptions options, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object dispatcherQueueController);

    private object m_dispatcherQueueController = null;

    public void EnsureWindowsSystemDispatcherQueueController() {
        if (Windows.System.DispatcherQueue.GetForCurrentThread() != null) {
            // one already exists, so we'll just use it.
            return;
        }

        if (m_dispatcherQueueController == null) {
            DispatcherQueueOptions options;
            options.dwSize = Marshal.SizeOf(typeof(DispatcherQueueOptions));
            options.threadType = 2;    // DQTYPE_THREAD_CURRENT
            options.apartmentType = 2; // DQTAT_COM_STA

            CreateDispatcherQueueController(options, ref m_dispatcherQueueController);
        }
    }
}