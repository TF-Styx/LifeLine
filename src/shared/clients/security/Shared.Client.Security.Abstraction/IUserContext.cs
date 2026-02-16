using Shared.Contracts.Desktop;

namespace Shared.Client.Security.Abstraction
{
    public interface IUserContext
    {
        CurrentUser? CurrentUser { get; }
        void SetUser(CurrentUser currentUser);
        void Clear();
    }
}
