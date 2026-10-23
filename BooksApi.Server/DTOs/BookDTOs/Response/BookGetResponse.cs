using System;
using System.Collections.Generic;
using System.Text;

namespace BooksApi.Server.DTOs.BookDTOs.Response
{
    public class BookGetResponse
    {
        public string Name { get; set; }
        public string Genre { get; set; }
        public string AuthorName { get; set; }
        public int Id { get; set; }
    }
}
