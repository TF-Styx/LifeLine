namespace Shared.WPF.ViewModels.Abstract
{
    public abstract class BaseWindowViewModel : BaseViewModel
    {
        private string _windowTitle = null!;
        public string WindowTitle
        {
            get => _windowTitle;
            set => SetProperty(ref _windowTitle, value);
        }
    }
}
