using FluentValidation;
using Microsoft.AspNetCore.Identity;
using SubscriptionManagement.Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.Application.Validations
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.ServiceId).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Password).SetValidator(new PasswordValidator());
        }
    }
}
