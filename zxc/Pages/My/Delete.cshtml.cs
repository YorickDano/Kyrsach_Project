using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using zxc.Pages.My.Classes;
using zxc.Pages.My.Classes.DataBase;

namespace zxc.Pages.My
{
    public class DeleteModel : PageModel
    {
        public void OnGet()
        {
        }

        public void OnPostDeletion(string name)
        {
            DataBase dataBase = new DataBase();
            dataBase.Deletion(name);
        }

        public void OnPostDeleteAll()
        {
            DataBase db = new DataBase();
            db.DeleteAll();
        }

        public void OnPostDublicateDeletion()
        {
            DataBase dataBase = new DataBase();
            dataBase.deleteDublicationsFromDataBase();
        }
    }
}
