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
    public class BaseOwner : BaseModel
    {
        public string url { get; set; }
        public string avatar_url { get; set; }
        public string login { get; set; }
    }

    public class User : BaseOwner
    {
        public string email { get; set; }
        public int public_repos { get; set; }

        protected List<Organization> _organizations = new List<Organization>();
        public List<Organization> Organizations { get { return _organizations; } set { _organizations = value; } } 

    }

    public class Repository : BaseModel
    {
        public string url { get; set; }
        public string full_name { get; set; }
        public string description { get; set; }
        public int open_issues { get; set; }
        public BaseOwner owner { get; set; }
    }

    public class Issue : BaseModel
    {
        public string url { get; set; }
        public int number { get; set; }
        public string title { get; set; }
        public int comments { get; set; }
    }

    public class Organization : BaseOwner
    {
    }
}
