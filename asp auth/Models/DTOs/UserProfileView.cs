using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Models.DTOs
{
    public class UserProfileView
    {
        public string Bio { get; set; }
        public string Nickname { get; set; }
        public int Age { get; set; }
    }
}
