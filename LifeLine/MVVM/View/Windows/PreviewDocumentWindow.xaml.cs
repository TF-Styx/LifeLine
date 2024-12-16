using LifeLine.MVVM.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LifeLine.MVVM.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для PreviewDocumentWindow.xaml
    /// </summary>
    public partial class PreviewDocumentWindow : Window
    {
        public PreviewDocumentWindow()
        {
            InitializeComponent();

            StateChanged += MainWindowStateChangeRaised;
        }

        #region Кнопки управления

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
            //App.Current.Shutdown();
            SystemCommands.CloseWindow(this);
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

        #endregion

        private Point _startPoint;
        private bool _isDragging;

        private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var viewModel = (PreviewDocumentVM)DataContext; // Приведение типов. Делать только если знаешь что находится в object

            // Получаем позицию курсора относительно изображения
            var image = (System.Windows.Controls.Image)sender;
            var cursorPosition = e.GetPosition(image);

            // Вычисляем текущие координаты в масштабированном пространстве
            double relativeX = (cursorPosition.X - viewModel.TranslateX) / viewModel.ScaleX;
            double relativeY = (cursorPosition.Y - viewModel.TranslateY) / viewModel.ScaleY;

            // Применяем масштабирование 
            double zoomFactor = e.Delta > 0 ? 0.1 : -0.1;

            double newScaleX = Math.Max(0.1, viewModel.ScaleX + zoomFactor);
            double newScaleY = Math.Max(0.1, viewModel.ScaleY + zoomFactor);

            // Корректируем сдвиг так, чтобы масштабирование происходило относительно курсора
            viewModel.TranslateX -= (relativeX * (newScaleX - viewModel.ScaleX)); /*(cursorPosition.X - relativeX * newScaleX);*/
            viewModel.TranslateY -= (relativeY * (newScaleY - viewModel.ScaleY)); /*(cursorPosition.Y - relativeY * newScaleY);*/

            // Обновляем масштаб
            viewModel.ScaleX = newScaleX; 
            viewModel.ScaleY = newScaleY;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDragging = true;
            _startPoint = e.GetPosition(this);
            Mouse.Capture((UIElement)sender);
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            Mouse.Capture(null);
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                var viewModel = (PreviewDocumentVM)DataContext;
                var currentPoint = e.GetPosition(this);

                viewModel.TranslateX += currentPoint.X - _startPoint.X;
                viewModel.TranslateY += currentPoint.Y - _startPoint.Y;

                _startPoint = currentPoint;
            }
        }
    }
}
