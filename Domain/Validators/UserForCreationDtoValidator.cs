using EventManager.API.Models;

using FluentValidation;

namespace EventManager.API.Domain.Validators
{
    public class UserForCreationDtoValidator : AbstractValidator<UserForCreationDto>
    {
        public UserForCreationDtoValidator ()
        {
            RuleFor (x => x.Username)
                .NotEmpty ()
                .Matches ("^[a-zA-Z0-9]*$");
            RuleFor (x => x.Firstname)
                .NotEmpty ()
                .Matches ("^[a-zA-Z0-9]*$");
            RuleFor (x => x.Lastname)
                .NotEmpty ()
                .Matches ("^[a-zA-Z0-9]*$");
            RuleFor (x => x.Password)
                .GreaterThan ("6")
                .NotEmpty ();
            RuleFor (x => x.Email)
                .EmailAddress ();
        }
    }
}
