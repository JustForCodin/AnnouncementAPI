namespace Announcements.Application.Dtos
{
    /// <summary>
    /// Detailed DTO for a single view, including similar announcements.
    /// </summary>
    public class AnnouncementDetailsResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; }
        public IEnumerable<AnnouncementResponseDto> SimilarAnnouncements { get; set; } = [];

    }
}