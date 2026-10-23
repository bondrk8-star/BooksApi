using System;
using System.Collections.Generic;
using System.Text;

namespace BooksApi.Server.DTOs.AuthorDTOs.Response
{
    public class AuthorGetResponse
    {
        public List<AuthorCreateResponse> Authors { get; set; }
    }
}
