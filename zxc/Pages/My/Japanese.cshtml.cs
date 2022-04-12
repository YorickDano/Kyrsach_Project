using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using zxc.Pages.My.Classes;
using zxc.Pages.My.Classes.DataBase;

namespace zxc.Pages.My
{
   
    public class JapaneseModel : PageModel
    {
        Random rand = new Random();
        public string name { get; set; }
        public string Id { get; set; }

        public void OnGet(string id)
        {
           name = "ZxcGhoul";
            Id = id;
        }

        public void TEMP()
        {
            ImageDataBase db = new ImageDataBase();
            db.setDataBaseFromDirectory();
        }
        public string randomJapaneseText(int size)
        {
            string str = "";
            int number = 0;
            for (int i = 0; i < size; ++i)
            {                                                                   //30AA  
                number = rand.Next(Convert.ToInt32("3041", 16), Convert.ToInt32("30FF", 16));
                number = number == 12439 
                    ? number - 1 : number == 12440 
                    ? number + 1 : number;
                str += (char)number;
            }
            return str;                                                  
        }                                                                   
     
        public string Doing(string str)
        {
            ImageDataBase db = new ImageDataBase();
            return db.getImageStringFromDataBase(str);
        }

        public void OnPost(string str)
        {
            name = str;
        }

        public List<string> getImagesFromDataBase()
        {
            return new ImageDataBase().getImagesAsStringFromDatatbase();
        }

        public List<string> getListOfImagesNameFromDB()
        {
            return new DataBase().getNamesOfImagesFromDataBase();
        }
       public List<string> getFilesNameFromDirectory()
        {
           List<string> list = getFilesNameWithArticleFromDirectory();
            for(int i = 0; i < list.Count; ++i)
            {
                list[i] = list[i].Remove(list[i].Length-4);
            }
            return list;
        }
        public List<string> getFilesNameWithArticleFromDirectory()
        {
            List<string> Names = new List<string>();
            DirectoryInfo di = new DirectoryInfo(@"wwwroot\img");

            foreach (var item in di.GetFiles())
            {
                Names.Add(item.Name);
            }

            return Names;
        }

        public void FileDeletion()
        {
            DirectoryInfo di = new DirectoryInfo(@"wwwroot\img");
            foreach (var item in di.GetFiles())
            {
                item.Delete();
            }
        }
       
        public void DownloadFilesFromDataBase(string name)
        {  
           new ImageDataBase().DownloadFileFromDB(name);
        }
    }
}
