using System;
using System.Collections.Generic;
using System.Text;

namespace BooksApi.Server.Models
{
    public class Book : IBook
    {
        public string Name { get ; set ; }
        public Author Author { get ; set ; }
        public int AuthorId { get ; set ; }
        public Genre Genre { get ; set ; }
        public int GenreId { get ; set ; }
        public int Id { get ; set ; }
    }
}
