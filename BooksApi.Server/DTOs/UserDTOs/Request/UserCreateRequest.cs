using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BooksApi.Server.DTOs.UserDTOs.Request
{
    public class UserCreateRequest
    {
        public string Username { get; set; }

        [Required, StringLength(8, MinimumLength = 3)]
        public string Password { get; set; }

    }
}
