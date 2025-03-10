using BearingManagementApi.Models.Api.Requests;
using FluentValidation;

namespace BearingManagementApi.Validations;

public class BearingRequestValidator : AbstractValidator<BearingRequest>
{
    public BearingRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotNull()
            .NotEmpty();

        RuleFor(request => request.Type)
            .NotNull()
            .NotEmpty();
        RuleFor(request => request.Manufacturer)
            .NotNull()
            .NotEmpty();
    }
}