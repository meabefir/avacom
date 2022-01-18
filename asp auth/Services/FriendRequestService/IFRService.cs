using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Services
{
    public interface IFRService
    {
        void DeleteFriendRequest(string username, int currentUserId);
    }
}
