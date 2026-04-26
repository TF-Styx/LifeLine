using LifeLine.HrPanel.Desktop.Enums;
using System.Windows;
using System.Windows.Controls;

namespace LifeLine.HrPanel.Desktop.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для PersonalDocuments.xaml
    /// </summary>
    public partial class PersonalDocuments : UserControl
    {
        public PersonalDocuments()
        {
            InitializeComponent();
        }

        #region ActionProperty

        public static readonly DependencyProperty ActionProperty =
            DependencyProperty.Register(
                nameof(Action),
                typeof(TypeAction),
                typeof(PersonalDocuments),
                new FrameworkPropertyMetadata(TypeAction.None, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public TypeAction Action
        {
            get => (TypeAction)GetValue(ActionProperty);
            set => SetValue(ActionProperty, value);
        }

        #endregion

        #region CommandAsyncProperty

        //public static readonly DependencyProperty CommandAsyncProperty =
        //    DependencyProperty.Register(
        //        nameof(CommandAsync),
        //        typeof(RelayCommandAsync),
        //        typeof(PersonalDocuments),
        //        new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //public RelayCommandAsync CommandAsync
        //{
        //    get => (RelayCommandAsync)GetValue(CommandAsyncProperty);
        //    set => SetValue(CommandAsyncProperty, value);
        //}

        #endregion

        #region CommandParameterProperty

        //public static readonly DependencyProperty CommandParameterProperty =
        //    DependencyProperty.Register(
        //        nameof(CommandParameter),
        //        typeof(object),
        //        typeof(PersonalDocuments),
        //        new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //public object CommandParameter
        //{
        //    get => (object)GetValue(CommandParameterProperty);
        //    set => SetValue(CommandParameterProperty, value);
        //}

        #endregion
    }
}
