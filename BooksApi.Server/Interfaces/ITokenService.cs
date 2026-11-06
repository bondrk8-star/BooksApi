using System;
using System.Collections.Generic;
using System.Text;
using BooksApi.Server.Models;

namespace BooksApi.Server.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
