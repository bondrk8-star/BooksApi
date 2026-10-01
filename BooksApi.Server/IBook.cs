using BooksApi.Server.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BooksApi.Server
{
    public interface IBook
    {
        public string Name { get; set; }
        public Author Author { get; set; }
        public Genre Genre { get; set; }
        public int Id { get; set; }
        
    }
}
