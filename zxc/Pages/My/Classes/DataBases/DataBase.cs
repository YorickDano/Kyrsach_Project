using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;

using System.Threading.Tasks;
using zxc.Pages.My.Classes.Objects;

namespace zxc.Pages.My.Classes.DataBase
{
    public class DataBase
    {
        protected SqliteConnection connection = new SqliteConnection($"Data Source=DataBases\\ZXC.db");
        protected SqliteCommand command = new SqliteCommand();

        public void createDataBase(string str)
        {
            SqliteConnection connection = new SqliteConnection($"Data Source=DataBases\\{str}.db");
            this.connection = connection;
            if (File.Exists(@$"DataBases\{str}.db"))
                return;
            else
                File.Create(@$"DataBases\{str}.db");
        }

        

        public void setDataBaseFromFileName(string name)
        {
            byte[] array;
            using (FileStream fs = new FileStream(@$"wwwroot\img\{name}", FileMode.Open))
            {
                array = new byte[fs.Length];
                fs.Read(array, 0, array.Length);
            }

            connection.Open();
            command.Connection = connection;
            command.CommandText = @"INSERT INTO Images (Name, Image)
                                    VALUES (@Name, @Image)";
            addingInDataBase(array, name);
            connection.Close();
            
        }



        public void createTableInDataBase(string name)
        {
            connection.Open();
            command.Connection = connection;
            command.CommandText = $"CREATE TABLE {name}(Name TEXT NOT NULL,Desc TEXT)";
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void setDataBaseFromDirectory()
        {
            DirectoryInfo directory = new DirectoryInfo(@"wwwroot\img");
            List<string> imagesFullName = new List<string>();
            List<string> imagesName = new List<string>();
            List<byte[]> imagesData = new List<byte[]>();
            foreach (var file in directory.GetFiles())
            {
                imagesFullName.Add(file.FullName);
                imagesName.Add(file.Name);
            }

            for (int i = 0; i < imagesFullName.Count; ++i)
            {
                using (FileStream fs = new FileStream(imagesFullName[i], FileMode.Open))
                {
                    imagesData.Add(new byte[fs.Length]);
                    fs.Read(imagesData[i], 0, imagesData[i].Length);
                }
            }

            connection.Open();
            command.Connection = connection;
            command.CommandText = @"INSERT INTO Images (Name, Image)
                                    VALUES (@Name, @Image)";
            for (short i = 0; i < imagesName.Count; ++i)
                addingInDataBase(imagesData[i], imagesName[i]);

            connection.Close();
        }
        public void deleteDublicationsFromDataBase()
        {
            connection.Open();
            command.Connection = connection;
            command.CommandText = $@"DELETE FROM Images 
                                    WHERE ROWID NOT IN(
                                    SELECT MIN(ROWID)
                                    FROM Images
                                    GROUP BY Name)";
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void Deletion(string name)
        {
            deleteFromDataBase(name);
            deleteFromDirectory(name);
        }
        public void deleteFromDataBase(string name)
        {
            connection.Open();
            command.Connection = connection;
            command.CommandText = $"DELETE FROM Images WHERE Name='{name}'";
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void deleteFromDirectory(string name)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(@$"wwwroot\img\");
            foreach (var item in directoryInfo.GetFiles())
                if (item.Name.Remove(item.Name.IndexOf(".")) == name)
                    item.Delete();
        }

        public void DeleteAll()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(@$"wwwroot\img\");
            foreach (var item in directoryInfo.GetFiles())
                item.Delete();

        }
        public void DownloadFilesFromDB(List<string> names, List<Image> list)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(@$"wwwroot\img\");
            List<string> listOfFilesName = new List<string>();
            if (directoryInfo.GetFiles().Count() != 0)
                foreach (var item in directoryInfo.GetFiles())
                    listOfFilesName.Add(item.Name);

            if (list.Count > 0)
            {
                int index = 0;
                foreach (var i in names)
                {
                    if (listOfFilesName.Contains(i))
                    {
                        ++index;
                        continue;
                    }
                    using (FileStream fs = new FileStream(@$"wwwroot\img\{i}", FileMode.OpenOrCreate))
                    {
                        fs.Write(list[index].Data, 0, list[index].Data.Length);
                    }
                    ++index;
                }
            }
        }

    

        public List<string> getNamesOfImagesFromDataBase()
        {
            List<string> listOfNames = new List<string>();
            using (SqliteConnection connection = new SqliteConnection($"Data Source=DataBases\\Images.db"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand("SELECT * FROM Images", connection);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                        while (reader.Read())
                            listOfNames.Add((string)reader.GetValue(0));

                }
                return listOfNames;
            }
        }
        

        public void addingInDataBase(byte[] image, string name)
        {
            connection.Open();
            command.Parameters.Clear();
            command.Parameters.Add(new SqliteParameter("@Name", name));
            command.Parameters.Add(new SqliteParameter("@Image", image));
            command.ExecuteNonQuery();
        }
    }
}
