using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Practices.Unity;

namespace HSR.CloudSolutions.SmartKitchen.Simulator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private UnityServiceLocator _serviceLocator;
        private IDialogService _dialogService;

        private async void App_OnStartup(object sender, StartupEventArgs e)
        {
            _serviceLocator = SetupDependencies();

            _dialogService = _serviceLocator.GetInstance<IDialogService>();

            var viewModel = _serviceLocator.GetInstance<MainWindowViewModel>();
            await viewModel.LoadAsync();

            var view = _serviceLocator.GetInstance<MainWindow>();
            view.DataContext = viewModel;
            view.Closing += OnClosing;

            MainWindow = view;

            MainWindow.Show();
        }

        private UnityServiceLocator SetupDependencies()
        {
            var container = new UnityContainer();
            container.Setup();
            return new UnityServiceLocator(container);
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            var vm = MainWindow?.DataContext as MainWindowViewModel;
            if (vm != null)
            {
                Task.Run(vm.UnloadAsync).Wait();
            }
        }
        
        private void App_OnExit(object sender, ExitEventArgs e)
        {
            _serviceLocator?.Dispose();
        }

        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            _dialogService.ShowException(e.Exception);
            e.Handled = true;
        }
    }
}
