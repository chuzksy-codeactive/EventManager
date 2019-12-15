using EventManager.API.Models;
using FluentValidation;

namespace EventManager.API.Domain.Validators
{
    public class EventForCreationDtoValidator : AbstractValidator<EventForCreationDto>
    {
        public EventForCreationDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();
            RuleFor(x => x.Note)
                .NotEmpty();
            RuleFor(x => x.Purpose)
                .NotEmpty();
            RuleFor(x => x.ScheduledDate)
                .NotEmpty();
        }
    }
}