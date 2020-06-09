using FluentValidation;
using SocialMedia.Core.DTOs;
using System;

namespace SocialMedia.Infrastructure.Validators
{
    public  class PostValidatior : AbstractValidator<PostDto>
    {
        public PostValidatior()
        {
            RuleFor(post => post.Description)
                .NotNull()
                .Length(10, 500);

            RuleFor(post => post.Date)
              .NotNull()
              .GreaterThan(DateTime.Now);  
              
        }
    }
}
