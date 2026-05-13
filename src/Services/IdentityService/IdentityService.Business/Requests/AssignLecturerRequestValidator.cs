using FluentValidation;

namespace IdentityService.Business.Requests;

public class AssignLecturerRequestValidator
    : AbstractValidator<AssignLecturerRequest>
{
    public AssignLecturerRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}