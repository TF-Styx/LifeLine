using Shared.Client.Security.Abstraction;
using Shared.Contracts.Desktop;

namespace Shared.Client.Security.Windows
{
    public sealed class UserContext : IUserContext
    {
        public CurrentUser? CurrentUser { get; private set; }

        public void SetUser(CurrentUser currentUser)
        {
            CurrentUser = currentUser;
        }

        public void Clear()
        {
            CurrentUser = null;
        }
    }
}
