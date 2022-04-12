using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zxc.Pages.My.Classes
{
    public class Image
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public byte[] Data { get; set; }

        public Image(int id,string name,string link,byte[] data)
        {
            ID = id; Name = name; Link = link; Data = data;
        }
    }
}
