using System.Windows;
using Tobii.Interaction;
using Tobii.Interaction.Wpf;

namespace wheres_waldo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Host _host;
        private WpfInteractorAgent _wpfInteractorAgent;

        protected override void OnStartup(StartupEventArgs e)
        {
            _host = new Host();
            _wpfInteractorAgent = _host.InitializeWpfAgent();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.Dispose();
            base.OnExit(e);
        }
    }
}
