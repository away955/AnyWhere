using Away.Service.Xray.Impl;

namespace Away.Wind
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            XrayService.XraysClose();
        }
    }
}
