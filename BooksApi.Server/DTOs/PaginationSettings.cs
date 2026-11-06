using System;
using System.Collections.Generic;
using System.Text;

namespace BooksApi.Server.DTOs
{
    public class PaginationSettings
    {
        public int PageSize {  get; set; }
        public int Page { get; set; }
    }
}
