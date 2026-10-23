using BooksApi.Server.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BooksApi.Server.DTOs.AuthorDTOs.Request
{
    public class AuthorCreateRequest
    {
        public string Name { get; set; }
    }
}
