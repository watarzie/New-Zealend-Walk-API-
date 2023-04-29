using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Models.DTO
{
    public class UpdateWalkRequestDto
    {
        // We cant update Id because it's unique so we create our dto model like this.
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }

    }
}
