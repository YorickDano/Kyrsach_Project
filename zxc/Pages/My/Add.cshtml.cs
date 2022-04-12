using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using zxc.Pages.My.Classes.DataBase;

namespace zxc.Pages.My
{
    public class AddModel : PageModel
    {
        ItemDataBase dataBase = new ItemDataBase();
        public string Name { get; set; }
        public string Image { get; set; }
        [BindProperty]
        public IFormFile Upload { get; set; }
        
        IWebHostEnvironment _appEnvironment;

        public AddModel(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }
        public void OnPostAdditionName(string name)
        {
            Name = name;
        }
       
        public async Task<IActionResult> OnPostAdditionItem(IFormFileCollection img, string name, string about)
        {
           Name = name + ".jpg";
           string url = "\\img\\";
            if (about.Length != 0)
                dataBase.AddInfoInAboutTable(name, about);

            foreach (var item in img)
            {
                using (FileStream fs = new FileStream(_appEnvironment.WebRootPath + url+Name, FileMode.OpenOrCreate))
                {
                    await  item.CopyToAsync(fs);
                }
                 dataBase.setDataBaseFromFileName(Name);
            }
            Image = Name;
            return null;  
        }
    }
}
