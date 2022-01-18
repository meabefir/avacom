using asp_auth.Models.Entities;
using asp_auth.Models.Views;
using lab2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Repositories
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<List<PostView>> GetPostsByUserId(int UserId);
        Task<List<PostView>> GetFeed(int UserId);
    }
}
