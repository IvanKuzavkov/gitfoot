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
    [DataContract]
    public partial class LoginRequest
    {
        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string password { get; set; }
    }

    [DataContract]
    public partial class LoginToken
    {
        [DataMember]
        public string token { get; set; }
    }

    [DataContract]
    public partial class AuthorizationRequest
    {
        [DataMember]
        public List<string> scopes { get; set; }

        [DataMember]
        public string note { get; set; }

        [DataMember]
        public string note_url { get; set; }
    }

    [DataContract]
    public partial class AuthorizationResponse
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string url { get; set; }

        [DataMember]
        public List<string> scopes { get; set; }

        [DataMember]
        public string token { get; set; }
    }
}
