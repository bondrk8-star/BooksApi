using BooksApi.Server.DTOs.BookDTOs.Request;
using BooksApi.Server.DTOs.BookDTOs.Response;
using BooksApi.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;

namespace BooksApi.Server.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BookController: ControllerBase
    {
        private readonly PostgresContext dbContext;
        private readonly UsersSettings options;
        public BookController(PostgresContext dbContext, IOptions<UsersSettings> options) 
        {
            this.dbContext = dbContext;
            this.options = options.Value;
            var json = this.options.ToJson(Formatting.Indented); //JsonConvert.SerializeObject(this.options, Formatting.Indented);
            this.options = null;
            this.options = json.FromJson<UsersSettings>();//JsonConvert.DeserializeObject<UsersSettings>(json);
        }

        [HttpPost]
        async public Task<ActionResult<BookCreateResponse>> Create([FromBody] BookCreateRequest request)
        {
            var author = await dbContext.Authors.Where(a => a.Id == request.AuthorId).Select(a => a.Name).FirstOrDefaultAsync();
            if (author == null) return NotFound("Автор не найден");
            
            var AuthorBooks = await dbContext.Books.Where(b => b.AuthorId == request.AuthorId).Select(b => b.Name).ToListAsync();
            if (AuthorBooks.Contains(request.Name)) return Conflict("Такая книга есть");

            if (options.AccesType != 0) return Forbid("Нет доступа");

            var NewBook = new Book() { Name = request.Name, AuthorId = request.AuthorId, GenreId = 0};
            
            var Genre = (await dbContext.Genres.FirstOrDefaultAsync(a => a.Id == 0)).Name;

            await dbContext.Books.AddAsync(NewBook);
            await dbContext.SaveChangesAsync();
            return new BookCreateResponse
            {
                Name = request.Name,
                AuthorName = author,
                GenreName = Genre
            };
        }

        [HttpGet]
        async public Task<ActionResult<BookGetResponse>> Get([FromQuery] BookGetRequest request)
        {
            var GetBook = await dbContext.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == request.Id);
            if (GetBook == null) return NotFound("Книга не найдена");

            return new BookGetResponse
            {
                Name = GetBook.Name,
                Id = GetBook.Id,
                Genre = GetBook.Genre.Name,
                AuthorName = GetBook.Author.Name
            };
        }

        [HttpPut]
        async public Task<ActionResult<BookPutResponse>> Put([FromBody] BookPutRequest request)
        {
            var book = await dbContext.Books.FirstOrDefaultAsync(b => b.Name == request.Name);
            if (book == null) return NotFound("Книга не найдена");
           
            book.Name = request.ChangeName;
            await dbContext.SaveChangesAsync();

            return new BookPutResponse
            {
                Name = book.Name
            };
        }

        [HttpDelete]
        async public Task<ActionResult<BookDeleteResponse>> Delete([FromBody] BookDeleteRequest request)
        {
            var book = await dbContext.Books.AnyAsync(b => b.Id == request.Id);
            if (book == null) return NotFound("Книга не найдена");
            dbContext.Books.Remove(new Book() { Id = request.Id});
            await dbContext.SaveChangesAsync();

            return new BookDeleteResponse
            {
                Result = true
            };
        }
    }
}
