using FluentValidation;
using PhoneNumbers;
using SubscriptionManagement.Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.Application.Validations
{
    public class SubscribeRequestValidator : AbstractValidator<SubscribeRequest>
    {
        private readonly PhoneNumberUtil _phoneNumberUtil;
        public SubscribeRequestValidator()
        {
            _phoneNumberUtil = PhoneNumberUtil.GetInstance();
            RuleFor(x => x.ServiceId).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.PhoneNumber).Must(BeAValidPhoneNumber).WithMessage("PhoneNumber is not valid for the specified country.");
            RuleFor(x => x.CountryCode)
            .NotEmpty().WithMessage("CountryCode must be provided.");
        }

        private bool BeAValidPhoneNumber(SubscribeRequest req, string phoneNumber)
        {
            try
            {
                var phoneNumberProto = _phoneNumberUtil.Parse(phoneNumber, req.CountryCode);
                return _phoneNumberUtil.IsValidNumber(phoneNumberProto);
            }
            catch (NumberParseException)
            {
                return false;
            }
        }
    }


}
