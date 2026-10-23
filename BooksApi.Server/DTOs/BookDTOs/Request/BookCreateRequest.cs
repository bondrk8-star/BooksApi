using System;
using System.Collections.Generic;
using System.Text;

namespace BooksApi.Server.DTOs.BookDTOs.Request
{
    public class BookCreateRequest
    {
        public string Name { get; set; }
        public int AuthorId { get; set; }
    }
}
