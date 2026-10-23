using System;
using System.Collections.Generic;
using System.Text;

namespace BooksApi.Server.DTOs.BookDTOs.Request
{
    public class BookPutRequest
    {
        public string Name { get; set; }
        public string ChangeName { get; set; }
    }
}
