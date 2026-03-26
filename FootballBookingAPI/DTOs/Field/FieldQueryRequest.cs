namespace FootballBookingAPI.DTOs.Field
{
    public class FieldQueryRequest
    {
        public string? Keyword { get; set; }
        public string? Location { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        // price_asc | price_desc | rating
        public string? Sort { get; set; }
    }
}
