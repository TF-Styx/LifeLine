using LifeLine.Utils.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LifeLine.MVVM.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {
        public bool Result { get; private set; }
        public MessageButtons ResultButton { get; private set; }

        public NotificationWindow(string Message, string Title, MessageButtons messageButtons = MessageButtons.OK)
        {
            InitializeComponent();

            StateChanged += MainWindowStateChangeRaised;

            MessageTextBlock.Text = Message;

            TitleText.Text = Title;
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            ResultButton = MessageButtons.Yes;
            Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            ResultButton = MessageButtons.OK;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            ResultButton = MessageButtons.Cancel;
            Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            ResultButton = MessageButtons.No;
            Close();
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_Executed_Minimize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void CommandBinding_Executed_Maximize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        private void CommandBinding_Executed_Restore(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        private void MainWindowStateChangeRaised(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                MainWindowBorder.BorderThickness = new Thickness(8);
            }
            else
            {
                MainWindowBorder.BorderThickness = new Thickness(0);
            }
        }

        private void CheckMessageButtonsResult(MessageButtons messageButtons)
        {
            if (messageButtons == MessageButtons.OK)
            {
                YesButton.Visibility = Visibility.Collapsed;
                NoButton.Visibility = Visibility.Collapsed;
                CancelButton.Visibility = Visibility.Collapsed;
            }
            else if (messageButtons == MessageButtons.Cancel)
            {
                OkButton.Visibility = Visibility.Collapsed;
                YesButton.Visibility = Visibility.Collapsed;
                NoButton.Visibility = Visibility.Collapsed;
            }
            else if (messageButtons == MessageButtons.OKCancel)
            {
                YesButton.Visibility = Visibility.Collapsed;
                NoButton.Visibility = Visibility.Collapsed;
            }
            else if (messageButtons == MessageButtons.Yes)
            {
                OkButton.Visibility = Visibility.Collapsed;
                NoButton.Visibility = Visibility.Collapsed;
                CancelButton.Visibility = Visibility.Collapsed;
            }
            else if (messageButtons == MessageButtons.No)
            {
                OkButton.Visibility = Visibility.Collapsed;
                YesButton.Visibility = Visibility.Collapsed;
                CancelButton.Visibility = Visibility.Collapsed;
            }
            else if (messageButtons == MessageButtons.YesNo)
            {
                OkButton.Visibility = Visibility.Collapsed;
                CancelButton.Visibility = Visibility.Collapsed;
            }
            else if (messageButtons == MessageButtons.YesNoCancel)
            {
                OkButton.Visibility = Visibility.Collapsed;
            }

        }
    }
}
