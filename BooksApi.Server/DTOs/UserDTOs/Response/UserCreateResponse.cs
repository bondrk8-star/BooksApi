using System;
using System.Collections.Generic;
using System.Text;

namespace BooksApi.Server.DTOs.UserDTOs.Response
{
    public class UserCreateResponse
    {
        public string Username {  get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }
    }
}
