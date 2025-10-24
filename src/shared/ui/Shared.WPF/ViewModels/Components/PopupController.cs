using Shared.WPF.Commands;
using Shared.WPF.Enums;
using Shared.WPF.ViewModels.Abstract;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Shared.WPF.ViewModels.Components
{
    public sealed class PopupController : BaseViewModel
    {
        public PopupController(PopupPlacement popupPlacement = PopupPlacement.Default, Func<bool>? condition = null)
        {
            _condition = condition;

            ShowCommand = new RelayCommand<UIElement>(Execute_ShowCommand, CanExecute_ShowCommand);
            HideCommand = new RelayCommand(Execute_HideCommand);

            if (popupPlacement == PopupPlacement.CustomRightUp)
            {
                CustomPopupPlacementCallback = PlacePopupRightUp;
            }
        }

        private readonly Func<bool>? _condition;

        private bool _isOpen;
        public bool IsOpen
        {
            get => _isOpen;
            set => SetProperty(ref _isOpen, value);
        }

        private bool _staysOpen;
        public bool StaysOpen
        {
            get => _staysOpen;
            set => SetProperty(ref _staysOpen, value);
        }

        private UIElement? _targetElement;
        public UIElement? TargetElement
        {
            get => _targetElement; 
            set => SetProperty(ref _targetElement, value);
        }

        public RelayCommand<UIElement> ShowCommand { get; private set; }
        private void Execute_ShowCommand(UIElement uIElement)
        {
            IsOpen = true;
            TargetElement = uIElement;
        }
        private bool CanExecute_ShowCommand(UIElement uIElement)
        {
            if (_condition == null)
                return true;

            return _condition.Invoke();
        }

        public RelayCommand HideCommand { get; private set; }
        private void Execute_HideCommand()
        {
            IsOpen = false;
            TargetElement = null;
        }

        #region Управление размерами

        // double.NaN != double.NaN
        private double _height = double.NaN;
        public double Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }

        private double _maxHeight = double.PositiveInfinity;
        public double MaxHeight
        {
            get => _maxHeight;
            set => SetProperty(ref _maxHeight, value);
        }

        public CustomPopupPlacementCallback? CustomPopupPlacementCallback { get; private set; }

        public void UpdatePopupSize(double actualHeightContainer, double actualHeightContent = 0)
        {
            if (actualHeightContainer > 100)
                MaxHeight = actualHeightContainer - 100;

            if (actualHeightContent > 0)
                Height = actualHeightContent + 100;
        }

        private CustomPopupPlacement[] PlacePopupRightUp(Size popupSize, Size targetSize, Point offset)
        {
            if (TargetElement is FrameworkElement target)
            {
                double xOffset = target.ActualWidth;
                double yOffset = 0;
                return [new CustomPopupPlacement(new Point(xOffset + 10, (yOffset - popupSize.Height) + 25), PopupPrimaryAxis.Vertical)];
            }
            return [new CustomPopupPlacement(new Point(0, 0), PopupPrimaryAxis.Vertical)];
        }

        #endregion
    }
}
