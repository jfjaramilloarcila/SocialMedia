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
                .WithMessage("La descripción no puede ser nula");

            RuleFor(post => post.Description)               
               .Length(10, 500)
               .WithMessage("La longitud de la descripción debe de ser entre 10 y 500 carácteres");

            RuleFor(post => post.Date)
              .NotNull()
              .GreaterThan(DateTime.Now);  
              
        }
    }
}
