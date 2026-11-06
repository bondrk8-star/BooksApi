using System;
using System.Collections.Generic;
using System.Text;

namespace BooksApi.Server.DTOs.BookDTOs.Request
{
    public class BookGetListRequest
    {
        public string Search {  get; set; }
        public PaginationSettings PaginationSettings { get; set; }
    }
}
