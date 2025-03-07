﻿using LifeLine.MVVM.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace LifeLine.MVVM.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();

            DataContext = new AuthWindowVM();
        }

        // Can execute
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
        //private void CommandBinding_Executed_Maximize(object sender, ExecutedRoutedEventArgs e)
        //{
        //    SystemCommands.MaximizeWindow(this);
        //}

        //// Restore
        //private void CommandBinding_Executed_Restore(object sender, ExecutedRoutedEventArgs e)
        //{
        //    SystemCommands.RestoreWindow(this);
        //}

        // Close
        private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e)
        {
            App.Current.Shutdown();
            //SystemCommands.CloseWindow(this);
        }

        // State change
        //private void MainWindowStateChangeRaised(object sender, EventArgs e)
        //{
        //    if (WindowState == WindowState.Maximized)
        //    {
        //        MainWindowBorder.BorderThickness = new Thickness(8);
        //        RestoreButton.Visibility = Visibility.Visible;
        //        MaximizeButton.Visibility = Visibility.Collapsed;
        //    }
        //    else
        //    {
        //        MainWindowBorder.BorderThickness = new Thickness(0);
        //        RestoreButton.Visibility = Visibility.Collapsed;
        //        MaximizeButton.Visibility = Visibility.Visible;
        //    }
        //}
    }
}
