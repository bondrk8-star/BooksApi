using System;
using System.Collections.Generic;
using System.Text;

namespace BooksApi.Server.DTOs.AuthorDTOs.Request
{
    public class AuthorPutRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
