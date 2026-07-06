using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace ProjectPilotLite.Wpf;

public partial class App : Application
{

    private static readonly string LogPath =
        Path.Combine(AppContext.BaseDirectory, "crash.log");

    public App()
    {
        DispatcherUnhandledException += OnDispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += OnAppDomainUnhandledException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        Log("DispatcherUnhandledException", e.Exception);

        MessageBox.Show(
            $"Une erreur inattendue est survenue :\n\n{e.Exception.Message}\n\n" +
            "Vérifie que l'API (ProjectPilotLite.Api) est bien démarrée sur http://localhost:5123.\n" +
            $"Détails enregistrés dans : {LogPath}",
            "ProjectPilot Lite - Erreur",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
        e.Handled = true;
    }

    private void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        Log("AppDomain.UnhandledException", e.ExceptionObject as Exception);
    }

    private void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        Log("UnobservedTaskException", e.Exception);
        e.SetObserved();
    }

    private static void Log(string source, Exception? ex)
    {
        try
        {
            File.AppendAllText(LogPath,
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {source}: {ex}\n\n");
        }
        catch
        {
        }
    }
}
