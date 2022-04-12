using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace zxc.Pages.My.Classes.Objects
{
    public class CurrentUser
    {
        public int Id;
        public string Name;
        public string Email;
        public string Password;
        

        protected SqliteConnection connection = new SqliteConnection($"Data Source=DataBases\\ZXC.db");
        protected SqliteCommand command = new SqliteCommand();

        
        public CurrentUser(string name,string email,string password,string status)
        {
            Name = name;
            Email = email;
            Password = password;
          
            connection.Open();
            command.Connection = connection;
            command.CommandText = $"UPDATE CurrentUser SET Name = '{name}', Email = '{email}', Password = '{password}', Status = '{status}' WHERE Id=1 ";
            command.ExecuteNonQuery();
        }
    }
}
