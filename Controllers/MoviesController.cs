using AutoMapper;
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
    public class MoviesController : ControllerBase
    {
       
        private new List<string> _AllowedExtensions = new List<string>{".jpg" , ".png" };
        private long _MaxAllowedPosterSize = 1048576;
        private readonly IMoviesService _moviesService;
        private readonly IGeneresService _generesService;
        private readonly IMapper _mapper;

        public MoviesController(IMoviesService moviesService , IGeneresService generesService ,IMapper mapper)
        {
            _moviesService = moviesService;
            _generesService = generesService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _moviesService.GetAll();
            // TODO: map movies to DTO
            var data = _mapper.Map<IEnumerable<MovieDetailsDTO>>(movies);
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        { 
           var movie = await _moviesService.GetById(id);
            if (movie == null)
                return NotFound();
            var dto = _mapper.Map<MovieDetailsDTO>(movie);
         
            return Ok(dto);   
        }

        [HttpGet("GetByGenereId")]
        public async Task<IActionResult> GetByGenereIdAsync(byte Genreid)
        {
            var movies = await _moviesService.GetAll(Genreid);
            // TODO: map movies to DTO
            var data = _mapper.Map<IEnumerable<MovieDetailsDTO>>(movies);
            return Ok(data);
            

        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] MovieDTO dto)
        {
            if (dto.Poster == null)
                return BadRequest("Poster Is Required ");
            if (!_AllowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName.ToLower())))
                return BadRequest("only .png and .jpg Extensions allowed");
            if(dto.Poster.Length > _MaxAllowedPosterSize)
                return BadRequest("Max Allowed size for poster is 1MB");
            var isValidGenere =await _generesService.IsValidGenre(dto.GenereId);
            if (!isValidGenere)
                return BadRequest(" Invalid Genere Id");
            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);
            Movie movie = _mapper.Map<Movie>(dto);
            movie.Poster = dataStream.ToArray();
       
            _moviesService.Add(movie);
            return Ok(movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromForm] MovieDTO dto)
        {
            var movie = await _moviesService.GetById(id);
            if (movie == null) 
                return BadRequest();
            var isValidGenere = await _generesService.IsValidGenre(dto.GenereId);
            if (!isValidGenere)
                return BadRequest(" Invalid Genere Id");
            if (dto.Poster != null)
            {
                if (!_AllowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName.ToLower())))
                    return BadRequest("only .png and .jpg Extensions allowed");
                if (dto.Poster.Length > _MaxAllowedPosterSize)
                    return BadRequest("Max Allowed size for poster is 1MB");
                using var dataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStream);
                movie.Poster= dataStream.ToArray();
            }
            
            movie.Title = dto.Title;
            movie.Year = dto.Year;
            movie.Rate = dto.Rate;
            movie.StoryLine = dto.StoryLine;

            _moviesService.Update(movie);
            return Ok(movie);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        { 
         Movie movie = await _moviesService.GetById(id);
            if (movie == null)
                return NotFound($"NO Movie was found with Id :{id}");
            _moviesService.Delete(movie);
            return Ok(movie);
        }


    }
}
