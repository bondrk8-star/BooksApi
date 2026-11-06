using BooksApi.Server.Models;
using System;
using System.Collections.Generic;
using System.Text;
using BooksApi.Server.DTOs;
using BooksApi.Server.DTOs.AuthorDTOs;
using BooksApi.Server.DTOs.GenreDTOs;

namespace BooksApi.Server.DTOs.BookDTOs
{
    public class BookItem
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public AuthorItem Author { get; set; }
        public GenreItem Genre { get; set; }

    }
}
