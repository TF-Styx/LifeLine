namespace LifeLine.User.Service.Client.Models.Response
{
    public sealed record UserResponse(string Id, string Login, string UserName, string Mail, DateTime DateRegistration, int IdStatus, int IdRole);
}
