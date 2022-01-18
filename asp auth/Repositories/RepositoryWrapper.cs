using asp_auth.Models;
using asp_auth.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly AppDbContext _context;
        private IUserRepository _user;
        private IPostRepository _post;
        private IFRRepository _friend_request;
        private IFriendRepository _friend;
        private IUserProfileRepo _user_profile;
        private IPostReactionRepository _post_reaction;
        private ISessionTokenRepository _sessionToken;

        public RepositoryWrapper(AppDbContext context)
        {
            _context = context;
        }

        public IUserRepository User
        {
            get
            {
                if (_user == null) _user = new UserRepository(_context);
                return _user;
            }
        }

        public IPostRepository Post
        {
            get
            {
                if (_post == null) _post = new PostRepository(_context);
                return _post;
            }
        }

        public IPostReactionRepository PostReaction
        {
            get
            {
                if (_post_reaction == null) _post_reaction = new PostReactionRepository(_context);
                return _post_reaction;
            }
        }

        public IFRRepository FriendRequest
        {
            get
            {
                if (_friend_request == null) _friend_request = new FRRepository(_context);
                return _friend_request;
            }
        }

        public IFriendRepository Friend
        {
            get
            {
                if (_friend == null) _friend = new FriendRepository(_context);
                return _friend;
            }
        }

        public IUserProfileRepo UserProfile
        {
            get
            {
                if (_user_profile == null) _user_profile = new UserProfileRepo(_context);
                return _user_profile;
            }
        }

        public ISessionTokenRepository SessionToken
        {
            get
            {
                if (_sessionToken == null) _sessionToken = new SessionTokenRepository(_context);
                return _sessionToken;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
