using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using zxc.Pages.My.Classes;
using zxc.Pages.My.Classes.DataBase;
using zxc.Pages.My.Classes.Objects;
using System.Web.Razor.Parser;
using System.Net.Http;
using AngleSharp.Html.Parser;
using System.IO;
using HtmlAgilityPack;
namespace zxc.Pages.My
{
    public class ItemModel : PageModel
    {
      
        public string Comment { get; 
            set; }
        public string Id { get; set; }
        public string ImageName { get; set; }
        public string IsRaited = "";
        public string[] TagName = new string[5];
        public void OnGet(string id)
        {
            ImageName = id;
            Id = id.Remove(id.IndexOf('.'));
            SetCurrentUserRate();
        }
        public void OnPost()
        {
         
        }

        public void SetCurrentUserRate()
        {
            ItemDataBase itemDataBase = new ItemDataBase();
            string rate = itemDataBase.GetRate(Id);
            if (rate != null)
            {
                switch (rate)
                {
                    case "1":
                        {
                            SetStarsTags(1, ref TagName);
                            break;
                        }
                    case "2":
                        {
                            SetStarsTags(2, ref TagName);
                            break;
                        }
                    case "3":
                        {
                            SetStarsTags(3, ref TagName);
                            break;
                        }
                    case "4":
                        {
                            SetStarsTags(4, ref TagName);
                            break;
                        }
                    case "5":
                        {
                            SetStarsTags(5, ref TagName);
                            break;
                        }
                    default:
                        {
                            SetStarsTags(0, ref TagName);
                            break;
                        }
                }
            }
            else
            {
                SetStarsTags(0, ref TagName);
            }
        }

        public void SetStarsTags(int rate, ref string[] starsTags)
        {
            for(byte i = 0; i < 5; ++i)
            {
                if (i >= rate)
                {
                    starsTags[i] = "star";
                }
                else
                {
                    starsTags[i] = "colored-star";
                }
            }
        }
       
        public string getDescriptionFromDataBase()
        {
            ItemDataBase dataBase = new ItemDataBase();
            return dataBase.GetDescriptionFromAboutTable(Id);
        }

        public string GetScore()
        {
            return String.Format("{0:f2}", Convert.ToDouble(new ItemDataBase().GetScore(Id)));
        }

        public string GetCountOfRates()
        {
            return new ItemDataBase().GetCountOfRates(Id);
        }

        public void UpdateUserRate(string id, string rate)
        {
            var db = new ItemDataBase();
            db.UpdateRate(id, db.GetCurrentUserEmail(), rate);
        }
        public void OnGetSetRaiting(string id, string rate)
        {
            OnGet(id+".jpg");
            ItemDataBase itemDataBase = new ItemDataBase();
            
            itemDataBase.UpdateScore(id, rate, itemDataBase.GetCurrentUser().Email);
            SetCurrentUserRate();
        }
      
        public void OnPostGetUserComment(string comment)
        {

        }

        public string randJapaneseText(int num)
        {
            Random rand = new Random();
            string str = "";
            int number = 0;
            for (int i = 0; i < num; ++i)
            {
                number = rand.Next(Convert.ToInt32("3041", 16), Convert.ToInt32("30FF", 16));

                number = number == 12439 
                    ? number - 1 : number == 12440 
                    ? number + 1 : number; 
                str += (char)number;
            }
            return str;
        }
        public string getId()
        {
            return Id;
        }

    }
}
