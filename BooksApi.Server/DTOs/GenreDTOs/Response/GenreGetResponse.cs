using System;
using System.Collections.Generic;
using System.Text;

namespace BooksApi.Server.DTOs.GenreDTOs.Response
{
    public class GenreGetResponse
    {
        public List<GenreCreateResponse> Genres {  get; set; }
    }
}
