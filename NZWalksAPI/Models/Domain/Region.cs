namespace NZWalksAPI.Models.Domain
{
    public class Region
    {
        public Guid Id { get; set; } // Unique Id for Region
        public string Code { get; set; } // For example the country is Turkey then the code is TR etc.
        public string Name { get; set; } // Name of the Region
        public string? RegionImageUrl { get; set; } // This is our image property for country flags
    }
}
