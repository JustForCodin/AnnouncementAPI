namespace Announcements.Application.Dtos
{
    /// <summary>
    /// DTO for creating a new announcement.
    /// </summary>
    public class CreateAnnouncementDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
    }   
}