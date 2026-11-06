using BooksApi.Server.Data;
using BooksApi.Server.DTOs.AuthorDTOs.Request;
using BooksApi.Server.DTOs.AuthorDTOs.Response;
using BooksApi.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BooksApi.Server.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly PostgresContext dbContext;

        public AuthorController(PostgresContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        async public Task<ActionResult<AuthorCreateResponse>> Create([FromBody] AuthorCreateRequest request)
        {
            var author = new Author() { Name = request.Name};

            if (await dbContext.Authors.AnyAsync(a => a.Name == author.Name)) return Conflict("Такой автор уже есть");

            await dbContext.Authors.AddAsync(author);
            await dbContext.SaveChangesAsync();
            return new AuthorCreateResponse()
            {
                Name = request.Name,
                Id = author.Id,
                BookCount = dbContext.Books.Count(b => b.AuthorId == author.Id)
            };
        }

        [HttpGet]
        async public Task<ActionResult<AuthorGetResponse>> Get([FromQuery] AuthorGetRequest request)
        {
            IQueryable<Author> sql = dbContext.Authors.OrderBy(a => a.Name);
            Console.WriteLine("Пойдет в базу данных: " + sql.ToQueryString());
            if(request.Name != null)
            {
                sql = sql.Where(a => a.Name.ToLower().Contains(request.Name.ToLower()));
            }
            Console.WriteLine("Пойдет в базу данных: " + sql.ToQueryString());
            var authors = await sql.ToListAsync();

            if (!authors.Any()) return NotFound("Нету");

            var AuthorsIds = authors.Select( a => a.Id).ToList();

            var BooksCount = await dbContext.Books.Where(b => AuthorsIds.Contains(b.AuthorId)).GroupBy(b => b.AuthorId).ToDictionaryAsync(k => k.Key, v => v.Count());

            return new AuthorGetResponse()
            {
                Authors = authors.Select(a => new AuthorCreateResponse() { Name = a.Name, Id = a.Id, BookCount = BooksCount[a.Id] }).ToList()
            };
        }

        [HttpDelete]
        async public Task<ActionResult<AuthorDeleteResponse>> Delete([FromBody] AuthorDeleteRequest request)
        {
            var author = await dbContext.Authors.AnyAsync(a => a.Id == request.Id);
            if (author == false) return NotFound("Автор не найден, удаление не прошло");

            dbContext.Authors.Remove(new Author { Id = request.Id});
            await dbContext.SaveChangesAsync();
            return new AuthorDeleteResponse()
            {
                Result = true

            };
        }

        [HttpPut]
        async public Task<ActionResult<AuthorPutResponse>> Update([FromBody]AuthorPutRequest request)
        {
            var author = await dbContext.Authors.FirstOrDefaultAsync(a => a.Id == request.Id);
            if (author == null) return NotFound("Автор не найден, изменить не удалось");

            author.Name = request.Name;
            await dbContext.SaveChangesAsync();
            return new AuthorPutResponse()
            {
                Id = request.Id,
                Name = author.Name
            };
        }
    }
}
