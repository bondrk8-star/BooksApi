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
            if (dbContext.Genres.Where(g => g.Name == request.Name) != null) return Conflict("Такой жанр уже есть");
            var genre = new Genre() { Name = request.Name};
            await dbContext.Genres.AddAsync(genre);
            await dbContext.SaveChangesAsync();
            return new GenreCreateResponse
            {
                Name = genre.Name
            };
        }
    }
}
