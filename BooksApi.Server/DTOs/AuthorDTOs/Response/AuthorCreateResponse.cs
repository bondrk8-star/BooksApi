using BooksApi.Server.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BooksApi.Server.DTOs.AuthorDTOs.Response
{
    public class AuthorCreateResponse
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int BookCount { get; set; }
    }
}
