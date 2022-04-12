using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zxc.Pages.My.Classes.DataBase;
using zxc.Pages.My.Classes.Objects;

namespace zxc.Pages
{
    public class IndexModel : PageModel
    {
        public string UserWithThisEmailExist = "";
        public string IncorrectData = "";
        public string MainUrl = "";
        private bool RememberMe;
        UsersDataBase Database = new UsersDataBase();
        

        public void OnGet()
        {

        }

        public void OnPostLogin(string email, string password)
        {
            new Pages.My.Classes.DataBase.ItemDataBase().ClearStatusOfCurrentUser();
            var user = Database.GetUserFromDataBaseByEmail(email);

            if (Database.IsThisUserExistAndDoesntBan(email, password))
            {
                Database.UpdateLastEnterDate(email);
                CurrentUser currentUser = new CurrentUser("", email, password, user.Status);
                Response.Redirect($"https://{Request.Host}/My/Japanese/{user.Id}");
            }
            else
            {
                IncorrectData = "Incorrect email or pasword";
            }
        }

        public void OnPostSignin(string emailSign, string nameSign, string passwordSign)
        {
            new Pages.My.Classes.DataBase.ItemDataBase().ClearStatusOfCurrentUser();
            if (!Database.IsThisEmailExist(emailSign))
            {
                string currentDate = DateTime.Now.ToLocalTime().ToString();
                Database.AddInfoInTable(emailSign, nameSign, passwordSign, currentDate);
                var user = Database.GetUserFromDataBaseByEmail(emailSign);
                new CurrentUser(nameSign,emailSign,passwordSign,user.Status);
                Response.Redirect($"https://{Request.Host}/My/Japanese/{user.Id}");
            }
            else
            {
                UserWithThisEmailExist = "User with this email already exist";
            }
        }

        public bool GetRememberMe()
        {
            return RememberMe;
        }

    }
}
