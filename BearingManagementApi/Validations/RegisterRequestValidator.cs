using BearingManagementApi.Models.Api.Requests;
using FluentValidation;

namespace BearingManagementApi.Validations;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(request => request.Username)
            .NotNull()
            .NotEmpty();

        RuleFor(request => request.Password)
            .NotNull()
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(128);
    }
}