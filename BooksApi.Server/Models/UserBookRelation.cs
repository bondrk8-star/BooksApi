using System;
using System.Collections.Generic;
using System.Text;

namespace BooksApi.Server.Models
{
    public class UserBookRelation
    {
        public Book Book { get; set; }
        public int BookId   { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
    }
}
