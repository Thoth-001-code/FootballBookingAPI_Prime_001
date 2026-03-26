using FootballBookingAPI.Data;
using FootballBookingAPI.DTOs.Common;
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
        // Chỉ trả về những sân đã được duyệt
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
                    Status = f.Status.ToString(),

                    Images = f.Images.Select(i => i.ImageUrl).ToList(),

                    AverageRating = f.Reviews.Any()
                        ? f.Reviews.Average(r => r.Rating)
                        : 0
                })
                .ToListAsync();
        }

        // Chỉ trả về những sân đã được duyệt
        public async Task<FieldResponse?> GetByIdAsync(int id)
        {
            return await _context.Fields
                .Where(f => f.Id == id && f.Status == FieldStatus.Approved)
                .Select(f => new FieldResponse
                {
                    Id = f.Id,
                    Name = f.Name,
                    Location = f.Location,
                    PricePerHour = f.PricePerHour,
                    Status = f.Status.ToString(),

                    Images = f.Images.Select(i => i.ImageUrl).ToList(),

                    AverageRating = f.Reviews.Any()
                        ? f.Reviews.Average(r => r.Rating)
                        : 0
                })
                .FirstOrDefaultAsync();
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
                    Status = f.Status.ToString(),

                    Images = f.Images.Select(i => i.ImageUrl).ToList(),

                    AverageRating = f.Reviews.Any()
                        ? f.Reviews.Average(r => r.Rating)
                        : 0
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
                    Status = f.Status.ToString(),

                    Images = f.Images.Select(i => i.ImageUrl).ToList(),

                    AverageRating = f.Reviews.Any()
                        ? f.Reviews.Average(r => r.Rating)
                        : 0
                })
                .ToListAsync();
        }
        // Tìm kiếm với nhiều điều kiện: keyword, location, price range, sort by price/rating
        public async Task<PagedResult<FieldResponse>> SearchAsync(FieldQueryRequest request)
        {
            var query = _context.Fields
                .AsNoTracking()
                .Where(f => f.Status == FieldStatus.Approved)
                .AsQueryable();

            // 🔍 SEARCH
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(f => f.Name.Contains(request.Keyword));
            }

            if (!string.IsNullOrEmpty(request.Location))
            {
                query = query.Where(f => f.Location.Contains(request.Location));
            }

            // 💰 FILTER PRICE
            if (request.MinPrice.HasValue)
            {
                query = query.Where(f => f.PricePerHour >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue)
            {
                query = query.Where(f => f.PricePerHour <= request.MaxPrice.Value);
            }

            // ⭐ SORT
            query = request.Sort switch
            {
                "price_asc" => query.OrderBy(f => f.PricePerHour),
                "price_desc" => query.OrderByDescending(f => f.PricePerHour),
                "rating" => query.OrderByDescending(f => f.Reviews.Average(r => (double?)r.Rating) ?? 0),
                _ => query.OrderByDescending(f => f.Id)
            };

            var totalItems = await query.CountAsync();

            // 📄 PAGINATION
            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(f => new FieldResponse
                {
                    Id = f.Id,
                    Name = f.Name,
                    Location = f.Location,
                    PricePerHour = f.PricePerHour,
                    Status = f.Status.ToString(),

                    Images = f.Images.Select(i => i.ImageUrl).ToList(),

                    AverageRating = f.Reviews.Any()
                        ? f.Reviews.Average(r => r.Rating)
                        : 0
                })
                .ToListAsync();

            return new PagedResult<FieldResponse>
            {
                Items = items,
                TotalItems = totalItems,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
