using Asticom_BackendExam.Models.Request;
using FluentValidation;

namespace Asticom_BackendExam.Validations
{
    public class AddUserValidator: AbstractValidator<AddUserRequest>
    {
        public AddUserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty(); //Required
            When(x => !string.IsNullOrEmpty(x.FirstName), () =>
            {
                //add input length restriction
                RuleFor(x => x.FirstName).MinimumLength(3);
                RuleFor(x => x.FirstName).MaximumLength(50);
            });

            RuleFor(x => x.LastName).NotEmpty();//Required
            When(x => !string.IsNullOrEmpty(x.LastName), () =>
            {
                //add input length restriction
                RuleFor(x => x.LastName).MinimumLength(3);
                RuleFor(x => x.LastName).MaximumLength(50);
            });

            RuleFor(x => x.Address).NotEmpty(); //Required
            When(x => !string.IsNullOrEmpty(x.Address), () =>
            {
                //add input length restriction
                RuleFor(x => x.Address).MinimumLength(10);
                RuleFor(x => x.Address).MaximumLength(200);
            });

            RuleFor(x => x.PostCode).NotEmpty(); //Required
            When(x => !string.IsNullOrEmpty(x.Address), () =>
            {
                //add input length restriction
                RuleFor(x => x.PostCode).MaximumLength(10);
            });

            RuleFor(x => x.PhoneNumber).NotEmpty(); // Required
            When(x => !string.IsNullOrEmpty(x.PhoneNumber), () =>
            {
                //add input length restriction
                RuleFor(x => x.PhoneNumber)
                    .Must(isValidPhoneNumber)
                    .WithMessage("This is not a valid phone number. The format must start with '09' followed by the last 9 digits");
            });

            RuleFor(x => x.Email).NotEmpty().EmailAddress();

            //Optional: can add special condition for username
            //(e.g. combination of first and last name)
            RuleFor(x => x.UserName).NotEmpty();

        }

        private bool isValidPhoneNumber(string phoneNumber)
        {
            if (phoneNumber.Length == 11) //process only if length is 11
            {
                for (int x=0; x < phoneNumber.Length; x++)
                {
                    if(x == 0)
                        if(phoneNumber[x] != '0') //hard check for first character
                            return false;
                    if(x == 1)
                        if (phoneNumber[x] != '9') //hard check for second character
                            return false;
                        
                    if (!char.IsDigit(phoneNumber[x])) return false;
                }

                return true;
            }
            else { return false; }
           
            
        }
    }
}
