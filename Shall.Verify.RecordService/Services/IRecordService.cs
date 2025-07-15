using Shall.Verify.Common.Dtos.Record;
using Shall.Verify.Common.Entities.Record;

namespace Shall.Verify.RecordService.Services;

public interface IRecordService
{
    #region Record CRUD
    Task<RecordAttributes> GetRecordAsync(Guid recordId);
    Task<bool> CreateRecordAsync(RecordAttributes record);
    Task<bool> DeleteRecordAsync(Guid recordId);
    #endregion Record CRUD

    #region Record Status
    Task<bool> AddRecordStatusAsync(RecordStatusRequest recordStatusRequest);

    Task<bool> AddBatchRecordStatusAsync(BatchRecordStatusRequest batchRecordRequest);
    #endregion Record Status

    #region Record Count
    Task<RecordCountResponse> GetRecordCountAsync(RecordCountRequest recordCountRequest);
    #endregion Record Count

    #region Record Match
    Task<RecordMatchResponse> GetRecordMatchAsync(RecordMatchRequest recordMatchRequest);
    #endregion Record Match
}