using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcMusic.Data;
using MvcMusic.Models;
using Projet_ASP.NET.Models;

namespace Projet_ASP.NET.Controllers
{
    [Route("api/Musics")]
    [ApiController]
    public class MusicsControllerAPI : ControllerBase
    {
        private readonly MvcMusicContext _context;

        public MusicsControllerAPI(MvcMusicContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MusicDTO>>> GetMusics()
        {
            return await _context.Music
                .Select(x => MusicToDTO(x))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MusicDTO>> GetMusicItem(long id)
        {
            var todoItem = await _context.Music.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return MusicToDTO(todoItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMusicItems(int id, MusicDTO musicDTO)
        {
            if (id != musicDTO.Id)
            {
                return BadRequest();
            }

            var musicItem = await _context.Music.FindAsync(id);
            if (musicItem == null)
            {
                return NotFound();
            }

            musicItem.Title = musicDTO.Title;
            musicItem.ReleaseDate = musicDTO.ReleaseDate;
            musicItem.Genre = musicDTO.Genre;
            musicItem.Price = musicItem.Price;
            musicItem.Creator = musicItem.Creator;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!MusicItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Music>> CreateMusic(Music musicItem)
        {
            var music = new Music
            {
                Title = musicItem.Title,
                ReleaseDate = musicItem.ReleaseDate,
                Genre = musicItem.Genre,
                Price = musicItem.Price,
                Creator = musicItem.Creator,
                IsValidated = musicItem.IsValidated,
            };

            _context.Music.Add(music);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetMusicItem),
                new { id = musicItem.Id },
                MusicToDTO(music));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMusicItem(long id)
        {
            var todoItem = await _context.Music.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.Music.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MusicItemExists(long id) =>
             _context.Music.Any(e => e.Id == id);

        private static MusicDTO MusicToDTO(Music music) =>
            new MusicDTO
            {
                Id = music.Id,
                Title = music.Title,
                ReleaseDate = music.ReleaseDate,
                Genre = music.Genre,
                Price = music.Price,
            };
    }
}
