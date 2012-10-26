using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace gitfoot.Models
{
    public class User
    {
        public int id { get; set; }
        public string login { get; set; }
        public string avatar_url { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public int public_repos { get; set; }
    }

    public class Repository : BaseModel
    {
//        public int id { get; set; }
        public string url { get; set; }
//        public string name { get; set; }
        public string full_name { get; set; }
        public string description { get; set; }
        public int open_issues { get; set; }
    }
}
