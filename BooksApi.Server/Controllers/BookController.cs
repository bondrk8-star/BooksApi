using BooksApi.Server.Data;
using BooksApi.Server.DTOs.BookDTOs;
using BooksApi.Server.DTOs.BookDTOs.Request;
using BooksApi.Server.DTOs.BookDTOs.Response;
using BooksApi.Server.Extensions;
using BooksApi.Server.Models;
using Microsoft.AspNetCore.Authorization;
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
        public BookController(PostgresContext dbContext) 
        {
            this.dbContext = dbContext;
          
        }

        [HttpPost]
        async public Task<ActionResult<BookCreateResponse>> Create([FromBody] BookCreateRequest request)
        {
            var author = await dbContext.Authors.Where(a => a.Id == request.AuthorId).Select(a => a.Name).FirstOrDefaultAsync();
            if (author == null) return NotFound("Автор не найден");
            
            var AuthorBooks = await dbContext.Books.Where(b => b.AuthorId == request.AuthorId).Select(b => b.Name).ToListAsync();
            if (AuthorBooks.Contains(request.Name)) return Conflict("Такая книга есть");

            var genre = await dbContext.Genres.Where(g => g.Id == request.GenreId).Select(g => g.Name).FirstOrDefaultAsync();
            if (genre == null) return NotFound("Жанр не найден");

            

            var NewBook = new Book() { Name = request.Name, AuthorId = request.AuthorId, GenreId = request.GenreId};

            await dbContext.Books.AddAsync(NewBook);
            await dbContext.SaveChangesAsync();
            return new BookCreateResponse
            {
                Name = NewBook.Name,
                AuthorName = author,
                GenreName = genre
            };
        }

        [HttpGet, Authorize(Policy = "Admin")]
        async public Task<ActionResult<BookGetResponse>> Get([FromQuery] BookGetRequest request)
        {
            var GetBook = await dbContext.Books.FirstOrDefaultAsync(b => b.Id == request.Id);
            if (GetBook == null) return NotFound("Книга не найдена");

            var genreName = await dbContext.Genres.Where(g => g.Id == GetBook.GenreId).Select(g => g.Name).FirstOrDefaultAsync();

            var authorName = await dbContext.Authors.Where(a => a.Id == GetBook.AuthorId).Select(a => a.Name).FirstOrDefaultAsync();

            var userClaims = User.Claims;

            return new BookGetResponse
            {
                Name = GetBook.Name,
                Id = GetBook.Id,
                Genre = genreName,
                AuthorName = authorName
            };
        }

        public async Task<ActionResult<BookGetListResponse>> GetList([FromQuery] BookGetListRequest request)
        {
            IQueryable<Book> sql = dbContext.Books.OrderBy(a => a.Name);

            if (request.Search != null)
            {
                sql = sql.Where(a => a.Name.ToLower().Contains(request.Search.ToLower()));
            }

            var books = await sql.Skip((request.PaginationSettings.Page - 1) * request.PaginationSettings.Page).Take(request.PaginationSettings.PageSize).Select(b => new { b.AuthorId, b.GenreId, Item = new BookItem() { Name = b.Name, Id = b.Id } }).ToListAsync();
            


            return new BookGetListResponse
            {
                Books = books
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
