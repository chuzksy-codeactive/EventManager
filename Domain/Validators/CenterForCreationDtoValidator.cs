using EventManager.API.Models;
using FluentValidation;

namespace EventManager.API.Domain.Validators
{
    public class CenterForCreationDtoValidator : AbstractValidator<CenterForCreationDto>
    {
        public CenterForCreationDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();
            RuleFor(x => x.Location)
                .NotEmpty();
            RuleFor(x => x.HallCapacity)
                .NotEmpty();
            RuleFor(x => x.Price)
                .NotEmpty();
            RuleFor(x => x.Facilities)
                .NotNull();
            RuleFor(x => x.Type)
                .IsInEnum();
        }
    }
}