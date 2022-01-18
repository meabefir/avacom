using asp_auth.Models;
using asp_auth.Models.DTOs;
using asp_auth.Models.Entities;
using lab2.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Repositories
{
    public class UserProfileRepo: GenericRepository<UserProfile>, IUserProfileRepo
    {
        public UserProfileRepo(AppDbContext context) : base(context)
        {

        }

        async public Task<UserProfileView> GetByPk(int userId)
        {
            var up = await _context.UserProfiles.Where(up => up.UserId.Equals(userId)).FirstOrDefaultAsync();
            var sender = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

            if (up == null)
            {
                // we create new user profile entry
                UserProfile new_profile = new UserProfile();
                new_profile.UserId = userId;
                new_profile.Bio = "";
                new_profile.Nickname = "";
                new_profile.User = sender;

                Create(new_profile);
                await SaveAsync();

                return new UserProfileView
                {
                    Username = sender.UserName,
                    Bio = new_profile.Bio,
                    Nickname = new_profile.Nickname,
                    Age = new_profile.Age
                };
            }

            return new UserProfileView
            {
                Username = sender.UserName,
                Bio = up.Bio,
                Nickname = up.Nickname,
                Age = up.Age
            };
        }

        public async Task<UserProfile> GetByPkRaw(int userId)
        {
            var up = await _context.UserProfiles.Where(up => up.UserId.Equals(userId)).FirstOrDefaultAsync();
            return up;
        }

        async public Task<UserProfileView> GetUserProfile(string username)
        {
            var up = await _context.UserProfiles.Where(up => up.User.UserName.Equals(username)).FirstOrDefaultAsync();

            var user = await _context.Users.Where(u => u.UserName == username).FirstOrDefaultAsync();
            if (user == null)
                return null;

            if (up == null)
            {
                // we create new user profile entry
                UserProfile new_profile = new UserProfile();
                new_profile.UserId = user.Id;
                new_profile.Bio = "";
                new_profile.Nickname = "";
                Create(new_profile);
                await SaveAsync();

                return new UserProfileView
                {
                    Bio = new_profile.Bio,
                    Nickname = new_profile.Nickname,
                    Age = new_profile.Age
                };
            }

            return new UserProfileView
            {
                Bio = up.Bio,
                Nickname = up.Nickname,
                Age = up.Age
            };
        }
    }
}
