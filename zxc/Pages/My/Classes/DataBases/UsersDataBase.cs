using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using zxc.Pages.My.Classes.Objects;

namespace zxc.Pages.My.Classes.DataBase
{
    public class UsersDataBase : DataBase
    {

        public void CreateTableInDataBase()
        {
            connection.Open();
            //SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            command.Connection = connection;
            command.CommandText = $"CREATE TABLE Users(Id INTEGER PRIMARY KEY AUTOINCREMENT,Name TEXT NOT NULL,Email TEXT NOT NULL, Password TEXT NOT NULL, CreatedDate TEXT NOT NULL,LastUpdatedDate TEXT NOT NULL,Status TEXT NOT NULL)";
            command.ExecuteNonQuery();
            connection.Close();
        }

        public Boolean IsThisEmailExist(string email)
        {
            SqliteCommand command = new SqliteCommand($@"SELECT * FROM Users WHERE Email='{email}'", connection);
            connection.Open();
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsThisUserExistAndDoesntBan(string email, string password)
        {
            SqliteCommand command = new SqliteCommand($@"SELECT * FROM Users WHERE Email='{email}' AND Password='{password}'", connection);
            connection.Open();
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    return true;
                }
            }
            return false;
        }

        public User GetUserFromDataBaseByEmail(string email)
        {
            connection.Open();
            User user = null;
            SqliteCommand command = new SqliteCommand("SELECT * FROM Users", connection);
            command.CommandText = $@"SELECT * FROM Users WHERE Email='{email}'";
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                    while (reader.Read())
                    {
                        user = new User(Convert.ToInt32(reader.GetValue(0)), reader.GetValue(1).ToString(),
                            reader.GetValue(2).ToString(), reader.GetValue(3).ToString(), reader.GetValue(4).ToString(),
                            reader.GetValue(5).ToString(), reader.GetValue(6).ToString());
                    }
            }

            connection.Close();
            return user;
        }
        public List<int> GetAllId()
        {
            var Ids = new List<int>();
            command.Connection = connection;

            command.CommandText = $@"SELECT Id FROM Users";

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                    while (reader.Read())
                    {
                        Ids.Add(Convert.ToInt32(reader.GetValue(0)));

                    }
            }
            return Ids;
        }

        public  void ChangeUsersStatus(List<User> users, string status)
        {
            command.Connection = connection;
            foreach (var user in users)
            {
                connection.Open();
                command.CommandText = $"UPDATE Users SET Status='{status}' WHERE Id='{user.Id}'";
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public  void UpdateLastEnterDate(string email)
        {
            connection.Open();
            command.Connection = connection;
            command.CommandText = $"UPDATE Users SET LastUpdatedDate='{DateTime.Now.ToLocalTime()}' WHERE Email='{email}'";
            command.ExecuteNonQuery();
        }

        public  void DeleteUser(List<User> users)
        {
            connection.Open();
            command.Connection = connection;
            foreach (var user in users)
            {
                command.CommandText = $"DELETE FROM Users WHERE Id='{user.Id}'";
                command.ExecuteNonQuery();
            }
        }
        public List<User> GetAllUsersFromDataBase()
        {
            List<User> users = new List<User>();
            SqliteCommand command = new SqliteCommand("SELECT * FROM Users", connection);
            connection.Open();
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                    while (reader.Read())
                    {
                        users.Add(new User(Convert.ToInt32(reader.GetValue(0)), reader.GetValue(1).ToString(),
                            reader.GetValue(2).ToString(), reader.GetValue(3).ToString(), reader.GetValue(4).ToString(),
                            reader.GetValue(5).ToString(), reader.GetValue(6).ToString()));

                    }
            }
            return users;
        }

        public void AddInfoInTable(string email, string name, string password, string createdDate)
        {
            connection.Open();
            command.Connection = connection;
            command.CommandText = @"INSERT INTO Users (Name, Email, Password, CreatedDate, LastUpdatedDate, Status)
                                    VALUES (@Name, @Email, @Password, @CreatedDate, @LastUpdatedDate, @Status)";

            addingInfoInAboutTable(name, email, password, createdDate, createdDate, "UnLock");
            connection.Close();

        }
        public void addingInfoInAboutTable(string name, string email, string password, string createdDate, string lastUpdatedDate, string status)
        {
            connection.Open();
            command.Parameters.Clear();
            command.Parameters.Add(new SqliteParameter("@Name", name));
            command.Parameters.Add(new SqliteParameter("@Email", email));
            command.Parameters.Add(new SqliteParameter("@Password", password));
            command.Parameters.Add(new SqliteParameter("@CreatedDate", createdDate));
            command.Parameters.Add(new SqliteParameter("@LastUpdatedDate", lastUpdatedDate));
            command.Parameters.Add(new SqliteParameter("@Status", status));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
