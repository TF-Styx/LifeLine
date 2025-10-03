using LifeLine.MVVM.ViewModel;
using LifeLine.Services.NavigationPages;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;

namespace LifeLine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowVM viewModel, IServiceProvider serviceProvider)
        {
            InitializeComponent();

            #region MyRegion
            //IDialogService dialogService = new DialogService();
            //INavigationPage navigationServices = new NavigationPage(MainFrame);
            //Func<EmployeeManagementContext> contextFactory = () => new EmployeeManagementContext();
            //IDataBaseService dataBaseServices = new DataBaseService(contextFactory);
            //DataContext = new MainWindowVM(navigationServices, dialogService, dataBaseServices);
            #endregion

            DataContext = viewModel;

            var navigationPage = serviceProvider.GetService<INavigationPage>();
            navigationPage.GetFrame(MainFrame);

            StateChanged += MainWindowStateChangeRaised;
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // Minimize
        private void CommandBinding_Executed_Minimize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        // Maximize
        private void CommandBinding_Executed_Maximize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        // Restore
        private void CommandBinding_Executed_Restore(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        // Close
        private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e)
        {
            App.Current.Shutdown();
            //SystemCommands.CloseWindow(this);
        }

        // State change
        private void MainWindowStateChangeRaised(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                MainWindowBorder.BorderThickness = new Thickness(6);
                RestoreButton.Visibility = Visibility.Visible;
                MaximizeButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                MainWindowBorder.BorderThickness = new Thickness(0);
                RestoreButton.Visibility = Visibility.Collapsed;
                MaximizeButton.Visibility = Visibility.Visible;
            }
        }
    }
}