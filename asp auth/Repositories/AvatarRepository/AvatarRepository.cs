using asp_auth.Models;
using asp_auth.Models.Entities;
using asp_auth.Models.Views;
using lab2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Repositories
{
    public class AvatarRepository: GenericRepository<Avatar>, IAvatarRepository 
    {
        public AvatarRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<AvatarView> UpdateAvatar(AvatarView dto, int userId)
        {
            var avatar = _context.Avatars.Where(a => a.UserId.Equals(userId)).FirstOrDefault();

            avatar.BodyId = dto.BodyId;
            avatar.ClothingId = dto.ClothingId;
            avatar.BrowsId = dto.BrowsId;
            avatar.EyesId = dto.EyesId;
            avatar.LipsId = dto.LipsId;
            avatar.NoseId = dto.NoseId;
            avatar.HairId = dto.HairId;
            _context.Avatars.Update(avatar);

            await SaveAsync();

            return dto;
        }
    }
}
