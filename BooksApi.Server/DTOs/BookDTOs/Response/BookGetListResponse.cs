using System;
using System.Collections.Generic;
using System.Text;

namespace BooksApi.Server.DTOs.BookDTOs.Response
{
    public class BookGetListResponse
    {
        public List<BookItem> Books { get; set; }
    }
}
