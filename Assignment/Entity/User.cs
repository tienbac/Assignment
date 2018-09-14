using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;

namespace Assignment.Entity
{
    class User
    {
        public override string ToString()
        {
            return name + "--" + email + "--" + phone + "--" + address + "--" + avatar;
        }

        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string avatar { get; set; }
    }
}
