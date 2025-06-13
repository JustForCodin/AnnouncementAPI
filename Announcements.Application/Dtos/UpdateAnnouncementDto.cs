namespace Announcements.Application.Dtos
{
    /// <summary>
    /// DTO for updating existing announcement.
    /// </summary>
    public class UpdateAnnouncementDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
    }   
}