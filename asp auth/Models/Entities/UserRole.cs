﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Models.Entities
{
    public class UserRole: IdentityUserRole<int>
    {
        public Role Role { get; set; }
        public User User { get; set; }
    }
}
