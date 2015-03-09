using Eagle.Domain;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Tests.Domain.Models
{
    [TableName("user")]
    [PrimaryKey("Id", AutoIncrement = true)]
    public class User : AggregateRoot
    {
        public User() : base() { }

        [Column("user_name")]
        public string Name { get; set; }

        [Column("user_nick_name")]
        public string NickName { get; set; }

        [Column("user_email")]
        public string Email { get; set; }

        [Column("user_password")]
        public string Password { get; set; }

        public static User Create(string name, string email, string password) 
        {
            User user = new User();

            user.Name = name;
            user.NickName = name;
            user.Email = email;
            user.Password = password;

            return user;
        }
    }
}
