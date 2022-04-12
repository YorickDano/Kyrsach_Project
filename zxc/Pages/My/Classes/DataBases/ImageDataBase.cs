using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;

namespace zxc.Pages.My.Classes.DataBase
{
    public class ImageDataBase : DataBase
    {
        public void DownloadFileFromDB(string name)
        {
            byte[] Data = null;
            connection.Open();
            command.CommandText = $"SELECT * FROM Images WHERE Name='{name}'";
            command.Connection = connection;
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Data = (byte[])reader.GetValue(1);
                }
            }
            using (FileStream fs = new FileStream($@"wwwroot\img\{name}", FileMode.OpenOrCreate))
            {
                fs.Write(Data, 0, Data.Length);
            }
        }

        public void Downloading()
        {
            DownloadFilesFromDB(getNamesOfImagesFromDataBase(), GetImagesDataFromDataBase());
        }

        public string getImageStringFromDataBase(string name)
        {
            List<Image> ListOfImages = GetImagesDataFromDataBase();

            return String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(ListOfImages.Find(x => x.Name == name).Data));
        }
        public List<string> getImagesAsStringFromDatatbase()
        {
            List<string> listOfImages = new List<string>();
            foreach (var item in GetImagesDataFromDataBase())
                listOfImages.Add(String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(item.Data)));

            return listOfImages;
        }

        public List<Image> GetImagesDataFromDataBase()
        {
            List<Image> list = new List<Image>();
            connection.Open();
            command.CommandText = "SELECT * FROM Images";
            command.Connection = connection;
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                    while (reader.Read())
                    {
                        byte[] imgData = (byte[])reader.GetValue(1);
                        string name = (string)reader.GetValue(0);
                        Image image = new Image(0, name, " ", imgData);
                        list.Add(image);
                    }

            }
            return list;
        }
    }
}



