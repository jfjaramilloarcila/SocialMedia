using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class PostMongoRepository : IPostRepository
    {
        public async Task<IEnumerable<Post>> GetPost()
        {
            var posts = Enumerable.Range(1, 10).Select(x => new Post
            {
                PostId = x,
                Description = $"Description Mongo {x}",
                Date = DateTime.Now,
                Image = $"htpps://misapis.con/{x}",
                UserId = x * 2
            });
            await Task.Delay(10);
            return posts;
        }
    }
}
