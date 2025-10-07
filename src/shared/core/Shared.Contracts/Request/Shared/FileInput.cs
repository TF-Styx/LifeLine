namespace Shared.Contracts.Request.Shared
{
    public sealed record FileInput(Stream Content, string FileName, string ContentType);
}
