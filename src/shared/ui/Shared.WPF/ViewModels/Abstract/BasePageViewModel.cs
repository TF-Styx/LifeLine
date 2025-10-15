namespace Shared.WPF.ViewModels.Abstract
{
    public abstract class BasePageViewModel : BaseViewModel
    {
        private string _pageTitle = string.Empty;
        public string PageTitle
        {
            get => _pageTitle;
            set => SetProperty(ref _pageTitle, value);
        }
    }
}
