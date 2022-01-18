using asp_auth.Models.DTOs;
using asp_auth.Models.Entities;
using lab2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Repositories
{
    public interface IUserProfileRepo: IGenericRepository<UserProfile>
    {
        Task<UserProfileView> GetByPk(int pk);
        Task<UserProfile> GetByPkRaw(int pk);
        Task<UserProfileView> GetUserProfile(string username);

    }

}
