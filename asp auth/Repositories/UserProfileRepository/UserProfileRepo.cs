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
            var sender = await _context.Users.Where(u => u.Id == userId).Include(u => u.Avatar).FirstOrDefaultAsync();

            if (up == null)
            {
                Console.WriteLine("ii fac user profile pt ca nu are");
                // we create new user profile entry
                UserProfile new_profile = new UserProfile();
                new_profile.UserId = userId;
                new_profile.Bio = "";
                new_profile.Nickname = "";
                new_profile.User = sender;
                Create(new_profile);

                // we create avatar
                Avatar new_avatar = new Avatar();
                new_avatar.UserId = sender.Id;
                _context.Set<Avatar>().Add(new_avatar);

                await SaveAsync();

                return new UserProfileView
                {
                    Username = sender.UserName,
                    Bio = new_profile.Bio,
                    Nickname = new_profile.Nickname,
                    Age = new_profile.Age,
                    Avatar = new Models.Views.AvatarView
                    {
                        HairId = new_avatar.HairId,
                        EyesId = new_avatar.EyesId,
                        LipsId = new_avatar.LipsId,
                        NoseId = new_avatar.NoseId,
                        ClothingId = new_avatar.ClothingId,
                        BodyId = new_avatar.BodyId,
                        BrowsId = new_avatar.BrowsId
                    }
                };
            }

            return new UserProfileView
            {
                Username = sender.UserName,
                Bio = up.Bio,
                Nickname = up.Nickname,
                Age = up.Age,
                Avatar = new Models.Views.AvatarView
                {
                    HairId = sender.Avatar.HairId,
                    EyesId = sender.Avatar.EyesId,
                    LipsId = sender.Avatar.LipsId,
                    NoseId = sender.Avatar.NoseId,
                    ClothingId = sender.Avatar.ClothingId,
                    BodyId = sender.Avatar.BodyId,
                    BrowsId = sender.Avatar.BrowsId
                }
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

            var user = await _context.Users.Where(u => u.UserName == username).Include(u => u.Avatar).FirstOrDefaultAsync();
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

                // we create avatar
                Avatar new_avatar = new Avatar();
                new_avatar.UserId = user.Id;
                _context.Set<Avatar>().Add(new_avatar);

                await SaveAsync();

                return new UserProfileView
                {
                    Bio = new_profile.Bio,
                    Nickname = new_profile.Nickname,
                    Age = new_profile.Age,
                    Avatar = new Models.Views.AvatarView
                    {
                        HairId = new_avatar.HairId,
                        EyesId = new_avatar.EyesId,
                        LipsId = new_avatar.LipsId,
                        NoseId = new_avatar.NoseId,
                        ClothingId = new_avatar.ClothingId,
                        BodyId = new_avatar.BodyId,
                        BrowsId = new_avatar.BrowsId
                    }
                };
            }

            return new UserProfileView
            {
                Bio = up.Bio,
                Nickname = up.Nickname,
                Age = up.Age,
                Avatar = new Models.Views.AvatarView
                {
                    HairId = user.Avatar.HairId,
                    EyesId = user.Avatar.EyesId,
                    LipsId = user.Avatar.LipsId,
                    NoseId = user.Avatar.NoseId,
                    ClothingId = user.Avatar.ClothingId,
                    BodyId = user.Avatar.BodyId,
                    BrowsId = user.Avatar.BrowsId
                }
            };
        }
    }
}
