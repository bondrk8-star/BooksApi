using BooksApi.Server.Data;
using BooksApi.Server.DTOs.GenreDTOs.Request;
using BooksApi.Server.DTOs.GenreDTOs.Response;
using BooksApi.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BooksApi.Server.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]

    public class GenreController: ControllerBase
    {
        private readonly PostgresContext dbContext;

        public GenreController(PostgresContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        async public Task<ActionResult<GenreCreateResponse>> Create([FromBody] GenreCreateRequest request)
        {
            if (await dbContext.Genres.AnyAsync(g => g.Name == request.Name)) return Conflict("Такой жанр уже есть");
            var genre = new Genre() { Name = request.Name};
            await dbContext.Genres.AddAsync(genre);
            await dbContext.SaveChangesAsync();
            return new GenreCreateResponse
            {
                Name = genre.Name
            };
        }

        [HttpGet]
        async public Task<ActionResult<GenreGetResponse>> Get([FromQuery] GenreGetRequest request)
        {
            IQueryable<Genre> genreData = dbContext.Genres.OrderBy(g => g.Name);
            if(request.Name != null)
            {
                genreData = genreData.Where(g => g.Name.ToLower().Contains(request.Name.ToLower()));
            }

            var genres = await genreData.ToListAsync();
            if (!genres.Any()) return NotFound("Такого жанра нет");

            var genresIds = genres.Select(g => g.Id).ToList();

            var BooksCount = await dbContext.Books.Where(b => genresIds.Contains(b.GenreId)).GroupBy(b => b.GenreId).ToDictionaryAsync(k => k.Key, v => v.Count());

            return new GenreGetResponse
            {
                Genres = genres.Select(g => new GenreCreateResponse() { Name = g.Name, BookCount = BooksCount[g.Id] }).ToList()
            };
        }
    }
}
