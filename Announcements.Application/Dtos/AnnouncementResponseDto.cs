namespace Announcements.Application.Dtos
{
    /// <summary>
    /// DTO for representing an announcement in a list view.
    /// </summary>
    public class AnnouncementResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; }
    }
}