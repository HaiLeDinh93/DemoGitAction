using CarMD.Fleet.Data.Request;
using CarMD.Fleet.Data.Request.Api;
using CarMD.Fleet.Data.Response;
using CarMD.Fleet.Data.Response.Api;
using System;

namespace CarMD.Fleet.Service.IService
{
    public interface IApiService
    {
        ServiceResult<bool> CreateLoggingTime(Guid userId, long kioskId, LoggingTimeModel logTimeModel);

        ServiceResult<bool> CreateLoggingError(Guid userId, long kioskId, LoggingErrorModel loggingErrorModel);

        ServiceResult<bool> FeedBack(FeedBackModel model);

        ServiceResult<bool> NotifyHavingDongle(long kioskId);

        ServiceResult<bool> EraseModuleDtcs(long kioskId, EraseModuleDtcModel model);

        ServiceResult<bool> SendReportEmail(Guid reportId, long kioskId, string email);

        ServiceResult<UserModel> Login(LoginModel authorizationModel, bool hashPassword = true);

        ServiceResult<UserModel> GetUserInformation(Guid userId);

        ServiceResult<LoggingErrorSearchModel> SearchLoggingError(SearchCriteria model);

        ServiceResult<LoggingTimeSearchModel> SearchLoggingTime(SearchCriteria model);

        ServiceResult<ReportModel> CreateDiagnosticReport(CreateReportRequest request);

        ServiceResult<ReportModel> CreateInnovaDiagnosticReport(CreateInnovaReportRequest request);

        ServiceResult<ReportHistoryModel> SearchReports(long kioskId, string timeZone, int pageSize, int pageIndex);

        ServiceResult<ReportModel> GetReportById(long kioskId, string timeZone, Guid id);

        ServiceResult<UserModel> LoginByAuth0(string email);
    }
}
