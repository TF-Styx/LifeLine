namespace LifeLine.HrPanel.Desktop.Services.Secure
{
    public interface IAuthenticationStateService
    {
        public event Action? AuthenticationRequired;
        void NotifyAuthenticationRequired();
    }
}