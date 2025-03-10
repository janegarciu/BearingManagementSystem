using BearingManagementApi.Models.Api.Requests;
using FluentValidation;

namespace BearingManagementApi.Validations;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(request => request.Username)
            .NotNull()
            .NotEmpty();

        RuleFor(request => request.Password)
            .NotNull()
            .NotEmpty();
    }
}