using Eagle.Domain;
using ServiceStack.DataAnnotations;
//using NPoco;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Tests.Domain.Models
{
    //[TableName("user")]
    //[PrimaryKey("Id", AutoIncrement = true)]
    [Alias("user")]
    [TablePrimaryKey("Id", ColumnName = "user_id", AutoIncrement = true)]
    [TableIgnoreColumns(new string[] { "Modified" })]
    public class User : AggregateRoot
    {
        public User() : base() { }

        //[Column("user_name")]
        [Alias("user_name")]
        public string Name { get; set; }

        //[Column("user_nick_name")]
        [Alias("user_nick_name")]
        public string NickName { get; set; }

        //[Column("user_email")]
        [Alias("user_email")]
        public string Email { get; set; }

        //[Column("user_password")]
        [Alias("user_password")]
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
