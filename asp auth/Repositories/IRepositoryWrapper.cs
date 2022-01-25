using asp_auth.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Repositories
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IPostRepository Post { get; }
        IPostReactionRepository PostReaction { get; }
        IFRRepository FriendRequest { get; }
        IFriendRepository Friend { get; }
        IUserProfileRepo UserProfile { get; }
        ICommentRepo Comment { get; }
        IAvatarRepository Avatar { get; }
        ISessionTokenRepository SessionToken { get; }
        Task SaveAsync();
    }
}
