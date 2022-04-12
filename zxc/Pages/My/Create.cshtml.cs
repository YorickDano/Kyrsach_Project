using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using zxc.Pages.My.Classes.DataBase;

namespace zxc.Pages.My
{
    public class MusicModel : PageModel
    {
        public void OnGet()
        {
        }

        public void OnPostCreateTable(string name)
        {
            DataBase dataBase = new DataBase();
            dataBase.createTableInDataBase(name);

        }
        public void OnPostCopyFromTo()
        {
            
        }
    }
}
