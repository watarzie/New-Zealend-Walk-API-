namespace NZWalksAPI.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; } // Unique Id for Walk
        public string Name { get; set; } 
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }

        public Guid DifficultyId { get; set; } // our relation property relate Diffuculty model  to Walk model

        public Guid RegionId { get; set; } // our relation property relate Region model to Walk model

        // Navigation properties
        public Difficulty Difficulty { get; set; }
        public Region Region { get; set; } // one to one 
    }
}
