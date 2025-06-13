using Announcements.Application.Dtos;
using Announcements.Domain.Entities;
using Announcements.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Announcements.Application.Services
{
    public interface IAnnouncementService
    {
        Task<IEnumerable<AnnouncementResponseDto>> GetAllAnnouncements();
        Task<AnnouncementDetailsResponseDto?> GetAnnouncementById(Guid id);
        Task<AnnouncementResponseDto> AddAnnouncement(CreateAnnouncementDto createDto);
        Task<bool> UpdateAnnouncement(Guid id, UpdateAnnouncementDto updateDto);
        Task<bool> DeleteAnnouncement(Guid id);

    }

    public class AnnouncementService : IAnnouncementService
    {
        private readonly AnnouncemetDbContext _context;

        public AnnouncementService(AnnouncemetDbContext context)
        {
            this._context = context;
        }

        public async Task<AnnouncementResponseDto> AddAnnouncement(CreateAnnouncementDto createDto)
        {
            var announcement = new Announcement
            {
                id = Guid.NewGuid(),
                Title = createDto.Title,
                Description = createDto.Description,
                DateAdded = DateTime.UtcNow
            };

            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();
            return MapToResponseDto(announcement);
        }

        public async Task<IEnumerable<AnnouncementResponseDto>> GetAllAnnouncements()
        {
            return await _context.Announcements
                .AsNoTracking()
                .OrderByDescending(a => a.DateAdded)
                .Select(a => MapToResponseDto(a))
                .ToListAsync();
        }

        public async Task<AnnouncementDetailsResponseDto?> GetAnnouncementById(Guid id)
        {
            var announcement = await _context.Announcements
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.id == id);

            if (announcement == null)
            {
                return null;
            }

            var similarAnnnouncements = await FindSimilarAnnouncements(announcement);
            return new AnnouncementDetailsResponseDto
            {
                Id = announcement.id,
                Title = announcement.Title,
                Description = announcement.Description,
                DateAdded = announcement.DateAdded,
                SimilarAnnouncements = similarAnnnouncements
            };
        }

        public async Task<bool> UpdateAnnouncement(Guid id, UpdateAnnouncementDto updateDto)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null)
            {
                return false;
            }

            announcement.Title = updateDto.Title;
            announcement.Description = updateDto.Description;

            _context.Announcements.Update(announcement);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAnnouncement(Guid id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null)
            {
                return false;
            }

            _context.Announcements.Remove(announcement);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Finds top 3 similar announcements.
        /// Similarity is defined by having at least one common word in the title or description.
        /// </summary>
        private async Task<IEnumerable<AnnouncementResponseDto>> FindSimilarAnnouncements(Announcement target)
        {
            // Get all words from the target announcement's title and description.
            // Using a HashSet for faster lookup & to get rid of duplicates.
            var targetWords = new HashSet<string>(
                (target.Title + " " + target.Description)
                .Split(new[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(word => word.ToLowerInvariant())
            );

            if (targetWords.Count == 0)
            {
                return Enumerable.Empty<AnnouncementResponseDto>();
            }

            var allOtherAnnouncements = await _context.Announcements
                .AsNoTracking()
                .Where(a => a.id != target.id)
                .ToListAsync();

            var similar = allOtherAnnouncements
                .Select(a => new
                {
                    Announcement = a,
                    Score = (a.Title + " " + a.Description)
                        .Split(new[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(word => word.ToLowerInvariant())
                        .Intersect(targetWords)
                        .Count()
                })
                .Where(x => x.Score > 0) // must have at least 1 word in common
                .OrderByDescending(x => x.Score) // order by most similar first
                .OrderByDescending(x => x.Announcement.DateAdded) // then by date
                .Take(3)
                .Select(x => MapToResponseDto(x.Announcement));

            return similar;
        }

        private static AnnouncementResponseDto MapToResponseDto(Announcement announcement)
        {
            return new AnnouncementResponseDto
            {
                Id = announcement.id,
                Title = announcement.Title,
                Description = announcement.Description,
                DateAdded = announcement.DateAdded
            };
        }
    }
}