namespace Shared.Kernel.Results
{
    public enum ErrorCode
    {
        Empty,
        NotFound,
        Validation,
        Exist,

        Save,

        Create,
        CreateHttp,

        Update,
        UpdateHttp,

        Delete,
        DeleteHttp,

        Server,
        InvalidRequest,
        InvalidResponse,

        ExistContactInformation,
        ExistDependentData,
        ExistEducationInformation,
    }
}
