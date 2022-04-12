using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zxc.Pages.My.Classes.Objects
{
    public class User
    {
        public int Id;
        public string Name;
        public string Email;
        public string Password;
        public DateTime CreatedDate;
        public DateTime LastUpdatedDate;
        public string Status;
        public bool CheckBox;


        public User(string _name, string _email, string _password)
        {
            Name = _name;
            Email = _email;
            Password = _password;
            
        }

        public User(string _name, string _email, string _password, string _status) 
        {
            Name = _name;
            Email = _email;
            Password = _password;
            Status = _status;
        }

        public User(int _id, string _name, string _email, string _password, string _createData, string _lastUpdatedData, string _status)
        {
            Id = _id;
            Name = _name;
            Email = _email;
            Password = _password;
            CreatedDate = Convert.ToDateTime(_createData);
            LastUpdatedDate = Convert.ToDateTime(_lastUpdatedData);
            Status = _status;
        }
    }
}
