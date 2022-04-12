using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using zxc.Pages.My.Classes.Objects;

namespace zxc.Pages.My.Classes.DataBase
{
    public class ItemDataBase : DataBase
    {
        public static bool IsRaited;

        public string GetCurrentUserStatus()
        {
            string status = null;
            connection.Open();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM CurrentUser";
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        status = reader.GetValue(4).ToString();      
                    }
                }
            }
            return status;
        }

        public void ClearStatusOfCurrentUser()
        {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "UPDATE CurrentUser SET Status = 0";
            command.ExecuteNonQuery();
        }

        public User GetCurrentUser()
        {
            User currentUser = null;
            connection.Open();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM CurrentUser";
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        currentUser = new User(reader.GetValue(1).ToString(), reader.GetValue(2).ToString(),
                            reader.GetValue(3).ToString());
                    }
                }
            }
            return currentUser;
        }

        public void UpdateRate(string itemName,string userEmail,string rate)
        {
            string score = string.Empty;

            connection.Open();
            command.CommandText = $"SELECT Score FROM About WHERE Name='{itemName}'";
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        score = reader.GetValue(0).ToString();
                    }
                }
                reader.Close();
            }
           
            List<string> listOfEmailsAndRates =new List<string>(score.Split('\t'));
            listOfEmailsAndRates.RemoveAll(x => x == "");
            score = "";
            for(int i = 0; i < listOfEmailsAndRates.Count; ++i)
            {
                if (listOfEmailsAndRates[i].Contains(userEmail))
                {
                    listOfEmailsAndRates[i] = listOfEmailsAndRates[i].Remove(0,1);
                    listOfEmailsAndRates[i] = listOfEmailsAndRates[i].Insert(0, score);
                    score += rate + " "+listOfEmailsAndRates[i] + "\t";
                    continue;
                }
              
                score += listOfEmailsAndRates[i] + "\t";
            }
            connection.Close();
            connection.Open();
            command.CommandText = $"UPDATE About SET Score='{score}' WHERE Name='{itemName}'";
            command.Connection = connection;
            command.ExecuteNonQuery();
        }

        public string GetDescriptionFromAboutTable(string name)
        {
            string desc = "";

            connection.Open();
            command.CommandText = $"SELECT Desc FROM About WHERE Name='{name}'";
            command.Connection = connection;

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        desc += (string)reader.GetValue(0);
                    }
                }
            }

            return desc;
        }

        public void GetCommentInfo(int id, ref Comments comments)
        {
            connection.Open();
            command.CommandText = $"SELECT * FROM Comments WHERE Id='{id}'";
            command.Connection = connection;

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        comments.Username = reader.GetValue(1).ToString();
                        comments.Text = reader.GetValue(2).ToString();
                        comments.Datetime = reader.GetValue(3).ToString();
                    }
                }
            }
        }

        public void GetAllInfo()
        {
            connection.Open();
            command.CommandText = "SELECT * FROM About";
            command.Connection = connection;

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Item item = new Item();
                    item.Comments = new Comments();

                    while (reader.Read())
                    {
                        item.Name = reader.GetValue(1).ToString();
                        item.Description = reader.GetValue(2).ToString();
                        GetCommentInfo((int)reader.GetValue(3), ref item.Comments);
                        item.Score = (double)reader.GetValue(4);
                    }
                }
            }
        }

        public string GetCurrentUserEmail()
        {
            string email = string.Empty;
            connection.Open();
            command.Connection = connection;
            command.CommandText = $"SELECT Email FROM CurrentUser WHERE Id='1'";
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        try
                        {
                            email = reader.GetValue(0).ToString();
                        }
                        catch { }
                    }
                }
            }
            return email;  
        }
        public string GetRate(string itemName)
        {
            string userEmail = GetCurrentUserEmail();
            string rateAndEmail = string.Empty;
            IsRaited = false;
            connection.Open();
            command.Connection = connection;
            command.CommandText = $"SELECT Score FROM About WHERE Name='{itemName}'";
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        try
                        {
                            rateAndEmail = reader.GetValue(0).ToString();
                        }
                        catch { }
                    }
                }
            }
            var listOfRatesAndEmails = new List<string>(rateAndEmail.Split('\t'));
            if (listOfRatesAndEmails[0] == "")
            {
                return null;
            }
            try
            {
                return listOfRatesAndEmails.Find(x => x.Contains(userEmail)).Split(' ')[0][0].ToString();
            }
            catch { return null; }
        }

        public void UpdateScore(string itemName, string newScore,string userEmail)
        {
            IsRaited = false;
            connection.Open();
            command.Connection = connection;
            command.CommandText = $"SELECT Score FROM About WHERE Name='{itemName}'";
            SqliteDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    try
                    {
                       if(reader.GetValue(0).ToString().Contains(userEmail))
                       {
                            UpdateRate(itemName, userEmail,newScore);
                            return;
                       }
                    }
                    catch {}
                }
            }
            reader.Close();
            command.CommandText = $"SELECT CountOfRates FROM About WHERE Name='{itemName}'";
            int count = 0;
           
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        try
                        {
                            count = Convert.ToInt32(reader.GetValue(0));
                        }
                        catch { count = 0; }
                    }
                }
            reader.Close();
            command.CommandText = $"SELECT Score From About WHERE Name='{itemName}'";
            string scores = "";
            if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        try
                        {
                            scores = reader.GetValue(0).ToString();
                        }
                        catch { scores = ""; }
                    }
                }
           
            if (scores != "")
            {
                scores += newScore+" "+ userEmail + "\t";                
            }
            else
            {
                scores = newScore + " " + userEmail + "\t";
            }
            reader.Close();
            command.CommandText = $"UPDATE About SET CountOfRates='{count+1}' WHERE Name='{itemName}'";
            command.ExecuteNonQuery();
            command.CommandText = $"UPDATE About SET Score='{scores}' WHERE Name='{itemName}'";
            command.ExecuteNonQuery();
        }

        public string GetScore(string name)
        {
            connection.Open();
            command.Connection = connection;
            command.CommandText = $"SELECT Score FROM About WHERE Name='{name}'";
            string score = "";
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        try
                        {
                            score = reader.GetValue(0).ToString();
                        }
                        catch { score = ""; }
                    }
                }
            }
            double sumOfScores = 0;
            var listOfScores = new List<string>(score.Split('\t'));
            listOfScores.RemoveAll(x => x == "");
            listOfScores.ForEach(x => sumOfScores += Char.GetNumericValue(x.Split(' ')[0][0]));
            return (sumOfScores / listOfScores.Count).ToString();
        }

        public string GetAllCommnts()
        {
            connection.Open();
            command.Connection = connection;
            command.CommandText = $"SELECT * FROM About";
            return null;
        }

        public string GetCountOfRates(string name)
        {
            connection.Open();
            command.Connection = connection;
            command.CommandText = $"SELECT CountOfRates FROM About WHERE Name='{name}'";
            string count = "";
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        try
                        {
                            count = reader.GetValue(0).ToString();
                        }
                        catch { count = ""; }
                    }
                }
            }
            return count;
        }

        public void addingInfoInAboutTable(string name, string desc)
        {
            connection.Open();
            command.Parameters.Clear();
            command.Parameters.Add(new SqliteParameter("@Name", name));
            command.Parameters.Add(new SqliteParameter("@Desc", desc));
            command.ExecuteNonQuery();

        }

        public void AddInfoInAboutTable(string name, string desc)
        {
            connection.Open();
            command.Connection = connection;
            command.CommandText = @"INSERT INTO About (Name, Desc)
                                    VALUES (@Name, @Desc)";
            addingInfoInAboutTable(name, desc);
            connection.Close();
        }
    }
}
