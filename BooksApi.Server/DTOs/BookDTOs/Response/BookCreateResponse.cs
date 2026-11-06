using System;
using System.Collections.Generic;
using System.Text;

namespace BooksApi.Server.DTOs.BookDTOs.Response
{
    public class BookCreateResponse
    {
        public string Name { get; set; }
        public string AuthorName { get; set; }
        public string GenreName { get; set; }
    }
}
