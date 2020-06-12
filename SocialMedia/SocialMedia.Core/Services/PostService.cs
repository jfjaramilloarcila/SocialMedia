﻿using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;

        public PostService(IPostRepository postRepository,IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
        }
               
        public async Task<Post> GetPost(int id)
        {
            return await _postRepository.GetPost(id);
        }

        public async Task<IEnumerable<Post>> GetPost()
        {
            return await _postRepository.GetPost();
        }        

        public async Task InsertPost(Post post)
        {
            var user = await _userRepository.GetUser(post.UserId);
            if (user == null)
            {
                throw new Exception("USER DOESN´T EXIST");
            }
            if (post.Description.Contains("Sexo"))
            {
                throw new Exception("CONTENT NOT ALLOWED");
            }
            await _postRepository.InsertPost(post);
        }

        public async Task<bool> UpdatePost(Post post)
        {
           return await _postRepository.UpdatePost(post);
        }

        public async Task<bool> DeletePost(int id)
        {
            return await _postRepository.DeletePost(id);
        }
    }
}
