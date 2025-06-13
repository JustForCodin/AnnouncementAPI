namespace Announcements.Domain.Entities
{
    public class Announcement
    {
        public Guid id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }

        /// <summary>
        /// Date and time when announcement object was created.
        /// Stored in UTC format to ensure time zone consistency.
        /// </summary>
        public DateTime DateAdded { get; set; }
    }
}