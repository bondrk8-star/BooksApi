using System;
using System.Collections.Generic;
using System.Text;

namespace BooksApi.Server.DTOs.UserDTOs.Response
{
    public class UserLoggerResponse
    {
        public int UserId { get; set; }
        public string Token { get; set; }
    }
}
