using SocialMedia.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetPost();

        Task<Post> GetPost(int id);

        Task InsertPost(Post post);

        Task<bool>UpdatePost(Post post);

        Task<bool> DeletePost(int id);

    }
}
