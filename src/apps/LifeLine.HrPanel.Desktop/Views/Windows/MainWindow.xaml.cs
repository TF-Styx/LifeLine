using LifeLine.HrPanel.Desktop.ViewModels.Windows;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowVM vm)
        {
            this.DataContext = vm;

            InitializeComponent();

            this.Width = 550;
            this.Height = 650;
            this.ResizeMode = ResizeMode.NoResize;

            vm.AuthController.ResizeWindowAfterLogin += () =>
            {
                this.Width = 800;
                this.Height = 450;
                this.ResizeMode = ResizeMode.CanResize;

                CalculateWindowPosition();
            };

            vm.AuthController.ResizeWindowAfterLogout += () =>
            {
                this.Width = 550;
                this.Height = 650;
                this.ResizeMode = ResizeMode.NoResize;

                CalculateWindowPosition();
            };
        }

        private void CalculateWindowPosition()
        {
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;

            this.Left = (screenWidth / 2) - (this.ActualWidth / 2);
            this.Top = (screenHeight / 2) - (this.ActualHeight / 2);
        }
    }
}
