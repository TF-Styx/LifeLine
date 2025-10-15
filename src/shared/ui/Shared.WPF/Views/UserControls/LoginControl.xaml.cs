using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Shared.WPF.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        public LoginControl()
        {
            InitializeComponent();
        }

        #region Свойства
        public static readonly DependencyProperty LoginProperty = DependencyProperty.Register
            (
                nameof(Login), 
                typeof(string), 
                typeof(LoginControl), 
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
            );
        public string Login
        {
            get => (string)GetValue(LoginProperty);
            set => SetValue(LoginProperty, value);
        }

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register
            (
                nameof(Password),
                typeof(string),
                typeof(LoginControl),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
            );
        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }
        #endregion

        #region Команды
        public static readonly DependencyProperty AuthCommandProperty = DependencyProperty.Register
            (
                nameof(AuthCommand),
                typeof(ICommand),
                typeof(LoginControl),
                new PropertyMetadata(null)
            );
        public ICommand AuthCommand
        {
            get => (ICommand)GetValue(AuthCommandProperty);
            set => SetValue(AuthCommandProperty, value);
        }
        #endregion
    }
}
