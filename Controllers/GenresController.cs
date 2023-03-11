using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Models;
using MoviesAPI.Services;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGeneresService _generesService;

        public GenresController(IGeneresService generesService)
        {
            _generesService= generesService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var Genres = await _generesService.GetAll();
            return Ok(Genres);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] GenreDTO dto)
        {
            Genere genre = new() { Name = dto.Name };
            await _generesService.Add(genre);
            return Ok(genre);
        }

        [HttpPut(template: "{id}")]
        public async Task<IActionResult> UpdateAsync(byte id, [FromBody] GenreDTO dto)
        {
            var genere = await _generesService.GetById(id);
            if (genere == null)
            {
                return NotFound($"No Genere was Found with ID : {id}");
            }
            _generesService.Update(genere);
            return Ok(genere);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(byte id)
        {
            var genere = await _generesService.GetById(id);
            if (genere == null)
                return NotFound($"No Genere was Found with ID : {id}");
            _generesService.Delete(genere);
            return Ok(genere);
        }


    }
}
