using asp_auth.Models;
using asp_auth.Models.Entities;
using lab2.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Repositories
{
    public class SessionTokenRepository: GenericRepository<SessionToken>, ISessionTokenRepository
    {
        public SessionTokenRepository(AppDbContext context): base(context) { }

        public async Task<SessionToken> GetByJti(string jti)
        {
            return await _context.SessionTokens.FirstOrDefaultAsync(t => t.Jti == jti);
        }
    }
}
