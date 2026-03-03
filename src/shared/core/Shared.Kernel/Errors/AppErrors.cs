using Terminex.Common.Results;

namespace Shared.Kernel.Errors
{
    public static class AppErrors
    {
        public static readonly ErrorCode CreateHttp = ErrorCode.Custom(nameof(CreateHttp), 10000);
        public static readonly ErrorCode UpdateHttp = ErrorCode.Custom(nameof(UpdateHttp), 10001);
        public static readonly ErrorCode DeleteHttp = ErrorCode.Custom(nameof(DeleteHttp), 10002);
        public static readonly ErrorCode SRPVerificationFailed = ErrorCode.Custom(nameof(SRPVerificationFailed), 10003);
        public static readonly ErrorCode ExistDependentData = ErrorCode.Custom(nameof(ExistDependentData), 10004);
        public static readonly ErrorCode ExistContactInformation = ErrorCode.Custom(nameof(ExistContactInformation), 10004);
        public static readonly ErrorCode ExistEducationInformation = ErrorCode.Custom(nameof(ExistEducationInformation), 10005);
        public static readonly ErrorCode ExistPersonalDocument = ErrorCode.Custom(nameof(ExistPersonalDocument), 10005);
    }
}