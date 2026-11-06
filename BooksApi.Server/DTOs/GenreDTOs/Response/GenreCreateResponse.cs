using System;
using System.Collections.Generic;
using System.Text;

namespace BooksApi.Server.DTOs.GenreDTOs.Response
{
    public class GenreCreateResponse
    {
        public string Name { get; set; }
        public int BookCount { get; set; }
    }
}
