using Terminex.Common.Results;

namespace Shared.Kernel.Errors
{
    public static class AppErrors
    {
        public static readonly ErrorCode Create = ErrorCode.Custom(nameof(Create), 10000);
        public static readonly ErrorCode Upload = ErrorCode.Custom(nameof(Upload), 10001);
        public static readonly ErrorCode Delete = ErrorCode.Custom(nameof(Delete), 10002);
        public static readonly ErrorCode CreateHttp = ErrorCode.Custom(nameof(CreateHttp), 10003);
        public static readonly ErrorCode UpdateHttp = ErrorCode.Custom(nameof(UpdateHttp), 10004);
        public static readonly ErrorCode DeleteHttp = ErrorCode.Custom(nameof(DeleteHttp), 10005);
        public static readonly ErrorCode SRPVerificationFailed = ErrorCode.Custom(nameof(SRPVerificationFailed), 10006);
        public static readonly ErrorCode ExistDependentData = ErrorCode.Custom(nameof(ExistDependentData), 10007);
        public static readonly ErrorCode ExistContactInformation = ErrorCode.Custom(nameof(ExistContactInformation), 10008);
        public static readonly ErrorCode ExistEducationInformation = ErrorCode.Custom(nameof(ExistEducationInformation), 10009);
        public static readonly ErrorCode ExistPersonalDocument = ErrorCode.Custom(nameof(ExistPersonalDocument), 10010);
        public static readonly ErrorCode ExistEmployeeSPecialty = ErrorCode.Custom(nameof(ExistEmployeeSPecialty), 10011);
        public static readonly ErrorCode ExistWorkPermit = ErrorCode.Custom(nameof(ExistWorkPermit), 10012);
        public static readonly ErrorCode ExistEducationDocument = ErrorCode.Custom(nameof(ExistEducationDocument), 10013);
    }
}