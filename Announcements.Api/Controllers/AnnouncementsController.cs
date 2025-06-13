using System.Net.Sockets;
using Announcements.Application.Dtos;
using Announcements.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Announcements.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnnouncementsController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementsController(IAnnouncementService announcementService)
        {
            this._announcementService = announcementService;
        }

        /// <summary>
        /// Retrives a list of all announcements
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AnnouncementResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var announcements = await _announcementService.GetAllAnnouncements();
            return Ok(announcements);
        }

        /// <summary>
        /// Retrives a specific announcement by it's id, plus all similar announcements.
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(AnnouncementDetailsResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var announcement = await _announcementService.GetAnnouncementById(id);
            if (announcement == null)
            {
                return NotFound();
            }
            return Ok(announcement);
        }

        /// <summary>
        /// Creates a new announcement.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(AnnouncementResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateAnnouncementDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newAnnouncement = await _announcementService.AddAnnouncement(createDto);
            return CreatedAtAction(nameof(GetById), new { id = newAnnouncement.Id }, newAnnouncement);
        }

        /// <summary>
        /// Updates an existing announcement.
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAnnouncementDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var success = await _announcementService.UpdateAnnouncement(id, updateDto);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        /// <summary>
        /// Deletes an announcement by id.
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _announcementService.DeleteAnnouncement(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }   
}