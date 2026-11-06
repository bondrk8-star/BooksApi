using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BooksApi.Server.Models
{
    public enum Roles
    {
        User,
        Admin,
        Moderator

    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public List<Book> Books { get; set; }
        public int Id { get; set; }
        public Roles Role {  get; set; }
        public List<UserBookRelation> BookRelations { get; set; }
    }
}
