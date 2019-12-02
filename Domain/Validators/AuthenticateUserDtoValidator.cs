using EventManager.API.Models;

using FluentValidation;

namespace EventManager.API.Domain.Validators
{
    public class AuthenticateUserDtoValidator : AbstractValidator<AuthenticateUserDto>
    {
        public AuthenticateUserDtoValidator ()
        {
            RuleFor (x => x.Username).NotEmpty ();
            RuleFor (x => x.Password).NotEmpty ();
        }
    }
}
