using asp_auth.Models.Entities;
using lab2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Repositories
{
    public interface IUserRepository: IGenericRepository<User>
    {
        Task<List<User>> GetAllUsers();
        Task<User> FindByUsernameAsync(string username);
        Task<User> GetUserByEmail(string email);
        Task<User> GetByIdWithRoles(int id);
        void test();
    }
}
