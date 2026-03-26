using FootballBookingAPI.Data;
    using FootballBookingAPI.DTOs.Field;
    using FootballBookingAPI.Models;
    using FootballBookingAPI.Services.Interfaces;
    using Microsoft.EntityFrameworkCore;


namespace FootballBookingAPI.Services.Implementations
{
    

    using static FootballBookingAPI.Models.Enums.Enums;

    public class FieldService : IFieldService
    {
        private readonly AppDbContext _context;

        public FieldService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FieldResponse> CreateAsync(string ownerId, CreateFieldRequest request)
        {
            var field = new Field
            {
                Name = request.Name,
                Location = request.Location,
                Description = request.Description,
                PricePerHour = request.PricePerHour,
                OwnerId = ownerId,
                Status = FieldStatus.Pending
            };

            _context.Fields.Add(field);
            await _context.SaveChangesAsync();

            return new FieldResponse
            {
                Id = field.Id,
                Name = field.Name,
                Location = field.Location,
                PricePerHour = field.PricePerHour,
                Status = field.Status.ToString()
            };
        }

        public async Task<List<FieldResponse>> GetAllAsync()
        {
            return await _context.Fields
                .Where(f => f.Status == FieldStatus.Approved)
                .Select(f => new FieldResponse
                {
                    Id = f.Id,
                    Name = f.Name,
                    Location = f.Location,
                    PricePerHour = f.PricePerHour,
                    Status = f.Status.ToString()
                })
                .ToListAsync();
        }

        public async Task<FieldResponse?> GetByIdAsync(int id)
        {
            var f = await _context.Fields.FindAsync(id);
            if (f == null || f.Status != FieldStatus.Approved)
                return null;

            return new FieldResponse
            {
                Id = f.Id,
                Name = f.Name,
                Location = f.Location,
                PricePerHour = f.PricePerHour,
                Status = f.Status.ToString()
            };
        }

        public async Task<bool> UpdateAsync(int id, string ownerId, UpdateFieldRequest request)
        {
            var field = await _context.Fields.FindAsync(id);
            if (field == null || field.OwnerId != ownerId)
                return false;

            field.Name = request.Name;
            field.Location = request.Location;
            field.Description = request.Description;
            field.PricePerHour = request.PricePerHour;

            field.Status = FieldStatus.Pending; // sửa phải duyệt lại

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id, string ownerId)
        {
            var field = await _context.Fields.FindAsync(id);
            if (field == null || field.OwnerId != ownerId)
                return false;

            _context.Fields.Remove(field);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveAsync(int id)
        {
            var field = await _context.Fields.FindAsync(id);
            if (field == null) return false;

            field.Status = FieldStatus.Approved;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectAsync(int id)
        {
            var field = await _context.Fields.FindAsync(id);
            if (field == null) return false;

            field.Status = FieldStatus.Rejected;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<FieldResponse>> GetAllForAdminAsync()
        {
            return await _context.Fields
                .Select(f => new FieldResponse
                {
                    Id = f.Id,
                    Name = f.Name,
                    Location = f.Location,
                    PricePerHour = f.PricePerHour,
                    Status = f.Status.ToString()
                })
                .ToListAsync();
        }

        public async Task<List<FieldResponse>> GetMyFieldsAsync(string ownerId)
        {
            return await _context.Fields
                .Where(f => f.OwnerId == ownerId)
                .Select(f => new FieldResponse
                {
                    Id = f.Id,
                    Name = f.Name,
                    Location = f.Location,
                    PricePerHour = f.PricePerHour,
                    Status = f.Status.ToString()
                })
                .ToListAsync();
        }
    }
}
