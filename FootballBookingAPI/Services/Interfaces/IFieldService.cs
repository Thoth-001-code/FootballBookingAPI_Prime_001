
using FootballBookingAPI.DTOs.Common;
using FootballBookingAPI.DTOs.Field;

namespace FootballBookingAPI.Services.Interfaces
{
   

    public interface IFieldService
    {
        Task<FieldResponse> CreateAsync(string ownerId, CreateFieldRequest request);
        Task<List<FieldResponse>> GetAllAsync();
        Task<FieldResponse?> GetByIdAsync(int id);
        Task<List<FieldResponse>> GetAllForAdminAsync();
        Task<bool> UpdateAsync(int id, string ownerId, UpdateFieldRequest request);
        Task<bool> DeleteAsync(int id, string ownerId);
        Task<List<FieldResponse>> GetMyFieldsAsync(string ownerId);
        Task<bool> ApproveAsync(int id);
        Task<bool> RejectAsync(int id);
        Task<PagedResult<FieldResponse>> SearchAsync(FieldQueryRequest request);
    }
}
