using System;
using System.Collections.Generic;
using System.Text;

namespace BooksApi.Server
{
    public enum AccesType 
    {
        Admin,
        User
    }

    public class UsersSettings
    {
        public AccesType AccesType { get; set; }
        public string Name { get; set; }
    }
}
